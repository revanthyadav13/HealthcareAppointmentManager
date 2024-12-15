using HealthcareAppointmentManager.Models.Data;

namespace HealthcareAppointmentManager.Models.Repositories
{
    public interface IPatient
    {
        public List<Patient> GetAllPatients();
        public void PatientRegister(Patient patient);
        public Patient GetPatientById(int id);

        public Patient GetPatientByUsername(string username);
        public Patient ValidatePatient(string username, string password);

        public List<Appointment> GetAppointmentsByPatientId(int id);

        public List<Doctor> GetAllDoctors();
    }
}
