using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ORC.Api.Data;
using ORC.Api.Models;

namespace ORC.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly OrcDbContext _context;
        private readonly IEmailService _emailService;

        public RegistrationController(OrcDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<ActionResult<Registration>> Register(Registration registration)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            // Send confirmation email
            string baseTemplate = System.IO.File.ReadAllText("wwwroot/EmailTemplates/Shared/BaseTemplate.html");
            string emailContent = System.IO.File.ReadAllText("wwwroot/EmailTemplates/Registration/RegistrationEmail.html");

            // Replace content placeholders
            emailContent = emailContent.Replace("[User's Name]", registration.LeaderName)
                                    .Replace("[User's Email]", registration.LeaderEmail)
                                    .Replace("[Summary]", $"Team {registration.TeamName} from {registration.Institution}");

            // Replace base template placeholders
            string emailBody = baseTemplate.Replace("[Page_Title]", "ORC Battle - Registration Confirmation")
                                        .Replace("[Email_Content]", emailContent)
                                        .Replace("[ORC_LOGO_URL]", "https://openrobotcombat.com/logo.png");

            await _emailService.SendEmailAsync(
                registration.LeaderEmail,
                "Thank You for Registering for ORC Battle!",
                emailBody,
                registration.LeaderName
            );

            return CreatedAtAction(nameof(GetRegistration), new { id = registration.Id }, registration);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Registration>> GetRegistration(int id)
        {
            var registration = await _context.Registrations.FindAsync(id);

            if (registration == null)
                return NotFound();

            return registration;
        }
    }
} 