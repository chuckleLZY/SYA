using System;
using System.Collections.Generic;
namespace SyaApi.Entities
{
    public class TakesEntity
    {
        public int student_id{get; set; }
        public int work_id{get; set;}
        public double work_time{get; set;}
        public int absent_num{get; set;}
        public double absent_time{get; set;}
    }
    public class TakesItemEntity
    {
        public int total{get; set;}
        public List<TakesEntity> TakesItem{get; set;}
    }
}