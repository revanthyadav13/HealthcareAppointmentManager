using HealthcareAppointmentManager.Models.Data;
using HealthcareAppointmentManager.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareAppointmentManager.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient")]

    public class PatientController : ControllerBase
    {
        private readonly IPatient _patient;

        public PatientController(IPatient patient)
        {
            this._patient = patient;
        }
        [AllowAnonymous]
        [Route("api/PatientRegister")]
        [HttpPost]
        public void PatientRegister(Patient model)
        {
            _patient.PatientRegister(model);
        }

        [Route("api/GetAllPatients")]
        [HttpGet]
        public List<Patient> GetAllPatients()
        {
            List<Patient> patients = new List<Patient>();
            patients = _patient.GetAllPatients();
            return patients;
        }

        [Route("api/GetPatientById/{id:int}")]
        [HttpGet]
        public Patient GetPatientById(int id)
        {
            Patient patient = new Patient();
            patient = _patient.GetPatientById(id);
            return patient;
        }

        [Route("api/GetAppointmentsByPatientId/{id:int}")]
        [HttpGet]

        public List<Appointment> GetAppointmentsByPatientId(int id)
        {
            List<Appointment> appointment = new List<Appointment>();
            appointment = _patient.GetAppointmentsByPatientId(id);
            return appointment;
        }

        [Route("api/GetPatientByUsername/{username}")]
        [HttpGet]

        public Patient GetPatientByUsername(string username)
        {
            Patient patient = new Patient();
            patient = _patient.GetPatientByUsername(username);
            return patient;
        }

        [Route("api/GetAllDoctors")]
        [HttpGet]
        public List<Doctor> GetAllDoctors()
        {
            List<Doctor> doctors = new List<Doctor>();
            doctors = _patient.GetAllDoctors();
            return doctors;
        }
    }
}

