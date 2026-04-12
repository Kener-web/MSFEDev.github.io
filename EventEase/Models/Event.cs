using System;
using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Event name is required.")]
        public string? Name { get; set; }
        public string? Location { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }

        // Navigation property for attendances
        public List<EventAttendance> Attendances { get; set; } = new();
    }
}