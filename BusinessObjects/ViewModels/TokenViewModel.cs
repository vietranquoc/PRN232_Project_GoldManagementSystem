namespace BusinessObjects.DTOs
{
    public class TokenViewModel
    {
        public string Code { get; set; } = null!; // JWT token
        public DateTime Expiration { get; set; }
        public double LifeTime { get; set; } // seconds
        public UserProfileViewModel UserProfile { get; set; } = null!;
    }

    public class UserProfileViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
} 