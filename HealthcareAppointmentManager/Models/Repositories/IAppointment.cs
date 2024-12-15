using HealthcareAppointmentManager.Models.Data;

namespace HealthcareAppointmentManager.Models.Repositories
{
    public interface IAppointment
    {
        public List<Appointment> GetAllAppointments();

        public void SaveAppointment(Appointment appointment);

        public Appointment GetAppointmentById(int id);
        public void UpdateAppointment(Appointment appointment);
    }
}
