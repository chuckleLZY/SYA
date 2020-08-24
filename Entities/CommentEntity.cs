namespace SyaApi.Entities
{
      public class CommentEntity
    {
        public int comment_id { get; set; }
        public int sender_id { get; set; }
        public int work_id { get; set; }
        //public int comment_time { get; set; }
        public string content { get; set; }
        public int likes_num { get; set; }


    }

}
  

