using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models
{
    public class Patient
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Range(0, 150)]
        public int Age { get; set; }
        public string Gender { get; set; }

        [Required, StringLength(200)]
        public string Disease { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please assign a doctor")]
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
