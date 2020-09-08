using System.Collections.Generic;

namespace SyaApi.Responses
{
    public class LeaveResponse
    {
        public int leave_id{get; set;}
        
        public string student_name{get; set;}

        public string work_name{get; set;}

        public int student_id{get; set;}

        public int work_id{get; set;}
        
        public string content{get; set;}

        public string leave_time{get; set;}

        public string proof{get; set;}

        public int status {get; set;}
        
        public decimal leave_duration{get; set;}

        public System.DateTime request_time{get; set;}
    }

    public class LeaveItemResponse
    {
        public int total{get; set;}
        public List<LeaveResponse> leavelist{get;set;}

    }
}