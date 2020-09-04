using System.ComponentModel.DataAnnotations;

namespace SyaApi.Requests
{
    public class FhasworkRequest
    {
        public int favorite_id {get; set;}
        public int work_id {get; set;}
       
    }

    public class ViewRequest
    {
         public int favorite_id {get; set;}
        public string query{get; set;}
    
    }

  
}