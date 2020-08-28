using System.ComponentModel.DataAnnotations;

namespace SyaApi.Requests
{
    public class AnnounceRequest
    {
        [Required]
        public string title{get; set;}
        
        [Required]
        public string content{get; set;}
    
    }

        public class ViewAnnounceRequest
    {
        [Required]
        public int pagenum{get; set;}

        [Required]
        public int pagesize{get; set;}

        
        public string query{get; set;}
    }

        public class AnnounceStatusRequest
    {
        [Required]
        public int announcement_id {get; set;}
    }
}
