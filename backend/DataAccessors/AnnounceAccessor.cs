using System.Threading.Tasks;
using System.Text;
using SyaApi.Plugins;
using SyaApi.Entities;
using System.Collections.Generic;

namespace SyaApi.DataAccessors
{
    public static class AnnounceAccessor
    {
       
        public static async Task<int> Create(AnnounceEntity announcement)
        {
            var query = "INSERT INTO announcement(user_id,title,content) values(@user_id,@title,@content)";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@user_id", announcement.user_id);
            command.Parameters.AddWithValue("@title", announcement.title);
            command.Parameters.AddWithValue("@content", announcement.content);
           
            await command.ExecuteNonQueryAsync();
            return (int)command.LastInsertedId;
        }

        ///<summery>
        /// 用户接收所有公告信息
        /// chuckle 8.28
        ///</summery>
        public static async Task<AnnounceItemEntity> Find()
        {
            AnnounceItemEntity announce=new AnnounceItemEntity();
            announce.totalpage=0;
            announce.AnnounceItem=new List<AnnounceEntity>();

            var query = "SELECT announcement_id,user_id,title,content,status,send_time FROM announcement";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                announce.totalpage++;
                AnnounceEntity temp=new AnnounceEntity();
                temp.announcement_id=reader.GetInt32("announcement_id");
                temp.user_id=reader.GetInt32("user_id");
                temp.title=reader.GetString("title");
                temp.content=reader.GetString("content");
                temp.send_time=reader.GetDateTime("send_time").ToString();
                temp.status=reader.GetInt32("status");
                announce.AnnounceItem.Add(temp);

            }
            return announce;
        }
    
        ///<summery>
        ///用户已查看公告，修改状态为1
        /// chuckle 8.28
        ///</summery>
        public static async Task<int> Change(AnnounceEntity entity)
        {
            var query = "UPDATE announcement SET status=@status WHERE announcement_id =@id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@id", entity.announcement_id);
            command.Parameters.AddWithValue("@status", 1);

            var row = await command.ExecuteNonQueryAsync();
            if(row>0)
            {
                return 1;
            }
            return 0;
        }
        


        ///<summery>
        ///用户删除消息，修改状态为2
        /// chuckle 8.28
        ///</summery>
        public static async Task<int> Delete(AnnounceEntity entity)
        {
            var query = "UPDATE announcement SET status=@status WHERE announcement_id =@id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@id", entity.announcement_id);
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