using System.Collections.Generic;

namespace SyaApi.Entities
{
    public class LeaveEntity
    {
        public int leave_id{get; set;}

        public int student_id{get; set;}

        public int work_id{get; set;}
        
        public string student_name{get; set;}

        public string work_name{get; set;}

        public string content{get; set;}

        public string leave_time{get; set;} //请假的开始时间

        public string proof{get; set;}

        public int status{get; set;}

        //request_time 申请时间

        public decimal leave_duration{get; set;} //请假的持续时长
    }
    
    public class LeaveItemEntity
    {
        public int total{get; set;}
        public List<LeaveEntity> leaveItem{get;set;}

    }
}