using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendTestWebAPI.Models
{
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; } //hash from password against username
        public string Password2 { get; set; } //hash from password against email
        public string Password3 { get; set; } //has from password against display name
        public string Email { get; set; }
        public string VerificationCode { get; set; }
    }
}
