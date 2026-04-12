using EventEase.Models;

namespace EventEase.Services
{
    /// <summary>
    /// Manages the current user session state for the application
    /// </summary>
    public interface IUserSessionService
    {
        User? CurrentUser { get; }
        bool IsLoggedIn { get; }
        Task LoginAsync(User user);
        Task LogoutAsync();
        Task UpdateCurrentUserAsync(User user);
        event Action? OnSessionChanged;
    }
}
