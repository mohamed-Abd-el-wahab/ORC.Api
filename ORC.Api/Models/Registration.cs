namespace ORC.Api.Models

{
    public class Registration
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Institution { get; set; } = string.Empty;
        public string TeamName { get; set; } = string.Empty;
        public string HowDidYouKnow { get; set; } = string.Empty;
        public string? OtherSource { get; set; }
        public string Experience { get; set; } = string.Empty;
        public string Goals { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    }
}