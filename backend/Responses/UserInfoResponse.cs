namespace SyaApi.Responses
{
    public class UserInfoResponse
    {
        public int user_id { get; set; }
        public int user_role { get; set; }
        public string user_name { get; set; }
        public bool gender { get; set; }
        public string avatar { get; set; }
        public string email { get; set; }
        public string tel { get; set; }
        public string bank { get; set; }

        /// 申请总次数
        public int nof_apply { get; set; }

        /// 请假总次数
        public int nof_absent { get; set; }

        /// 工作总时长
        public decimal work_time { get; set; }

        /// 已获得薪水总量
        public decimal income { get; set; }
    }
}
