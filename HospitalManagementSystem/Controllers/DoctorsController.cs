using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;




namespace HospitalManagementSystem.Controllers
{

    [AuthFilter("Admin")]
    public class DoctorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Doctors
        public async Task<IActionResult> Index()
        {
           
            var doctors = await _context.Doctors
       .Include(d => d.Patients)
       .Include(d => d.Appointments)
       .Include(d => d.User)
       .ToListAsync();
            return View(doctors);
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            // ✅ Get all UserIds already assigned to a Doctor
            var assignedUserIds = _context.Doctors
                .Where(d => d.UserId != null)
                .Select(d => d.UserId)
                .ToList();

            // ✅ Get users that are not yet assigned AND have role 'Doctor'
            var users = _context.Users
                .Where(u => !assignedUserIds.Contains(u.Id) && u.Role == "Doctor")
                .ToList();

            // ✅ Now use 'users' to populate the view model
            var viewModel = new DoctorCreateViewModel
            {
                Doctors = _context.Doctors.ToList(),
                Users = users
            };
            return View(viewModel);
        }

        //POST: Doctors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorCreateViewModel model)
        {
       
            var assignedUserIds = _context.Doctors.Select(d => d.UserId).ToList();
            model.Users = _context.Users
                .Where(u => !assignedUserIds.Contains(u.Id) && u.Role == "Doctor")
                .ToList();
            model.Doctors = _context.Doctors.ToList();

            if (ModelState.IsValid)
            {
                bool alreadyAssigned = await _context.Doctors.AnyAsync(d => d.UserId == model.UserId);
                if (alreadyAssigned)
                {
                    ModelState.AddModelError("UserId", "This user is already assigned to another doctor profile.");
                    return View(model);
                }
                // ✅ Get the selected User to copy email
                var selecteduser = await _context.Users.FindAsync(model.UserId);
                if (selecteduser == null)
                {
                    ModelState.AddModelError("UserId", "Selected user does not exist.");
                    return View(model);
                }
                
                {
                    var doctor = new Doctor
                    {
                        Name = model.Name,
                        Specialty = model.Specialty,
                        Degree = model.Degree,
                        YearsOfExperience = model.YearsOfExperience,
                        UserId = model.UserId,
                        Email = selecteduser.Email
                    };

                    _context.Doctors.Add(doctor);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }




        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
           
            ViewBag.UserId = new SelectList(_context.Users, "Id", "Email", doctor.UserId);
            return View(doctor);

        }

        // POST: Doctors/Edit/5
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Specialty,UserId")] Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", doctor.UserId);
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.Id == id);

        }
    }
}