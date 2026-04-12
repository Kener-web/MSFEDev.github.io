using EventEase.Models;

namespace EventEase.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly List<EventAttendance> _attendances = new();
        private int _nextId = 1;

        public Task<List<EventAttendance>> GetAllAttendancesAsync()
        {
            return Task.FromResult(_attendances.ToList());
        }

        public Task<EventAttendance?> GetAttendanceByIdAsync(int id)
        {
            var attendance = _attendances.FirstOrDefault(a => a.Id == id);
            return Task.FromResult(attendance);
        }

        public Task<List<EventAttendance>> GetAttendancesByUserIdAsync(int userId)
        {
            var attendances = _attendances.Where(a => a.UserId == userId).ToList();
            return Task.FromResult(attendances);
        }

        public Task<List<EventAttendance>> GetAttendancesByEventIdAsync(int eventId)
        {
            var attendances = _attendances.Where(a => a.EventId == eventId).ToList();
            return Task.FromResult(attendances);
        }

        public Task<EventAttendance?> GetAttendanceAsync(int userId, int eventId)
        {
            var attendance = _attendances.FirstOrDefault(a => a.UserId == userId && a.EventId == eventId);
            return Task.FromResult(attendance);
        }

        public Task AddAttendanceAsync(EventAttendance attendance)
        {
            attendance.Id = _nextId++;
            attendance.RegistrationDate = DateTime.Now;
            _attendances.Add(attendance);
            return Task.CompletedTask;
        }

        public Task UpdateAttendanceAsync(EventAttendance attendance)
        {
            var existing = _attendances.FirstOrDefault(a => a.Id == attendance.Id);
            if (existing != null)
            {
                existing.Attended = attendance.Attended;
                existing.AttendanceDate = attendance.AttendanceDate;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAttendanceAsync(int id)
        {
            var attendance = _attendances.FirstOrDefault(a => a.Id == id);
            if (attendance != null)
            {
                _attendances.Remove(attendance);
            }
            return Task.CompletedTask;
        }

        public Task<bool> IsUserRegisteredForEventAsync(int userId, int eventId)
        {
            var exists = _attendances.Any(a => a.UserId == userId && a.EventId == eventId);
            return Task.FromResult(exists);
        }

        public Task MarkAttendanceAsync(int attendanceId)
        {
            var attendance = _attendances.FirstOrDefault(a => a.Id == attendanceId);
            if (attendance != null)
            {
                attendance.Attended = true;
                attendance.AttendanceDate = DateTime.Now;
            }
            return Task.CompletedTask;
        }
    }
}
