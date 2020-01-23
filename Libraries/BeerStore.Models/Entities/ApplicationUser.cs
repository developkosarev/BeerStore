using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BeerStore.Models.Entities
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(70)]        
        public string FirstName { get; set; }

        [MaxLength(70)]
        public string LastName { get; set; }

        public string RefreshTokenHash { get; set; }
        
        public int? UserMetaId { get; set; }

        public UserMeta UserMeta { get; set; }
    }
}
