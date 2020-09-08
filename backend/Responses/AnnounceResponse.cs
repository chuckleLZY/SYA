using System.Collections.Generic;

namespace SyaApi.Responses
{
    public class AnnounceResponse
    {
        public int user_id { get; set; }  

        public string title{get; set;} 

        public int status { get; set; }

        public string send_time {get; set;}
    }

    public class AnnounceItemResponse
    {
        public int totalpage{get; set;}

        public int pagenum{get; set;}

        public List<AnnounceResponse> AnnounceItem {get; set;}
    }
    public class AnnounceInfoResponse
    {
        public string content{get; set;} 

    }



}