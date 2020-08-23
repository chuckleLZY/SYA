using System;

namespace SyaApi.Entities
{
    public class UserEntity
    {
        public int user_id { get; set; }
        public int user_role { get; set; }
        public string user_name { get; set; }
        public bool gender { get; set; }
        public string avatar {get; set; } // 头像url
        public string email { get; set; }
        public string tel { get; set; }
        public string bank { get; set; }
    }
}
