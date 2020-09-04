using System;
using System.Collections.Generic;

namespace SyaApi.Responses
{
    public class FhasworkResponse
    {
        public int favorite_id {get; set;}
        public int work_id {get; set;}
        public System.DateTime add_time { get; set; }


    }

    public class FhasworkItemResponse
    {
        
        public List<FhasworkResponse> fhasworklist{get;set;}

    }
    
}