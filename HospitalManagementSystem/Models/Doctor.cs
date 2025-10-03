using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HospitalManagementSystem.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Specialty { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Degree { get; set; } = string.Empty;
        public string Email { get; set; }
        [Range(0, 50)]
        public int YearsOfExperience { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Patient> Patients { get; set; } = new List<Patient>();


    }
}
