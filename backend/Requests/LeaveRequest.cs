using System.ComponentModel.DataAnnotations;

namespace SyaApi.Requests
{
    public class LeaveRequest
    {
        [Required]
        public int work_id{get; set;}
        [Required]
        public string content{get; set;}
        [Required]
        public string leave_time{get; set;}
        [Required]
        public string proof{get; set;}
        [Required]
        public int status{get; set;}
    }

    public class ManageLeaveRequest
    {
        [Required]
        public int leave_id {get; set;}

        [Required]
        public int status {get; set;} // Constants: ApplyStatus
    }
}