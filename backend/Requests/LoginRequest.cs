using System.ComponentModel.DataAnnotations;

namespace SyaApi.Requests
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }  // Email is also accepted here

        [Required]
        public string Password { get; set; }
    }
}
