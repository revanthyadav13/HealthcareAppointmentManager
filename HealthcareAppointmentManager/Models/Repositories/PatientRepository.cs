using HealthcareAppointmentManager.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HealthcareAppointmentManager.Models.Repositories
{
    public class PatientRepository : IPatient
    {
        private readonly AppDbContext appDbContext;
        private readonly IConfiguration configuration;
        private readonly ILogger<PatientRepository> _logger;

        public PatientRepository(AppDbContext appDbContext, IConfiguration configuration, ILogger<PatientRepository> logger)
        {
            this.appDbContext = appDbContext;
            this.configuration = configuration;
            this._logger = logger;
        }

        public List<Patient> GetAllPatients()
        {
            List<Patient> patients = new List<Patient>();
            var getData = appDbContext.Patients.FromSqlRaw("GetAllPatientData");

            foreach (var data in getData)
            {
                patients.Add(new Patient()
                {
                    PatientName = data.PatientName,
                    Username = data.Username,
                    Password = data.Password,
                    ConfirmPassword = data.ConfirmPassword,
                    Gender = data.Gender,
                    DateOfBirth = data.DateOfBirth
                });
            }
            return patients;
        }

        public void PatientRegister(Patient patient)
        {
            appDbContext.Database.ExecuteSqlRaw(
                "RegisterPatientData @patient_name={0}, @username={1}, @password={2}, @confirm_password={3}, @gender={4}, @date_of_birth={5}",
                patient.PatientName, patient.Username, patient.Password, patient.ConfirmPassword, patient.Gender, patient.DateOfBirth
            );
        }

        public Patient GetPatientById(int id)
        {
            Patient patient = new Patient();
            var getdata = appDbContext.Patients.FromSqlRaw("GetPatientDataById @patient_id={0}", id).ToList().FirstOrDefault();
            if (getdata != null)
            {
                patient.PatientID = getdata.PatientID;
                patient.PatientName = getdata.PatientName;
                patient.Username = getdata.Username;
                patient.Password = getdata.Password;
                patient.ConfirmPassword = getdata.ConfirmPassword;
                patient.Gender = getdata.Gender;
                patient.DateOfBirth = getdata.DateOfBirth;

            };

            return patient;
        }

        public Patient ValidatePatient(string username, string password)
        {
            _logger.LogInformation("Attempting to validate patient with username: {Username}", username);

            var patient = appDbContext.Patients
                .FromSqlRaw("ValidatePatientLogin @username={0}, @password={1}", username, password)
                .AsEnumerable()  // Materializes the results in memory
                .SingleOrDefault();

            if (patient != null)
            {
                _logger.LogInformation("Patient validated successfully with username: {Username}", username);
            }
            else
            {
                _logger.LogWarning("Failed to validate patient with username: {Username}", username);
            }

            return patient;
        }

        public List<Appointment> GetAppointmentsByPatientId(int id)
        {

            List<Appointment> appointments = new List<Appointment>();

            appointments = appDbContext.Appointments
                .FromSqlRaw("GetAppointmentsByPatientId @PatientID={0}", id)
                .ToList();

            return appointments;
        }

        public Patient GetPatientByUsername(string username)
        {
            Patient patient = new Patient();
            var getdata = appDbContext.Patients.FromSqlRaw("GetPatientIdByUsername @username={0}", username).ToList().FirstOrDefault();
            if (getdata != null)
            {
                patient.PatientID = getdata.PatientID;
            }
            return patient;
        }

        public List<Doctor> GetAllDoctors()
        {
            List<Doctor> doctors = new List<Doctor>();
            var getdata = appDbContext.Doctors.FromSqlRaw("GetAllDoctors");
            foreach (var doctor in getdata)
            {
                doctors.Add(new Doctor()
                {
                    DoctorID = doctor.DoctorID,
                    DoctorName = doctor.DoctorName,
                    Username = doctor.Username,
                    Password = doctor.Password,
                    ConfirmPassword = doctor.ConfirmPassword,
                    Gender = doctor.Gender,
                    DoctorSpecialization = doctor.DoctorSpecialization,
                    DoctorYearsOfExperience = doctor.DoctorYearsOfExperience
                });
            }
            return doctors;
        }
    }
}

