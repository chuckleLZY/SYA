using System.Threading.Tasks;
using System.Text;
using SyaApi.Plugins;
using SyaApi.Entities;

namespace SyaApi.DataAccessors
{
    public static class LeaveAccessor
    {

        public static async Task<LeaveEntity> Find(int id)
        {
            var query = "SELECT leave_id,leave_information.student_id,leave_information.work_id,content,leave_time,proof,status,user_name,work_name FROM (leave_information natural join user) join work using (work_id) WHERE leave_id=@id and leave_information.student_id=user.user_id;";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id",id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new LeaveEntity
                {
                    leave_id=reader.GetInt32("leave_id"),
                    student_id=reader.GetInt32("student_id"),
                    work_id=reader.GetInt32("work_id"),
                    content=reader.GetString("content"),
                    leave_time=reader.GetString("leave_time"),
                    proof=reader.GetString("proof"),
                    student_name=reader.GetString("user_name"),
                    work_name=reader.GetString("work_name"),
                    status=reader.GetInt32("status")             
                };
            }
            return null;
        }

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