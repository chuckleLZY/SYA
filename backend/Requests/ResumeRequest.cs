using System.ComponentModel.DataAnnotations;

namespace SyaApi.Requests
{
    public class ResumeRequest
    {

        [Required]
        public int age {get; set;}

        [Required]
        [StringLength(Constants.Limit.UsernameLength)]
        public string student_name {get; set;}

        public string city{ get; set;}

        public string education{get; set;}

        public string community{get; set;}

        public string project {get; set;}

        public string academic{get; set;}

        public string skill{get; set;}

        public string introduction{get; set;}
    }

}