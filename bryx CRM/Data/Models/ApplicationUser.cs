using Microsoft.AspNetCore.Identity;

namespace bryx_CRM.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Дополнительные свойства пользователя
        public string? FullName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginAt { get; set; }
    }
}
