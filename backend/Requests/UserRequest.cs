using System.ComponentModel.DataAnnotations;

namespace SyaApi.Requests
{
    public class UserRequest
    {
        public bool gender { get; set; }
        public string avatar { get; set; }
        public string tel { get; set; }
        public string bank { get; set; }
    }
}
