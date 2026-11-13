using Microsoft.JSInterop;
using NationEase.Contracts;

namespace NationEase.Services;

public sealed class LocalAuthenticationService(
    SqliteOpfsDatabase database,
    LocalNationService nation,
    IJSRuntime jsRuntime)
{
    private const string UserIdStorageKey = "nationEaseUserId";

    public event Action? AuthenticationStateChanged;

    public UserResponse? CurrentUser { get; private set; }

    public bool IsAuthenticated => CurrentUser is not null;

    public async Task InitializeAsync()
    {
        await database.InitializeAsync();
        await nation.SeedDefaultCitiesForAllUsersAsync();
        var storedUserId = await jsRuntime.InvokeAsync<string?>("sessionStorage.getItem", UserIdStorageKey);
        if (int.TryParse(storedUserId, out var userId))
        {
            CurrentUser = await nation.GetUserAsync(userId);
            if (CurrentUser is null)
            {
                await jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", UserIdStorageKey);
            }
        }

        AuthenticationStateChanged?.Invoke();
    }

    public async Task<bool> RegisterAsync(string email, string password)
    {
        CurrentUser = await nation.CreateUserAsync(email, password);
        await PersistCurrentUserAsync();
        AuthenticationStateChanged?.Invoke();
        return true;
    }

    public async Task<bool> SignInAsync(string email, string password)
    {
        var user = await database.InvokeAsync<UserResponse>("authenticate", email.Trim(), password);
        if (user is null)
        {
            return false;
        }

        CurrentUser = user;
        await PersistCurrentUserAsync();
        AuthenticationStateChanged?.Invoke();
        return true;
    }

    public async Task<UserResponse> CreateUserAsync(string email, string password) =>
        await nation.CreateUserAsync(email, password);

    public async Task<UserResponse> UpdateUserAsync(int userId, string email, string? password)
    {
        var user = await nation.UpdateUserAsync(userId, email, password);
        if (CurrentUser?.UserId == userId)
        {
            CurrentUser = user;
            AuthenticationStateChanged?.Invoke();
        }

        return user;
    }

    public async Task DeleteUserAsync(int userId)
    {
        await nation.DeleteUserAsync(userId);
        if (CurrentUser?.UserId == userId)
        {
            await SignOutAsync();
        }
    }

    public async Task SignOutAsync()
    {
        CurrentUser = null;
        await jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", UserIdStorageKey);
        AuthenticationStateChanged?.Invoke();
    }

    private Task PersistCurrentUserAsync() =>
        jsRuntime.InvokeVoidAsync(
            "sessionStorage.setItem",
            UserIdStorageKey,
            CurrentUser!.UserId.ToString()).AsTask();
}
