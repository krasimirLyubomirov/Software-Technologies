using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AspBlog.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }
    }
}
