using System;
using System.Collections.Generic;
namespace SyaApi.Entities
{
    public class MessageEntity
    {
        public int message_id{get; set;}

        public int message_type{get; set;}

        public string content {get; set;}

        public string message_time {get; set;}

        public int sender_id {get; set;}

        public int receiver_id {get; set;}

        public int status {get; set;}

        public string sender_name{get; set;}

        public string receiver_name {get; set;}
    }

    public class MessageItemEntity
    {
        public int total {get; set;}

        public List<MessageEntity> messageItem {get; set;}
    }
}