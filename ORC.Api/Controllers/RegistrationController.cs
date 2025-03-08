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
        private readonly string _adminEmail = "open.robot.combat@gmail.com";

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

            // Send confirmation email to user
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

            // Send notification email to admin
            string adminEmailContent = System.IO.File.ReadAllText("wwwroot/EmailTemplates/Registration/AdminNotificationEmail.html");

            // Replace content placeholders for admin email
            adminEmailContent = adminEmailContent
                .Replace("[TeamName]", registration.TeamName)
                .Replace("[Institution]", registration.Institution)
                .Replace("[RegistrationDate]", registration.RegistrationDate.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("[TeamSize]", registration.TeamSize.ToString())
                .Replace("[LeaderName]", registration.LeaderName)
                .Replace("[LeaderEmail]", registration.LeaderEmail)
                .Replace("[LeaderPhone]", registration.LeaderPhone)
                .Replace("[TeamMembers]", registration.TeamMembers)
                .Replace("[HowDidYouKnow]", registration.HowDidYouKnow)
                .Replace("[OtherSource]", registration.OtherSource ?? "N/A")
                .Replace("[RoboticsExperience]", registration.RoboticsExperience)
                .Replace("[TechnicalSkills]", registration.TechnicalSkills)
                .Replace("[HasPriorExperience]", registration.HasPriorExperience ? "Yes" : "No")
                .Replace("[PriorExperienceDetails]", registration.PriorExperienceDetails ?? "N/A")
                .Replace("[RelevantProjects]", registration.RelevantProjects)
                .Replace("[AnticipatedChallenges]", registration.AnticipatedChallenges);

            // Replace base template placeholders for admin email
            string adminEmailBody = baseTemplate.Replace("[Page_Title]", "ORC Battle - New Registration")
                                        .Replace("[Email_Content]", adminEmailContent)
                                        .Replace("[ORC_LOGO_URL]", "https://openrobotcombat.com/logo.png");

            await _emailService.SendEmailAsync(
                _adminEmail,
                $"New ORC Battle Registration: {registration.TeamName}",
                adminEmailBody,
                "ORC Admin"
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