using System;
using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class EventAttendance
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Registration date is required.")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public bool Attended { get; set; } = false;

        public DateTime? AttendanceDate { get; set; }

        // Navigation properties
        public User? User { get; set; }
        public Event? Event { get; set; }
    }
}
