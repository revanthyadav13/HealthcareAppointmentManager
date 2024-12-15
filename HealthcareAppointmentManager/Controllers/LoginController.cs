using Microsoft.AspNetCore.Mvc;
using HealthcareAppointmentManager.Models.Repositories;
using HealthcareAppointmentManager.Models.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;

namespace HealthcareAppointmentManager.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IDoctor _doctorRepository;
        private readonly IPatient _patientRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IDoctor doctorRepository, IPatient patientRepository, IConfiguration configuration, ILogger<LoginController> logger)
        {
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            _logger.LogInformation("Login attempt for user: {Username}", login.Username);

            // Check Doctor table using the stored procedure

            var doctor = _doctorRepository.ValidateDoctor(login.Username, login.Password);
            if (doctor != null)
            {
                _logger.LogInformation("Doctor {Username} authenticated successfully", login.Username);
                return GenerateToken(doctor.Username, UserRole.Doctor.ToString());
            }

            // Check Patient table using the stored procedure
            var patient = _patientRepository.ValidatePatient(login.Username, login.Password);
            if (patient != null)
            {
                _logger.LogInformation("Patient {Username} authenticated successfully", login.Username);
                return GenerateToken(patient.Username, UserRole.Patient.ToString());
            }

            _logger.LogWarning("Authentication failed for user: {Username}", login.Username);
            return Unauthorized();
        }

        private IActionResult GenerateToken(string username, string role)
        {
            try
            {
                _logger.LogInformation("Generating token for user: {Username} with role: {Role}", username, role);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.Role, role) // Include role claim
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Issuer"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                _logger.LogInformation("Token generated successfully for user: {Username}", username);
                return Ok(new { Token = tokenString });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating token for user: {Username}", username);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
