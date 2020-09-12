using System;
using System.Collections.Generic;

namespace SyaApi.Entities
{
    public class WorkEntity
    {
        public int teacher_id{get;set;}
        public int work_id{get; set;}
        public string work_name{get; set;}
        public string cover{get; set;}
        public string work_description{get; set;}
        public string address{get; set;}
        public int salary{get; set;}
        public string work_time{get; set;}
        public int likes_num{get; set;}
        public int collect_num{get; set;}
        public int share_num{get; set;}
        public string start_day{get; set;}
        public string end_day{get; set;}
        public string start_time{get; set;}
        public string end_time{get; set;}
        public double total_time{get; set;}
        public int week_day{get; set;}
    }


    public class WorkItemEntity
    {
        public int total{get; set;}
        public List<WorkEntity> workItem{get;set;}

    }

    public class WorkTimeEntity
    {
        public string start_day{get; set;}
        public string end_day{get; set;}
        public string start_time{get; set;}
        public string end_time{get; set;}
        public int week_day{get; set;}
    }
}