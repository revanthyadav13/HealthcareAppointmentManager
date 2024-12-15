using HealthcareAppointmentManager.Models.Data;
using HealthcareAppointmentManager.Models.Repositories;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HealthcareAppointmentManager.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Doctor, Patient")]
    public class AppointmentController : ControllerBase
    {
        private IAppointment _appointment;
        private readonly ILogger<AppointmentController> logger;

        public AppointmentController(IAppointment appointment, ILogger<AppointmentController> logger)
        {
            this._appointment = appointment;
            this.logger = logger;
        }
        [Route("api/GetAllAppointments")]
        [HttpGet]
        public List<Appointment> GetAllAppointments()
        {
            logger.LogInformation("Get All action method was invoked");
            List<Appointment> appointments = new List<Appointment>();
            appointments = _appointment.GetAllAppointments();
            logger.LogInformation($"GetAllAppointments completed successfully.{JsonSerializer.Serialize(appointments)}");
            return appointments;
        }

        [Route("api/SaveAppointmentData")]
        [HttpPost]
        public void SaveEmployeeData(Appointment model)
        {
            _appointment.SaveAppointment(model);
        }

        [Route("api/GetAppointmentById/{id:int}")]
        [HttpGet]
        public Appointment GetAppointmentById(int id)
        {
            Appointment appointment = new Appointment();
            appointment = _appointment.GetAppointmentById(id);
            return appointment;
        }

        [Route("api/UpdateAppointmentData")]
        [HttpPost]
        public void UpdateEmployeeData(Appointment model)
        {
            _appointment.UpdateAppointment(model);
        }
    }
}
