using System;
using Microsoft.AspNetCore.Identity;

namespace JwtAuthApp.Server.Models.Identity
{
    public class AppUser : IdentityUser
    {
        public DateTime JoinDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? FacebookId { get; set; }
        public string PictureUrl { get; set; }
    }
}
