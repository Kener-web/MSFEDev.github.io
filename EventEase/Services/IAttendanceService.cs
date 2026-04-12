using EventEase.Models;

namespace EventEase.Services
{
    public interface IAttendanceService
    {
        Task<List<EventAttendance>> GetAllAttendancesAsync();
        Task<EventAttendance?> GetAttendanceByIdAsync(int id);
        Task<List<EventAttendance>> GetAttendancesByUserIdAsync(int userId);
        Task<List<EventAttendance>> GetAttendancesByEventIdAsync(int eventId);
        Task<EventAttendance?> GetAttendanceAsync(int userId, int eventId);
        Task AddAttendanceAsync(EventAttendance attendance);
        Task UpdateAttendanceAsync(EventAttendance attendance);
        Task DeleteAttendanceAsync(int id);
        Task<bool> IsUserRegisteredForEventAsync(int userId, int eventId);
        Task MarkAttendanceAsync(int attendanceId);
    }
}
