using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using SyaApi.Plugins;
using SyaApi.Entities;

namespace SyaApi.DataAccessors
{
    public static class FhasworkAccessor
    {
        public static async Task<int> Create(FhasworkEntity fhaswork)
        {
            var query = "INSERT INTO favorite_has_work(favorite_id,work_id) values(@favorite_id,@work_id)";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@favorite_id", fhaswork.favorite_id);
            command.Parameters.AddWithValue("@work_id", fhaswork.work_id);
            
            await command.ExecuteNonQueryAsync();
            return (int)command.LastInsertedId;
        }
        public static async Task<FhasworkEntity> Find(int favorite_id,int work_id)
        {
            var query = "SELECT favorite_id,work_id,add_time FROM favorite_has_work WHERE favorite_id=@favorite_id AND work_id=@work_id" ;

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@favorite_id",favorite_id);
            command.Parameters.AddWithValue("@work_id",work_id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new FhasworkEntity
                {
                    favorite_id=reader.GetInt32("favorite_id"),
                    work_id=reader.GetInt32("work_id"),
                    add_time=reader.GetDateTime("add_time"),
                              
                };
            }
            return null;
        }


        public static async Task<FhasworkItemEntity> ViewFhaswork(int favorite_id)
        {
            FhasworkItemEntity fhaswork=new FhasworkItemEntity();
            fhaswork.total=0;
            fhaswork.FhasworkItem=new List<FhasworkEntity>();
            var query = "SELECT favorite_id,work_id,add_time FROM favorite_has_work WHERE favorite_id=@favorite_id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@favorite_id",favorite_id);
            using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                FhasworkEntity temp=new FhasworkEntity();
                
                temp.favorite_id=reader.GetInt32("favorite_id");
                temp.work_id=reader.GetInt32("work_id");
                temp.add_time=reader.GetDateTime("add_time");
                
                
                fhaswork.total++;
                fhaswork.FhasworkItem.Add(temp);
            }
            return fhaswork;
        }

       
    }

}