using System;

namespace SyaApi.Entities
{
    public class AccountEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public DateTime Registered { get; set; }
    }
}
