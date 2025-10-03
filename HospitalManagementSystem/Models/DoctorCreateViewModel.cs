using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models
{
    public class DoctorCreateViewModel
    {
        public string Name { get; set; }
        public string Specialty { get; set; }
        
        public string Degree { get; set; } 
        public int YearsOfExperience { get; set; }
        
         
        
        public int UserId { get; set; }
       
        
        public List<User> Users { get; set; } = new List<User>();
        public List<Doctor> Doctors { get; set; } = new List<Doctor>();
        public List<Patient> Patients { get; set; } = new List<Patient>();
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();

    }
}
