using System;

namespace SyaApi.Entities
{
    public class LeaveEntity
    {
        public int leave_id{get; set;}

        public int student_id{get; set;}

        public int work_id{get; set;}

        public string content{get; set;}

        public string leave_time{get; set;}

        public string proof{get; set;}
    }
    
}