using EventEase.Models;

namespace EventEase.Services
{
    /// <summary>
    /// Manages the current user session state for the application
    /// </summary>
    public class UserSessionService : IUserSessionService
    {
        private User? _currentUser;

        public User? CurrentUser => _currentUser;

        public bool IsLoggedIn => _currentUser != null;

        public event Action? OnSessionChanged;

        public Task LoginAsync(User user)
        {
            _currentUser = user;
            NotifySessionChanged();
            return Task.CompletedTask;
        }

        public Task LogoutAsync()
        {
            _currentUser = null;
            NotifySessionChanged();
            return Task.CompletedTask;
        }

        public Task UpdateCurrentUserAsync(User user)
        {
            if (_currentUser != null && _currentUser.Id == user.Id)
            {
                _currentUser = user;
                NotifySessionChanged();
            }
            return Task.CompletedTask;
        }

        private void NotifySessionChanged()
        {
            OnSessionChanged?.Invoke();
        }
    }
}
