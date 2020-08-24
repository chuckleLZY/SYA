using System.Threading.Tasks;
using System.Text;
using SyaApi.Plugins;
using SyaApi.Entities;

namespace SyaApi.DataAccessors
{
    public static class HistoryAccessor
    {
        public static async Task<int> Create(HistoryEntity history)
        {
            var query = "INSERT INTO history_view(student_id,work_id) values(@student_id,@work_id)";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@student_id", history.student_id);
            command.Parameters.AddWithValue("@work_id", history.work_id);
            //command.Parameters.AddWithValue("@view_time", history.view_time);
            await command.ExecuteNonQueryAsync();
            return (int)command.LastInsertedId;
        }
        public static async Task<HistoryEntity> Find(int student_id)
        {
            var query = "SELECT work_id FROM history_view WHERE student_id=@student_id " ;

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("student_id",student_id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new HistoryEntity
                {
                    student_id=reader.GetInt32("student_id"),
                    work_id=reader.GetInt32("work_id"),
                    //view_time=reader.GetInt32("view_time"),
                              
                };
            }
            return null;
        }

       
    }

}