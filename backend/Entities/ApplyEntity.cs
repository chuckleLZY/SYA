using System.Collections.Generic;

namespace SyaApi.Entities
{
    public class ApplyEntity
    {
        public int apply_id {get; set;}

        public int student_id {get; set;}

        public int teacher_id {get; set;}

        public int work_id {get; set;}

        public int resume_id {get; set;}

        public int status {get; set;}

    }

    public class ApplyItemEntity
    {
        public int total {get; set;}
        public List<ApplyEntity> ApplyItem {get;set;}

    }
}