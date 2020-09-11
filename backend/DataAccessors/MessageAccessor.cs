using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using SyaApi.Plugins;
using SyaApi.Entities;

namespace SyaApi.DataAccessors
{
    public static class MessageAccessor
    {
        ///<summery>
        /// 查找用户发送的信息
        /// chuckle 8.28
        ///</summery>
        public static async Task<MessageItemEntity> FindSend(int id)
        {
            MessageItemEntity message=new MessageItemEntity();
            message.total=0;
            message.messageItem=new List<MessageEntity>();

            var query = "SELECT message_id,message_type,content,message_time,sender_id,receiver_id,status FROM message_library WHERE sender_id=@id AND m_type=@type";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id",id);
            command.Parameters.AddWithValue("@type",0);
            

            using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                message.total++;
                MessageEntity temp=new MessageEntity();
                temp.message_id=reader.GetInt32("message_id");
                temp.message_type=reader.GetInt32("message_type");
                temp.content=reader.GetString("content");
                temp.message_time=reader.GetDateTime("message_time").ToString();
                temp.sender_id=reader.GetInt32("sender_id");
                temp.receiver_id=reader.GetInt32("receiver_id");
                temp.status=reader.GetInt32("status");
                message.messageItem.Add(temp);

            }
            return message;
        }

        ///<summery>
        /// 查找用户接收的信息
        /// chuckle 8.28
        ///</summery>
        public static async Task<MessageItemEntity> FindReceive(int id,int type)
        {
            MessageItemEntity message=new MessageItemEntity();
            message.total=0;
            message.messageItem=new List<MessageEntity>();

            var query = "SELECT message_id,message_type,content,message_time,sender_id,receiver_id,status FROM message_library WHERE receiver_id=@id AND status<>@status AND m_type=@type";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id",id);
            command.Parameters.AddWithValue("@status",2);
            command.Parameters.AddWithValue("@type",type);

            using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                message.total++;
                MessageEntity temp=new MessageEntity();
                temp.message_id=reader.GetInt32("message_id");
                temp.message_type=reader.GetInt32("message_type");
                temp.content=reader.GetString("content");
                temp.message_time=reader.GetDateTime("message_time").ToString();
                temp.sender_id=reader.GetInt32("sender_id");
                temp.receiver_id=reader.GetInt32("receiver_id");
                temp.status=reader.GetInt32("status");
                message.messageItem.Add(temp);

            }
            return message;
        }
    
        ///<summery>
        ///用户已查看消息，修改状态为1
        /// chuckle 8.28
        ///</summery>
        public static async Task<int> Change(MessageEntity entity)
        {
            var query = "UPDATE message_library SET status=@status WHERE message_id =@id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@id", entity.message_id);
            command.Parameters.AddWithValue("@status", 1);

            var row = await command.ExecuteNonQueryAsync();
            if(row>0)
            {
                return 1;
            }
            return 0;
        }
        

        ///<summery>
        /// 用户发送（新建）信息
        /// chuckle 8.28
        ///</summery>
        public static async Task<int> Create(MessageEntity entity,int type)
        {
            var query = "INSERT INTO message_library(message_type,content,message_time,sender_id,receiver_id,m_type) VALUES(@message_type,@content,@message_time,@sender_id,@receiver_id,@type)";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@message_type", entity.message_type);
            command.Parameters.AddWithValue("@content", entity.content);
            command.Parameters.AddWithValue("@message_time", entity.message_time);
            command.Parameters.AddWithValue("@sender_id", entity.sender_id);
            command.Parameters.AddWithValue("@receiver_id", entity.receiver_id);
            command.Parameters.AddWithValue("@type",type);

            await command.ExecuteNonQueryAsync();
            return (int)command.LastInsertedId;
        }


        ///<summery>
        ///用户删除消息，修改简历状态为2
        /// chuckle 8.28
        ///</summery>
        public static async Task<int> Delete(MessageEntity entity)
        {
            var query = "UPDATE message_library SET status=@status WHERE message_id =@id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@id", entity.message_id);
            command.Parameters.AddWithValue("@status", 2);

            var row = await command.ExecuteNonQueryAsync();
            if(row>0)
            {
                return 2;
            }
            return 0;
        }














    }
}