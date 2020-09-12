using System.Threading.Tasks;
using System.Text;
using SyaApi.Plugins;
using SyaApi.Entities;

namespace SyaApi.DataAccessors
{
    public static class LikeAccessor
    {
        public static async Task<int> Create(int id,int work_id)
        {
            var query = "INSERT INTO take_like(student_id,work_id) VALUES(@student_id,@work_id)";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@student_id", id);
            command.Parameters.AddWithValue("@work_id", work_id);

            await command.ExecuteNonQueryAsync();
            return (int)command.LastInsertedId;
        }

        public static async Task<int> Find(int id,int work_id)
        {
            var query = "SELECT student_id,work_id,status FROM take_like WHERE student_id=@id AND work_id=@work_id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id",id);
            command.Parameters.AddWithValue("@work_id",work_id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return reader.GetInt32("status");
            }
            return -1;
        }

        public static async Task<int> Change(int id,int work_id,int s)
        {
            var query = "UPDATE take_like SET status=@status WHERE student_id=@id AND work_id=@work_id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            if(s==0)
                command.Parameters.AddWithValue("@status",1);
            else 
                command.Parameters.AddWithValue("@status",0);
            command.Parameters.AddWithValue("@id",id);
            command.Parameters.AddWithValue("@work_id",work_id);

            var row = await command.ExecuteNonQueryAsync();
            if(row>0)
            {
                return 1-s;
            }
            return -1;
        }
    }
}