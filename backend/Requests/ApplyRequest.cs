using System.ComponentModel.DataAnnotations;

namespace SyaApi.Requests
{
    public class ApplyRequest
    {
        //public 
    }

    public class ManageApplyRequest
    {
        [Required]
        public int apply_id {get; set;}

        [Required]
        public int status {get; set;} // Constants: ApplyStatus
    }
}