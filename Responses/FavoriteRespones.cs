using System;
using System.Collections.Generic;

namespace SyaApi.Responses
{
    public class FavoriteResponse
    {
       
        public int favorite_id { get; set; }
        public int user_id { get; set; }
        public string favorite_name { get; set; }
        public int work_num { get; set; }

    }

     public class FavoriteItemResponse
    {
        
        public List<FavoriteResponse> favoritelist{get;set;}

    }
    
}