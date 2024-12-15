using HealthcareAppointmentManager.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthcareAppointmentManager.Models.Repositories
{
    public class AppointmentRepository:IAppointment
    {
        private AppDbContext _appDbContext;
        private string myConnectionString;
        public AppointmentRepository(AppDbContext appDbContext, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            myConnectionString = configuration.GetConnectionString("HealthCareConnectionString");
        }
        public List<Appointment> GetAllAppointments()
        {
            List<Appointment> appointments = new List<Appointment>();
            var getdata = _appDbContext.Appointments.FromSqlRaw("GetAllAppointments");
            foreach (var appointment in getdata)
            {
                appointments.Add(new Appointment()
                {
                    AppointmentID = appointment.AppointmentID,
                    DoctorID = appointment.DoctorID,
                    PatientID = appointment.PatientID,
                    AppointmentDate = appointment.AppointmentDate,
                    AppointmentTime = appointment.AppointmentTime,
                    AppointmentStatus = appointment.AppointmentStatus,
                    PurposeDescription = appointment.PurposeDescription
                });
            }
            return appointments;
        }

        public Appointment GetAppointmentById(int id)
        {
            Appointment appointment = new Appointment();
            var getdata = _appDbContext.Appointments.FromSqlRaw("GetAppointmentDataById @appointment_id={0}", id).ToList().FirstOrDefault();
            if (getdata != null)
            {
                appointment.AppointmentID = getdata.AppointmentID;
                appointment.DoctorID = getdata.DoctorID;
                appointment.PatientID = getdata.PatientID;
                appointment.AppointmentDate = getdata.AppointmentDate;
                appointment.AppointmentTime = getdata.AppointmentTime;
                appointment.AppointmentStatus = getdata.AppointmentStatus;
                appointment.PurposeDescription = getdata.PurposeDescription;
            };

            return appointment;
        }

        public void SaveAppointment(Appointment appointment)
        {
            _appDbContext.Database.ExecuteSqlRaw("SaveAppointmentData  @patient_id={0}, @doctor_id={1}, @appointment_date={2}, @appointment_time={3},@appointment_status={4},@purpose_description={5}"
                , appointment.PatientID, appointment.DoctorID, appointment.AppointmentDate, appointment.AppointmentTime, appointment.AppointmentStatus, appointment.PurposeDescription);
        }

        public void UpdateAppointment(Appointment appointment)
        {
            _appDbContext.Database.ExecuteSqlRaw("UpdateAppointmentData @appointment_status={0}, @appointment_id={1}", appointment.AppointmentStatus, appointment.AppointmentID);
        }
    }
}
