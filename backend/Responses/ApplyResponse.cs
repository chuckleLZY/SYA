using System.Collections.Generic;
namespace SyaApi.Responses
{
    public class ApplyResponse
    {
        public int apply_id {get; set;}

        public string student_name {get; set;}

        public string teacher_name {get; set;}

        public string work_name {get; set;}

        public int resume_id {get; set;}

        public int status {get; set;}
    }
    
    public class ApplyItemResponse
    {
        public int total{get; set;}
        public List<ApplyResponse> applist{get;set;}

    }



}