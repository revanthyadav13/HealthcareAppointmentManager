using HealthcareAppointmentManager.Models.Data;

namespace HealthcareAppointmentManager.Models.Repositories
{
    public interface IDoctor
    {
        
        public void DoctorRegister(Doctor doctor);
        public Doctor GetDoctorById(int id);

        public Doctor ValidateDoctor(string username, string password);
        public Doctor GetDoctorByUsername(string username);

        public List<Appointment> GetAppointmentsByDoctorId(int id);
    }
}
