using NationEase.Contracts;
using NationEase.Data;
using NationEase.Models;

namespace NationEase.Services;

public sealed class LocalNationService(SqliteOpfsDatabase database)
{
    public async Task<UserResponse?> GetUserAsync(int userId) =>
        (await database.QueryAsync<UserResponse>(
            "SELECT UserId, Email FROM Users WHERE UserId = ?",
            userId)).FirstOrDefault();

    public Task<List<UserResponse>> GetUsersAsync() =>
        database.QueryAsync<UserResponse>("SELECT UserId, Email FROM Users ORDER BY Email");

    public async Task<UserResponse> CreateUserAsync(string email, string password)
    {
        PasswordPolicy.EnsureValid(password);
        email = NormalizeEmail(email);
        await EnsureEmailAvailableAsync(email);

        var stateRows = StateSeedData.States.Select((state, index) => new object?[]
        {
            index + 1,
            state.Date,
            state.Acronym,
            state.Name
        }).ToArray();
        return await database.InvokeAsync<UserResponse>(
            "createUser", email, password, stateRows, CreateCitySeedRows())
            ?? throw new InvalidOperationException("The user account could not be created.");
    }

    public async Task SeedDefaultCitiesForAllUsersAsync()
    {
        foreach (var user in await GetUsersAsync())
        {
            await SeedDefaultCitiesAsync(user.UserId);
        }
    }

    private async Task SeedDefaultCitiesAsync(int userId)
    {
        await database.InvokeAsync<bool>("seedDefaultCities", userId, CreateCitySeedRows());
    }

    private static object?[][] CreateCitySeedRows()
    {
        var stateIds = StateSeedData.States
            .Select((state, index) => (state.Acronym, StateId: index + 1))
            .ToDictionary(state => state.Acronym, state => state.StateId);
        return CitySeedData.Cities.Select((city, index) => new object?[]
        {
            index + 1,
            stateIds[city.StateAcronym],
            city.Name,
            city.Population
        }).ToArray();
    }

    public async Task<UserResponse> UpdateUserAsync(int userId, string email, string? password)
    {
        if (!string.IsNullOrEmpty(password))
        {
            PasswordPolicy.EnsureValid(password);
        }

        email = NormalizeEmail(email);
        await EnsureEmailAvailableAsync(email, userId);

        return await database.InvokeAsync<UserResponse>("updateUser", userId, email, password)
            ?? throw new KeyNotFoundException("The user account no longer exists.");
    }

    public async Task DeleteUserAsync(int userId)
    {
        var affected = await database.ExecuteAsync("DELETE FROM Users WHERE UserId = ?", userId);
        if (affected == 0)
        {
            throw new KeyNotFoundException("The user account no longer exists.");
        }
    }

    public Task<List<StateResponse>> GetStatesAsync(int userId) =>
        database.QueryAsync<StateResponse>(
            """
            SELECT StateId, Date, Acronym, Name
            FROM States
            WHERE OwnerUserId = ?
            ORDER BY StateId
            """,
            userId);

    public async Task<StateResponse> CreateStateAsync(int userId, CreateStateRequest request)
    {
        await EnsureAcronymAvailableAsync(userId, request.Acronym);
        await database.ExecuteAsync(
            """
            INSERT INTO States (StateId, OwnerUserId, Date, Acronym, Name)
            SELECT COALESCE(MAX(StateId), 0) + 1, ?, ?, ?, ?
            FROM States
            WHERE OwnerUserId = ?
            """,
            userId,
            request.Date,
            request.Acronym.Trim().ToUpperInvariant(),
            request.Name.Trim(),
            userId);

        return (await database.QueryAsync<StateResponse>(
            """
            SELECT StateId, Date, Acronym, Name
            FROM States
            WHERE OwnerUserId = ? AND Acronym = ? COLLATE NOCASE
            """,
            userId,
            request.Acronym.Trim())).Single();
    }

