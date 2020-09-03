using System.ComponentModel.DataAnnotations;

namespace SyaApi.Requests
{
    public class FavoriteRequest
    {
        public int favorite_id {get; set;}
        public string favorite_name{get; set;}
        public int work_num{get; set;}
    
    }
     public class CFavoriteRequest
    {
        public string favorite_name{get; set;}
        public int work_num{get; set;}
    
    }
}
