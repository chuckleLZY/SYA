using System;
using System.Collections.Generic;
namespace SyaApi.Responses
{
    public class WorkResponse
    {
        public string cover{get; set;}
        public string work_description{get; set;}
        public string address{get; set;}
        public int salary{get; set;}
        public string work_time{get; set;}
        public int likes_num{get; set;}
        public int collect_num{get; set;}
        public int share_num{get; set;}
    }

    public class WorkItemResponse
    {
        public int total{get; set;}
        public List<WorkResponse> worklist{get;set;}

    }


}