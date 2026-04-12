using EventEase.Models;

namespace EventEase.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users = new();
        private int _nextId = 1;

        public Task<List<User>> GetAllUsersAsync()
        {
            return Task.FromResult(_users.ToList());
        }

        public Task<User?> GetUserByIdAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(user);
        }

        public Task<User?> GetUserByEmailAsync(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email == email);
            return Task.FromResult(user);
        }

        public Task AddUserAsync(User user)
        {
            user.Id = _nextId++;
            user.RegistrationDate = DateTime.Now;
            _users.Add(user);
            return Task.CompletedTask;
        }

        public Task UpdateUserAsync(User user)
        {
            var existing = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existing != null)
            {
                existing.Name = user.Name;
                existing.Email = user.Email;
                existing.Birthdate = user.Birthdate;
            }
            return Task.CompletedTask;
        }

        public Task DeleteUserAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _users.Remove(user);
            }
            return Task.CompletedTask;
        }
    }
}
