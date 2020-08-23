using System.ComponentModel.DataAnnotations;

namespace SyaApi.Requests
{
    public class ProvideWorkRequest
    {
        public string work_name{get; set;}
        public string cover{get; set;}
        public string work_description{get; set;}
        public string address{get; set;}
        public int salary{get; set;}
        public string work_time{get; set;}
    }
}