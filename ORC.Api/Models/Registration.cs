namespace ORC.Api.Models
{
    public class Registration
    {
        public int Id { get; set; }
        
        // Team Information
        public string TeamName { get; set; } = string.Empty;
        public string Institution { get; set; } = string.Empty;
        public string LeaderName { get; set; } = string.Empty;
        public string LeaderEmail { get; set; } = string.Empty;
        public string LeaderPhone { get; set; } = string.Empty;
        public int TeamSize { get; set; }
        public string TeamMembers { get; set; } = string.Empty;

        // Background & Experience
        public string HowDidYouKnow { get; set; } = string.Empty;
        public string? OtherSource { get; set; }
        public string RoboticsExperience { get; set; } = string.Empty;
        public string TechnicalSkills { get; set; } = string.Empty;

        // Technical Preparation
        public bool HasPriorExperience { get; set; }
        public string? PriorExperienceDetails { get; set; }
        public string RelevantProjects { get; set; } = string.Empty;
        public string AnticipatedChallenges { get; set; } = string.Empty;

        // Agreements
        public bool AgreeToRules { get; set; }
        public bool AgreeToMedia { get; set; }
        public bool VerifyInformation { get; set; }

        // Metadata
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    }
}