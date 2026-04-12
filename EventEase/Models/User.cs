using System;
using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Birthdate is required.")]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Navigation property for attendances
        public List<EventAttendance> Attendances { get; set; } = new();

        // Calculate age from birthdate
        public int Age => DateTime.Now.Year - Birthdate.Year - 
            (Birthdate.AddYears(DateTime.Now.Year - Birthdate.Year) > DateTime.Now ? 1 : 0);
    }
}
