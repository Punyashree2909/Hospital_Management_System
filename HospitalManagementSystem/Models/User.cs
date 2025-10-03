using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HospitalManagementSystem.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } = string.Empty; // store hashed

        [Required]
        public string Role { get; set; } = "Admin"; // Admin, Doctor, Nurse
        public Doctor Doctor { get; set; }
    }
}
