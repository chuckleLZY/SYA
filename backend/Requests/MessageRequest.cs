
using System.ComponentModel.DataAnnotations;

namespace SyaApi.Requests
{
    public class MessageRequest
    {
        [Required]
        public int message_type{get; set;}

        [Required]
        public string content {get; set;}

        [Required]
        public int receiver_id {get; set;}
    }

    public class MessageStatusRequest
    {
        [Required]
        public int message_id {get; set;}
    }
        public class ViewMessageRequest
    {
        [Required]
        public int pagenum{get; set;}

        [Required]
        public int pagesize{get; set;}

        
        public string query{get; set;}
    }
}