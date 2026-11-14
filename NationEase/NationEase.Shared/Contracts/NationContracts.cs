using System.ComponentModel.DataAnnotations;
namespace NationEase.Contracts;

/// <summary>State data displayed by the client.</summary>
public sealed record StateResponse(int StateId, DateOnly Date, string Acronym, string Name);

/// <summary>State fields required to create a state.</summary>
public sealed record CreateStateRequest
{
    [Required, MaxLength(2)]
    public required string Acronym { get; init; }

    [Required, MaxLength(100)]
    public required string Name { get; init; }

    public required DateOnly Date { get; init; }
}

/// <summary>State fields used to replace an existing state.</summary>
public sealed record UpdateStateRequest
{
    [Required, MaxLength(2)]
    public required string Acronym { get; init; }

    [Required, MaxLength(100)]
    public required string Name { get; init; }

    public required DateOnly Date { get; init; }
}

/// <summary>City data displayed by the client.</summary>
public sealed record CityResponse(
    int CityId,
    int StateId,
    string StateAcronym,
    string Name,
    decimal Population,
    DateOnly? FoundingDate,
    decimal? Latitude,
    decimal? Longitude);

/// <summary>City fields required to create a city.</summary>
public sealed record CreateCityRequest
{
    [Range(1, int.MaxValue)]
    public required int StateId { get; init; }

    [Required, MaxLength(150)]
    public required string Name { get; init; }

    [Range(0, long.MaxValue)]
    public required decimal Population { get; init; }

    public DateOnly? FoundingDate { get; init; }

    [Range(typeof(decimal), "-90", "90")]
    public decimal? Latitude { get; init; }

    [Range(typeof(decimal), "-180", "180")]
    public decimal? Longitude { get; init; }
}

/// <summary>City fields used to replace an existing city.</summary>
public sealed record UpdateCityRequest
{
    [Range(1, int.MaxValue)]
    public required int StateId { get; init; }

    [Required, MaxLength(150)]
    public required string Name { get; init; }

    [Range(0, long.MaxValue)]
    public required decimal Population { get; init; }

    public DateOnly? FoundingDate { get; init; }

    [Range(typeof(decimal), "-90", "90")]
    public decimal? Latitude { get; init; }

    [Range(typeof(decimal), "-180", "180")]
    public decimal? Longitude { get; init; }
}

/// <summary>A browser-local user account.</summary>
public sealed record UserResponse(int UserId, string Email);
