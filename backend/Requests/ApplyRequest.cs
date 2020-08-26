using System.ComponentModel.DataAnnotations;

namespace SyaApi.Requests
{
    public class ApplyRequest
    {
        [Required]
        public int work_id {get; set;}
        
    }

    public class ManageApplyRequest
    {
        [Required]
        public int apply_id {get; set;}

        [Required]
        public int status {get; set;} // Constants: ApplyStatus
    }
}