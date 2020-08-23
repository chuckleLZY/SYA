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

        [Required]
        public string city{ get; set;}

        [Required]
        public string education{get; set;}

        [Required]
        public string community{get; set;}

        [Required]
        public string project {get; set;}

        [Required]
        public string academic{get; set;}

        [Required]
        public string skill{get; set;}

        [Required]
        public string introduction{get; set;}
    }

}