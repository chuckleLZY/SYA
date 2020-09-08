using System;
using System.Collections.Generic;
namespace SyaApi.Entities
{
     public class AnnounceEntity
    {
        public int announcement_id { get; set; }

        public int user_id { get; set; }

        public string title{get; set;}
        
        public string content { get; set; }

        public int status { get; set; }

        public string send_time {get; set;}

    }
    public class AnnounceItemEntity
    {
        public int totalpage{get; set;}

        public List<AnnounceEntity> AnnounceItem {get; set;}
    }

}
   
