using System.Threading.Tasks;
using System.Text;
using SyaApi.Plugins;
using SyaApi.Entities;

namespace SyaApi.DataAccessors
{
    public static class FavoriteAccessor
    {
        public static async Task<FavoriteEntity> Find(int favorite_id)
        {
            var query = "SELECT favorite_id,user_id,favorite_name,work_num FROM favorite WHERE favorite_id=@favorite_id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@favorite_id",favorite_id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new FavoriteEntity
                {
                    favorite_id=reader.GetInt32("favorite_id"),
                    user_id=reader.GetInt32("user_id"),
                    favorite_name=reader.GetString("favorite_name"),
                    work_num=reader.GetInt32("work_num")               
                };
            }
            return null;
        }

        public static async Task<int> Change(FavoriteEntity favorite)
        {
            var query = "UPDATE favorite SET favorite_name=@favorite_name,work_num=@work_num where favorite_id=@favorite_id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@favorite_id", favorite.favorite_id);
            command.Parameters.AddWithValue("@favorite_name", favorite.favorite_name);
            command.Parameters.AddWithValue("@work_num", favorite.work_num);
           

            var row = await command.ExecuteNonQueryAsync();
            if(row>0)
            {
                return favorite.favorite_id;
            }
            return 0;
        }
        


        public static async Task<int> Create(FavoriteEntity favorite)
        {
            var query = "INSERT INTO favorite(favorite_id,user_id,favorite_name,work_num) VALUES(@favorite_id,@user_id,@favorite_name,@work_num)";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@favorite_id", favorite.favorite_id);
            command.Parameters.AddWithValue("@user_id", favorite.user_id);
            command.Parameters.AddWithValue("@favorite_name", favorite.favorite_name);
            command.Parameters.AddWithValue("@work_num", favorite.work_num);
           

            await command.ExecuteNonQueryAsync();
            return (int)command.LastInsertedId;
        }

        public static async Task<int> Delete(int favorite_id)
        {
            var query = "DELETE FROM favorite where favorite_id=@favorite_id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@favorite_id", favorite_id);
            var row = await command.ExecuteNonQueryAsync();
            if(row>0)
            {
                return favorite_id;
            }
            return 0;
        }
    }

}