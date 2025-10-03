using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HospitalManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;

        public HomeController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            string role = HttpContext.Session.GetString("UserRole");
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (role == "Admin")
            {
                ViewBag.DoctorsCount = context.Doctors.Count();
                ViewBag.PatientsCount = context.Patients.Count();
                ViewBag.AppointmentsCount = context.Appointments.Count();

                // 5 recent appointments for all
                var recentAppointments = context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .OrderByDescending(a => a.AppointmentDateTime)
                    .Take(5)
                    .ToList();

                return View(recentAppointments);
            }
            else if (role == "Doctor")
            {
                var doctor = context.Doctors.FirstOrDefault(d => d.UserId == userId);
                if (doctor != null)
                {
                    int doctorId = doctor.Id;

                    ViewBag.DoctorsCount = 1;
                    ViewBag.PatientsCount = context.Appointments
                                            .Where(a => a.DoctorId == doctorId)
                                            .Select(a => a.PatientId).Distinct().Count();
                    ViewBag.AppointmentsCount = context.Appointments
                                            .Where(a => a.DoctorId == doctorId).Count();

                    var recentAppointments = context.Appointments
                        .Include(a => a.Patient)
                        .Include(a => a.Doctor)
                        .Where(a => a.DoctorId == doctorId)
                        .OrderByDescending(a => a.AppointmentDateTime)
                        .Take(5)
                        .ToList();

                    return View(recentAppointments);
                }
                else
                {
                    // Doctor not found ? return empty dashboard
                    ViewBag.DoctorsCount = 0;
                    ViewBag.PatientsCount = 0;
                    ViewBag.AppointmentsCount = 0;
                    return View(new List<Appointment>());
                }
            }

            else if (role == "Nurse")
            {
                // Count patients, appointments (all patients can be seen by nurse)
                ViewBag.DoctorsCount = context.Doctors.Count();
                ViewBag.PatientsCount = context.Patients.Count();
                ViewBag.AppointmentsCount = context.Appointments.Count();

                var recentAppointments = context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .OrderByDescending(a => a.AppointmentDateTime)
                    .Take(5)
                    .ToList();

                return View(recentAppointments);
            }
            else
            {
                // Default: empty dashboard
                ViewBag.DoctorsCount = 0;
                ViewBag.PatientsCount = 0;
                ViewBag.AppointmentsCount = 0;
                return View(new List<Appointment>());
            }
        }
    }


    }

