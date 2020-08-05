using System.ComponentModel.DataAnnotations;

namespace SyaApi.Requests
{
    public class RegisterRequest
    {
        [Required]
        [StringLength(Constants.Limit.UsernameLength)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Range(Constants.Role.Student, Constants.Role.Provider)]
        public int Role { get; set; }
    }
}
