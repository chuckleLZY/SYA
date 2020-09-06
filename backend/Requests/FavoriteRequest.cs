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

    public class ViewFavoriteRequest
    {
        [Required]
        public int pagenum{get; set;}

        [Required]
        public int pagesize{get; set;}

        
        public string query{get; set;}
    }

    public class HasFavoriteRequest
    {
        [Required]
        public int favorite_id {get; set;}

        [Required]
        public int work_id{get; set;}
    }

        public class InfoFavoriteRequest
    {      
        [Required]
        public int favorite_id {get; set;}


    
    }
}
