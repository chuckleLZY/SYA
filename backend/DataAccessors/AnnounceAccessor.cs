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
        public static async Task<AnnounceItemEntity> Find(int id)
        {
            AnnounceItemEntity announce=new AnnounceItemEntity();
            announce.totalpage=0;
            announce.AnnounceItem=new List<AnnounceEntity>();

            var query = "SELECT announcement_id,user_id,title,announce_send.status,send_time FROM announcement JOIN announce_send USING (announcement_id) WHERE receive_id=@id ";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                announce.totalpage++;
                AnnounceEntity temp=new AnnounceEntity();
                temp.announcement_id=reader.GetInt32("announcement_id");
                temp.user_id=reader.GetInt32("user_id");
                temp.title=reader.GetString("title");
                temp.send_time=reader.GetDateTime("send_time").ToString();
                temp.status=reader.GetInt32("status");
                announce.AnnounceItem.Add(temp);

            }
            return announce;
        }


        ///<summery>
        ///管理员接收所有公告信息
        /// chuckle 8.28
        ///</summery>
        public static async Task<AnnounceItemEntity> FindSend(int id)
        {
            AnnounceItemEntity announce=new AnnounceItemEntity();
            announce.totalpage=0;
            announce.AnnounceItem=new List<AnnounceEntity>();

            var query = "SELECT announcement_id,user_id,title,content,status,send_time FROM announcement  WHERE user_id=@id ";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);

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
        /// 用户查看公告具体信息
        /// chuckle 8.28
        ///</summery>
        public static async Task<AnnounceEntity> FindInfo(int id)
        {
            AnnounceEntity announce=new AnnounceEntity();


            var query = "SELECT announcement_id,user_id,title,content,status,send_time FROM announcement WHERE announcement_id=@id ";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (reader.Read())
            {
                announce.announcement_id=reader.GetInt32("announcement_id");
                announce.user_id=reader.GetInt32("user_id");
                announce.title=reader.GetString("title");
                announce.content=reader.GetString("content");
                announce.send_time=reader.GetDateTime("send_time").ToString();
                announce.status=reader.GetInt32("status");

            }
            return announce;
        }
    
        ///<summery>
        ///用户已查看公告，修改状态为1
        /// chuckle 8.28
        ///</summery>
        public static async Task<int> Change(AnnounceEntity entity)
        {
            var query = "UPDATE announce_send SET status=@status WHERE receive_id =@id AND announcement_id=@ann_id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", entity.user_id);
            command.Parameters.AddWithValue("@ann_id", entity.announcement_id);
            command.Parameters.AddWithValue("@status", 1);

            var row = await command.ExecuteNonQueryAsync();
            if(row>0)
            {
                return 1;
            }
            return 0;
        }
        


        ///<summery>
        ///用户删除消息
        /// chuckle 8.28
        ///</summery>
        public static async Task<int> Delete(AnnounceEntity entity)
        {
            var query = "DELETE FROM announce_send WHERE receive_id =@id AND announcement_id=@ann_id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@id", entity.user_id);
            command.Parameters.AddWithValue("@ann_id", entity.announcement_id);

            var row = await command.ExecuteNonQueryAsync();
            if(row>0)
            {
                return 2;
            }
            return 0;
        }

        
        ///<summery>
        ///管理员删除消息
        /// chuckle 8.28
        ///</summery>
        public static async Task<int> DeleteAll(AnnounceEntity entity)
        {
            var query = "DELETE FROM announcement WHERE  announcement_id=@ann_id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            
            command.Parameters.AddWithValue("@ann_id", entity.announcement_id);

            var row = await command.ExecuteNonQueryAsync();
            if(row>0)
            {
                return 2;
            }
            return 0;
        }

        public static async Task<List<int>> Createsend(int ann_id)
        {
            List<int>userItem=new List<int>();

            var query = "SELECT user_id FROM user WHERE user_role<>0 ";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            using var reader = await command.ExecuteReaderAsync();

            var query_all="";
            while (reader.Read())
            {
                userItem.Add(reader.GetInt32("user_id"));
                query_all+="INSERT INTO announce_send(announcement_id,receive_id) values("+ann_id+","+reader.GetInt32("user_id")+");";
            
            }
            await Createrecord(query_all);
            

            return userItem;
        }
       
       public static async Task<int> Createrecord(string query)
       {
            // query = "INSERT INTO announce_send(announcement_id,receive_id) values(@ann_id,@receive_id);";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            // command.Parameters.AddWithValue("@ann_id", ann_id);
            // command.Parameters.AddWithValue("@receive_id", userid);
           
            await command.ExecuteNonQueryAsync();
            
            return (int)command.LastInsertedId;
           
       }
    }

}