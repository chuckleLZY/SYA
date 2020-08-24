using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using SyaApi.Plugins;
using SyaApi.Entities;

namespace SyaApi.DataAccessors
{
    public static class LeaveAccessor
    {

        ///<summery>
        /// 根据leave_id读取Leave
        /// 不包含student_name和work_name
        /// dumei 08.24
        ///</summery>
        public static async Task<LeaveEntity> Read(int leave_id)
        {
            var query = @"SELECT leave_id,student_id,work_id,
            content,leave_time,proof,status,leave_duration
            FROM leave_information
            WHERE leave_id=@id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", leave_id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new LeaveEntity()
                {
                    leave_id=reader.GetInt32("leave_id"),
                    student_id=reader.GetInt32("student_id"),
                    work_id=reader.GetInt32("work_id"),
                    content=reader.GetString("content"),
                    leave_time=reader.GetString("leave_time"),
                    proof=reader.GetString("proof"),
                    status=reader.GetInt32("status"),
                    leave_duration=reader.GetDecimal("leave_duration")
                };
            }
            return null;
        }
        public static async Task<LeaveEntity> Find(int id)
        {
            var query = @"SELECT leave_id,leave_information.student_id,
            leave_information.work_id,content,leave_time,proof,
            user_name,work_name,status,leave_duration 
            FROM (leave_information natural join user) join work using (work_id) 
            WHERE leave_id=@id and leave_information.student_id=user.user_id;";

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
                    status=reader.GetInt32("status"),
                    leave_duration=reader.GetDecimal("leave_duration")          
                };
            }
            return null;
        }

        public static async Task<int> Create(LeaveEntity leave)
        {
            var query = @"INSERT INTO leave_information(student_id,work_id,content,leave_time,proof,status,leave_duration)
             VALUES(@student_id,@work_id,@content,@leave_time,@proof,@status,@duration)";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@student_id",leave.student_id);
            command.Parameters.AddWithValue("@work_id",leave.work_id);
            command.Parameters.AddWithValue("@content",leave.content);
            command.Parameters.AddWithValue("@leave_time",leave.leave_time);
            command.Parameters.AddWithValue("@proof",leave.proof);
            command.Parameters.AddWithValue("@status",0);
            command.Parameters.AddWithValue("@duration",0);

            await command.ExecuteNonQueryAsync();
            return (int)command.LastInsertedId;

        }

        ///<summery>
        /// (非学生用户)查看所有请假申请
        /// 这里不查询student.name和work.name
        /// dumei 08.24
        ///</summery>
        public static async Task<LeaveItemEntity> ProViewLeaves(int provider_id)
        {
            var leave_apps = new LeaveItemEntity();
            leave_apps.total = 0;
            leave_apps.leaveItem = new List<LeaveEntity>();
            var query = @"SELECT leave_id,student_id,work_id,
            content,leave_time,proof,status,leave_duration
            FROM leave_information NATURAL JOIN work
            WHERE teacher_id=@id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", provider_id);

            using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var temp = new LeaveEntity()
                {
                    leave_id=reader.GetInt32("leave_id"),
                    student_id=reader.GetInt32("student_id"),
                    work_id=reader.GetInt32("work_id"),
                    content=reader.GetString("content"),
                    leave_time=reader.GetString("leave_time"),
                    proof=reader.GetString("proof"),
                    status=reader.GetInt32("status"),
                    leave_duration=reader.GetDecimal("leave_duration")   
                };
                leave_apps.total++;
                leave_apps.leaveItem.Add(temp);
            }
            return leave_apps;
        }

        ///<summery>
        /// 设置请假申请状态
        /// dumei 08.24
        ///</summery>
        public static async Task<int> SetLeaveStatus(int leave_id, int status)
        {
            var query = "UPDATE leave_information SET status=@status WHERE leave_id=@leave_id";
            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@leave_id", leave_id);
            command.Parameters.AddWithValue("@status", status);

            var row = await command.ExecuteNonQueryAsync();
            if (row>0) return leave_id;
            else return -1; // the leave information not exists
        }



    }
}