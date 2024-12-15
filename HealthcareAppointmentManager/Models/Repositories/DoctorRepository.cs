using HealthcareAppointmentManager.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HealthcareAppointmentManager.Models.Repositories
{
    public class DoctorRepository : IDoctor
    {
        private readonly AppDbContext appDbContext;
        private readonly IConfiguration configuration;
        private readonly ILogger<DoctorRepository> _logger;

        public DoctorRepository(AppDbContext appDbContext, IConfiguration configuration, ILogger<DoctorRepository> logger)
        {
            this.appDbContext = appDbContext;
            this.configuration = configuration;
            this._logger = logger;
        }

        public void DoctorRegister(Doctor doctor)
        {
            appDbContext.Database.ExecuteSqlRaw(
                "RegisterDoctorData @doctor_name={0}, @username={1}, @password={2}, @confirm_password={3}, @gender={4}, @doctor_specialization={5}, @doctor_years_of_experience={6}",
                doctor.DoctorName, doctor.Username, doctor.Password, doctor.ConfirmPassword, doctor.Gender, doctor.DoctorSpecialization, doctor.DoctorYearsOfExperience
            );
        }

        

        public List<Appointment> GetAppointmentsByDoctorId(int id)
        {
            List<Appointment> appointments = new List<Appointment>();

            appointments = appDbContext.Appointments
                .FromSqlRaw("GetAppointmentsByDoctorId @DoctorID={0}", id)
                .ToList();

            return appointments;
        }

        public Doctor GetDoctorById(int id)
        {
            Doctor doctor = new Doctor();
            var getdata = appDbContext.Doctors.FromSqlRaw("GetDoctorDataById @doctor_id={0}", id).ToList().FirstOrDefault();
            if (getdata != null)
            {
                doctor.DoctorID = getdata.DoctorID;
                doctor.DoctorName = getdata.DoctorName;
                doctor.Username = getdata.Username;
                doctor.Password = getdata.Password;
                doctor.ConfirmPassword = getdata.ConfirmPassword;
                doctor.Gender = getdata.Gender;
                doctor.DoctorSpecialization = getdata.DoctorSpecialization;
                doctor.DoctorYearsOfExperience= getdata.DoctorYearsOfExperience;
            };

            return doctor;
        }

        public Doctor GetDoctorByUsername(string username)
        {
            Doctor doctor = new Doctor();
            var getdata = appDbContext.Doctors.FromSqlRaw("GetDoctorIdByUsername @username={0}", username).ToList().FirstOrDefault();
            if (getdata != null)
            {
                doctor.DoctorID = getdata.DoctorID;
            }
            return doctor;
        }

        public Doctor ValidateDoctor(string username, string password)
        {
            _logger.LogInformation("Attempting to validate doctor with username: {Username}", username);

            var doctor = appDbContext.Doctors
                .FromSqlRaw("ValidateDoctorLogin @username={0}, @password={1}", username, password)
                .AsEnumerable()  // Materializes the results in memory
                .SingleOrDefault();

            if (doctor != null)
            {
                _logger.LogInformation("Doctor validated successfully with username: {Username}", username);
            }
            else
            {
                _logger.LogWarning("Failed to validate doctor with username: {Username}", username);
            }

            return doctor;
        }

    }
}
