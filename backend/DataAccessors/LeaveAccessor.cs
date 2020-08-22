using System.Threading.Tasks;
using System.Text;
using SyaApi.Plugins;
using SyaApi.Entities;

namespace SyaApi.DataAccessors
{
    public static class LeaveAccessor
    {
        public static async Task<int> Create(LeaveEntity leave)
        {
            var query = "INSERT INTO leave_information(student_id,work_id,content,leave_time,proof) VALUES(@student_id,@work_id,@content,@leave_time,@proof)";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@student_id",leave.student_id);
            command.Parameters.AddWithValue("@work_id",leave.work_id);
            command.Parameters.AddWithValue("@content",leave.content);
            command.Parameters.AddWithValue("@leave_time",leave.leave_time);
            command.Parameters.AddWithValue("@proof",leave.proof);

            await command.ExecuteNonQueryAsync();
            return (int)command.LastInsertedId;

        }
    }
}