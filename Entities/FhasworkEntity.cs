using System.Collections.Generic;

namespace SyaApi.Entities
{

    public class FhasworkEntity
    {
        public int favorite_id { get; set; }
        public int work_id { get; set; }
        public System.DateTime add_time { get; set; }
    }

    public class FhasworkItemEntity
    {
        public int total{get; set;}
        public List<FhasworkEntity> FhasworkItem{get;set;}

    }
}