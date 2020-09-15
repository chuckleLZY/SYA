using System.Threading.Tasks;
using System.Collections.Generic;
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
            var query = "UPDATE favorite SET favorite_name=@favorite_name where favorite_id=@favorite_id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@favorite_id", favorite.favorite_id);
            command.Parameters.AddWithValue("@favorite_name", favorite.favorite_name);
           

            var row = await command.ExecuteNonQueryAsync();
            if(row>0)
            {
                return favorite.favorite_id;
            }
            return 0;
        }
        


        public static async Task<int> Create(FavoriteEntity favorite)
        {
            var query = "INSERT INTO favorite(user_id,favorite_name) VALUES(@user_id,@favorite_name)";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@user_id", favorite.user_id);
            command.Parameters.AddWithValue("@favorite_name", favorite.favorite_name);
           

            await command.ExecuteNonQueryAsync();
            return (int)command.LastInsertedId;
        }

        public static async Task<int> Delete(int favorite_id)
        {
            var query = "DELETE FROM favorite WHERE favorite_id=@favorite_id";

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


        public static async Task<FavoriteItemEntity> ViewFavorite(int user_id)
        {
            FavoriteItemEntity favorite=new FavoriteItemEntity();
            favorite.total=0;
            favorite.FavoriteItem=new List<FavoriteEntity>();
            var query = "SELECT favorite_id,user_id,favorite_name,work_num FROM favorite WHERE user_id=@user_id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@user_id",user_id);
            using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                FavoriteEntity temp=new FavoriteEntity();
                
                temp.favorite_id=reader.GetInt32("favorite_id");
                temp.user_id=reader.GetInt32("user_id");
                temp.favorite_name=reader.GetString("favorite_name");
                temp.work_num=reader.GetInt32("work_num");
                
                favorite.total++;
                favorite.FavoriteItem.Add(temp);
            }
            return favorite;
        }


        public static async Task<int> AddFavorWork(HasFavoriteEntity entity)
        {
            var query = "INSERT INTO favorite_has_work(favorite_id,work_id) VALUES(@favorite_id,@work_id)";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@work_id", entity.work_id);
            command.Parameters.AddWithValue("@favorite_id", entity.favorite_id);
           

            await command.ExecuteNonQueryAsync();
            return (int)command.LastInsertedId;
        }

        public static async Task<int> FindFavorWork(HasFavoriteEntity entity)
        {
            var query = "SELECT favorite_id,work_id FROM favorite_has_work WHERE favorite_id=@favorite_id AND work_id=@work_id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@favorite_id",entity.favorite_id);
            command.Parameters.AddWithValue("@work_id",entity.work_id);
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                HasFavoriteEntity temp=new HasFavoriteEntity
                {
                    favorite_id=reader.GetInt32("favorite_id"),
                    work_id=reader.GetInt32("work_id"),              
                };
                return 1;
            }
            return 0;
            
        }
        public static async Task<int> ChangeNum( int id)
        {
            var query = "UPDATE favorite SET work_num=(SELECT Count(work_id) From favorite_has_work WHERE favorite_id=@id) WHERE favorite_id=@f_id;";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@f_id", id);
           

            var row = await command.ExecuteNonQueryAsync();
            if(row>0)
            {
                return id;
            }
            return 0;
        }

        public static async Task<WorkItemEntity> ViewInfo(int id)
        {
            WorkItemEntity work=new WorkItemEntity();
            work.total=0;
            work.workItem=new List<WorkEntity>();

            var query = "SELECT teacher_id,work_id,work_name,cover,work_description,address,salary,likes_num,collect_num,share_num FROM work WHERE work_id=ANY (SELECT work_id FROM favorite_has_work WHERE favorite_id=@id);";
            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id",id);
            using var reader = await command.ExecuteReaderAsync();
            while ( reader.Read())
            {
                WorkEntity temp=new WorkEntity();
                
                temp.teacher_id=reader.GetInt32("teacher_id");
                temp.work_id=reader.GetInt32("work_id");
                temp.work_name=reader.GetString("work_name");
                temp.cover=reader.GetString("cover");
                temp.work_description=reader.GetString("work_description");
                temp.address=reader.GetString("address");
                //temp.work_time=reader.GetString("work_time");
                temp.salary=reader.GetInt32("salary");
                temp.likes_num=reader.GetInt32("likes_num");
                temp.collect_num=reader.GetInt32("collect_num");
                temp.share_num=reader.GetInt32("share_num");
                
                work.total++;
                work.workItem.Add(temp);
            }
            return work;
        }

        public static async Task<int> DeleteFavorWork(HasFavoriteEntity entity)
        {
            var query = "DELETE FROM favorite_has_work WHERE favorite_id=@favorite_id AND work_id=@work_id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@work_id", entity.work_id);
            command.Parameters.AddWithValue("@favorite_id", entity.favorite_id);
           
            var row = await command.ExecuteNonQueryAsync();
            if(row>0)
            {
                return 1;
            }
            return 0;
        }

    }

}