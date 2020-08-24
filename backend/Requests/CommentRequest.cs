using System.ComponentModel.DataAnnotations;

namespace SyaApi.Requests
{
    public class CommentRequest
    {      
        public int work_id{get;set;}
       // public int sender_id{get;set;}
        public string content{get; set;}
        
    
    }
}
