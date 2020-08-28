
using System.ComponentModel.DataAnnotations;

namespace SyaApi.Requests
{
    public class FindworkRequest
    {
        [Required]
        public int work_id{get;set;}
    }

    public class ProvideWorkRequest
    {
        [Required]
        public string work_name{get; set;}

        [Required]
        public string cover{get; set;}

        [Required]
        public string work_description{get; set;}

        [Required]
        public string address{get; set;}

        [Required]
        public int salary{get; set;}

        [Required]
        public string work_time{get; set;}
    }

    public class ViewWorkRequest
    {
        [Required]
        public int pagenum{get; set;}

        [Required]
        public int pagesize{get; set;}

        
        public string query{get; set;}
    }

    public class UpdateWorkRequest
    {
        [Required]
        public int work_id{get;set;}
        [Required]
        public string work_name{get; set;}

        [Required]
        public string cover{get; set;}

        [Required]
        public string work_description{get; set;}

        [Required]
        public string address{get; set;}

        [Required]
        public int salary{get; set;}

        [Required]
        public string work_time{get; set;}
    }


}