using System.Text.Json;
using HealthcareAppointmentManager.Models.Data;
using HealthcareAppointmentManager.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HealthcareAppointmentManager.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Doctor")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctor _doctor;

        public DoctorController(IDoctor doctor)
        {
            this._doctor = doctor;
        }
        [AllowAnonymous]
        [Route("api/DoctorRegister")]
        [HttpPost]
        public void DoctorRegister(Doctor model)
        {
            _doctor.DoctorRegister(model);
        }

        [Route("api/GetDoctorById/{id:int}")]
        [HttpGet]
        public Doctor GetDoctorById(int id)
        {
            Doctor doctor = new Doctor();
            doctor = _doctor.GetDoctorById(id);
            return doctor;
        }

        [Route("api/GetAppointmentsByDoctorId/{id:int}")]
        [HttpGet]

        public List<Appointment> GetAppointmentsByDoctorId(int id)
        {
            List<Appointment> appointment = new List<Appointment>();
            appointment = _doctor.GetAppointmentsByDoctorId(id);
            return appointment;
        }

        [Route("api/GetDoctorByUsername/{username}")]
        [HttpGet]

        public Doctor GetDoctorByUsername(string username)
        {
            Doctor doctor = new Doctor();
            doctor = _doctor.GetDoctorByUsername(username);
            return doctor;
        }
    }
}
