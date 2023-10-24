using System;
using System.ComponentModel.DataAnnotations;

namespace RefactorThis.Models
{
    public class Login
    {
        [Key]
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime? LastLoggedIn { get; set; }
        public Guid APIToken { get; set; }
        public string APITokenExpiry { get; set; }
    }
}
