using System.Collections.Generic;

namespace SyaApi.Entities
{

    public class FavoriteEntity
    {
        public int favorite_id { get; set; }
        public int user_id { get; set; }
        public string favorite_name { get; set; }
        public int work_num { get; set; }


    }
        public class FavoriteItemEntity
    {
        public int total{get; set;}
        public List<FavoriteEntity> FavoriteItem{get;set;}

    }

    public class HasFavoriteEntity
    {
        public int favorite_id { get; set; }

        public int work_id { get; set; }

        public string add_name { get; set; }
    }




}