    public async Task<StateResponse> UpdateStateAsync(int userId, int stateId, UpdateStateRequest request)
    {
        await EnsureAcronymAvailableAsync(userId, request.Acronym, stateId);
        var affected = await database.ExecuteAsync(
            """
            UPDATE States
            SET Date = ?, Acronym = ?, Name = ?
            WHERE StateId = ? AND OwnerUserId = ?
            """,
            request.Date,
            request.Acronym.Trim().ToUpperInvariant(),
            request.Name.Trim(),
            stateId,
            userId);
        EnsureUpdated(affected, "The state no longer exists.");

        return (await database.QueryAsync<StateResponse>(
            "SELECT StateId, Date, Acronym, Name FROM States WHERE StateId = ? AND OwnerUserId = ?",
            stateId,
            userId)).Single();
    }

    public async Task DeleteStateAsync(int userId, int stateId)
    {
        var stateExists = (await database.QueryAsync<CountRow>(
            "SELECT COUNT(*) AS Count FROM States WHERE StateId = ? AND OwnerUserId = ?",
            stateId,
            userId)).Single().Count > 0;
        if (!stateExists)
        {
            throw new KeyNotFoundException("The state no longer exists.");
        }

        var cityCount = (await database.QueryAsync<CountRow>(
            "SELECT COUNT(*) AS Count FROM Cities WHERE StateId = ? AND OwnerUserId = ?",
            stateId,
            userId)).Single().Count;
        if (cityCount > 0)
        {
            throw new InvalidOperationException("Delete the state's cities before deleting the state.");
        }

        await database.ExecuteAsync(
            "DELETE FROM States WHERE StateId = ? AND OwnerUserId = ?",
            stateId,
            userId);
    }

    public Task<List<CityResponse>> GetCitiesAsync(int userId) =>
        database.QueryAsync<CityResponse>(
            """
            SELECT city.CityId, city.StateId, state.Acronym AS StateAcronym, city.Name,
                   city.Population, city.FoundingDate
            FROM Cities AS city
            INNER JOIN States AS state
                ON state.OwnerUserId = city.OwnerUserId AND state.StateId = city.StateId
            WHERE city.OwnerUserId = ?
            ORDER BY city.Name
            """,
            userId);

    public async Task<CityResponse> CreateCityAsync(int userId, CreateCityRequest request)
    {
        await EnsureStateExistsAsync(userId, request.StateId);
        var insertedCity = (await database.QueryAsync<IdRow>(
            """
            INSERT INTO Cities (CityId, OwnerUserId, StateId, Name, Population, FoundingDate)
            SELECT COALESCE(MAX(CityId), 0) + 1, ?, ?, ?, ?, ?
            FROM Cities
            WHERE OwnerUserId = ?
            RETURNING CityId AS Id
            """,
            userId,
            request.StateId,
            request.Name.Trim(),
            request.Population,
            request.FoundingDate,
            userId)).Single();

        return (await GetCityByIdAsync(userId, insertedCity.Id)).Single();
    }

    public async Task<CityResponse> UpdateCityAsync(int userId, int cityId, UpdateCityRequest request)
    {
        await EnsureStateExistsAsync(userId, request.StateId);
        var affected = await database.ExecuteAsync(
            """
            UPDATE Cities
            SET StateId = ?, Name = ?, Population = ?, FoundingDate = ?
            WHERE CityId = ? AND OwnerUserId = ?
            """,
            request.StateId,
            request.Name.Trim(),
            request.Population,
            request.FoundingDate,
            cityId,
            userId);
        EnsureUpdated(affected, "The city no longer exists.");

        return (await GetCityByIdAsync(userId, cityId)).Single();
    }

    public async Task DeleteCityAsync(int userId, int cityId)
    {
        var affected = await database.ExecuteAsync(
            "DELETE FROM Cities WHERE CityId = ? AND OwnerUserId = ?",
            cityId,
            userId);
        EnsureUpdated(affected, "The city no longer exists.");
    }

    public Task<List<CityResponse>> GetCartAsync(int userId) =>
        database.QueryAsync<CityResponse>(
            """
            SELECT city.CityId, city.StateId, state.Acronym AS StateAcronym, city.Name,
                   city.Population, city.FoundingDate
            FROM UserCities AS selected
            INNER JOIN Cities AS city
                ON city.OwnerUserId = selected.UserId AND city.CityId = selected.CityId
            INNER JOIN States AS state
                ON state.OwnerUserId = city.OwnerUserId AND state.StateId = city.StateId
            WHERE selected.UserId = ?
            ORDER BY city.Name
            """,
            userId);

    public async Task AddToCartAsync(int userId, int cityId)
    {
        var cityExists = (await database.QueryAsync<CountRow>(
            "SELECT COUNT(*) AS Count FROM Cities WHERE CityId = ? AND OwnerUserId = ?",
            cityId,
            userId)).Single().Count > 0;
        if (!cityExists)
        {
            throw new KeyNotFoundException("The city no longer exists.");
        }

        await database.ExecuteAsync(
            "INSERT OR IGNORE INTO UserCities (UserId, CityId) VALUES (?, ?)",
            userId,
            cityId);
    }

    public Task RemoveFromCartAsync(int userId, int cityId) =>
        database.ExecuteAsync(
            "DELETE FROM UserCities WHERE UserId = ? AND CityId = ?",
            userId,
            cityId);

    public Task ClearCartAsync(int userId) =>
        database.ExecuteAsync("DELETE FROM UserCities WHERE UserId = ?", userId);

    private async Task EnsureEmailAvailableAsync(string email, int? exceptUserId = null)
    {
        var matches = exceptUserId is int userId
            ? await database.QueryAsync<CountRow>(
                "SELECT COUNT(*) AS Count FROM Users WHERE Email = ? COLLATE NOCASE AND UserId <> ?",
                email,
                userId)
            : await database.QueryAsync<CountRow>(
                "SELECT COUNT(*) AS Count FROM Users WHERE Email = ? COLLATE NOCASE",
                email);

        if (matches.Single().Count > 0)
        {
            throw new InvalidOperationException("A user with that email address already exists.");
        }
    }

    private async Task EnsureAcronymAvailableAsync(int userId, string acronym, int? exceptStateId = null)
    {
        var matches = exceptStateId is int stateId
            ? await database.QueryAsync<CountRow>(
                "SELECT COUNT(*) AS Count FROM States WHERE OwnerUserId = ? AND Acronym = ? COLLATE NOCASE AND StateId <> ?",
                userId,
                acronym.Trim(),
                stateId)
            : await database.QueryAsync<CountRow>(
                "SELECT COUNT(*) AS Count FROM States WHERE OwnerUserId = ? AND Acronym = ? COLLATE NOCASE",
                userId,
                acronym.Trim());

        if (matches.Single().Count > 0)
        {
            throw new InvalidOperationException("A state with that acronym already exists in this account.");
        }
    }

    private async Task EnsureStateExistsAsync(int userId, int stateId)
    {
        var count = (await database.QueryAsync<CountRow>(
            "SELECT COUNT(*) AS Count FROM States WHERE StateId = ? AND OwnerUserId = ?",
            stateId,
            userId)).Single().Count;
        if (count == 0)
        {
            throw new InvalidOperationException("Select a state that belongs to this user account.");
        }
    }

    private Task<List<CityResponse>> GetCityByIdAsync(int userId, int cityId) =>
        database.QueryAsync<CityResponse>(
            """
            SELECT city.CityId, city.StateId, state.Acronym AS StateAcronym, city.Name,
                   city.Population, city.FoundingDate
            FROM Cities AS city
            INNER JOIN States AS state
                ON state.OwnerUserId = city.OwnerUserId AND state.StateId = city.StateId
            WHERE city.OwnerUserId = ? AND city.CityId = ?
            """,
            userId,
            cityId);

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();

    private static void EnsureUpdated(int affected, string message)
    {
        if (affected == 0)
        {
            throw new KeyNotFoundException(message);
        }
    }

    private sealed record CountRow(int Count);

    private sealed record IdRow(int Id);

}
