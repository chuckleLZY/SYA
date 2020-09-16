using System;
using System.Globalization;
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
            content,proof,status,leave_duration,
            request_time,leave_day,leave_start,leave_end
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
                    //leave_time=reader.GetString("leave_time"),
                    proof=reader.GetString("proof"),
                    status=reader.GetInt32("status"),
                    leave_duration=reader.GetDouble("leave_duration"),
                    request_time=reader.GetDateTime("request_time"),
                    leave_day=reader.GetString("leave_day"),
                    leave_start=reader.GetString("leave_start"),
                    leave_end=reader.GetString("leave_end")  
                };
            }
            return null;
        }


        public static async Task<LeaveEntity> Find(int id)
        {
            var query = @"SELECT leave_id,leave_information.student_id,
            leave_information.work_id,content,proof,
            user_name,work_name,status,leave_duration,request_time,leave_day,leave_start,leave_end
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
                    //leave_time=reader.GetString("leave_time"),
                    proof=reader.GetString("proof"),
                    student_name=reader.GetString("user_name"),
                    work_name=reader.GetString("work_name"),
                    status=reader.GetInt32("status"),
                    leave_duration=reader.GetDouble("leave_duration"),
                    request_time=reader.GetDateTime("request_time"),
                    leave_day=reader.GetString("leave_day"),
                    leave_start=reader.GetString("leave_start"),
                    leave_end=reader.GetString("leave_end")       
                };
            }
            return null;
        }

        ///<summery>
        /// 请假查重
        /// 已经请假成功返回1
        /// 正在申请请假返回2
        /// 其它（可以继续请假）返回0
        /// dumei 09.15
        ///</summery>
        public static async Task<int> CheckLeave(int stu_id, int work_id, string request_lv_day)
        {
            var query = @"SELECT status,leave_day
            FROM leave_information
            WHERE student_id=@stu_id AND work_id=@work_id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@stu_id", stu_id);
            command.Parameters.AddWithValue("@work_id", work_id);

            using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                int status = reader.GetInt32("status");
                string leave_day = reader.GetString("leave_day");
                if (leave_day.Equals(request_lv_day))
                {
                    if (status == Constants.ApplyStatus.Accepted)
                    {
                        return 1;
                    }
                    else if (status == Constants.ApplyStatus.Applying)
                    {
                        return 2;
                    }
                }
            }
            return 0;
        }

        public static async Task<int> Create(LeaveEntity leave)
        {
            var query = @"INSERT INTO leave_information(student_id,work_id,content,
            proof,status,leave_duration,leave_day,leave_start,leave_end)
             VALUES(@student_id,@work_id,@content,
             @proof,@status,@duration,@day,@start,@end)";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@student_id",leave.student_id);
            command.Parameters.AddWithValue("@work_id",leave.work_id);
            command.Parameters.AddWithValue("@content",leave.content);
            //command.Parameters.AddWithValue("@leave_time",""); //to be delete
            command.Parameters.AddWithValue("@proof",leave.proof);
            command.Parameters.AddWithValue("@status",0);
            command.Parameters.AddWithValue("@duration",leave.leave_duration);
            command.Parameters.AddWithValue("@day",leave.leave_day);
            command.Parameters.AddWithValue("@start",leave.leave_start);
            command.Parameters.AddWithValue("@end",leave.leave_end);

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
            content,proof,status,leave_duration,
            request_time,leave_day,leave_start,leave_end
            FROM leave_information NATURAL JOIN work
            WHERE teacher_id=@id ORDER BY status ASC";

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
                    //leave_time=reader.GetString("leave_time"),
                    proof=reader.GetString("proof"),
                    status=reader.GetInt32("status"),
                    leave_duration=reader.GetDouble("leave_duration"),
                    request_time=reader.GetDateTime("request_time"),
                    leave_day=reader.GetString("leave_day"),
                    leave_start=reader.GetString("leave_start"),
                    leave_end=reader.GetString("leave_end")  
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

        //学生查看自己的请假信息
         public static async Task<LeaveItemEntity> ViewLeave(int student_id)
        {
            LeaveItemEntity leave=new LeaveItemEntity();
            leave.total=0;
            leave.leaveItem=new List<LeaveEntity>();
            var query = "SELECT leave_id,student_id,work_id,content,proof,status,request_time,leave_duration FROM leave_information WHERE student_id=@student_id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@student_id",student_id);
            using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                LeaveEntity temp=new LeaveEntity();
                
                temp.leave_id=reader.GetInt32("leave_id");
                temp.student_id=reader.GetInt32("student_id");
                temp.work_id=reader.GetInt32("work_id");
                temp.content=reader.GetString("content");
                //temp.leave_time=reader.GetString("leave_time");
                temp.proof=reader.GetString("proof");
                temp.status=reader.GetInt32("status");
                temp.leave_duration=reader.GetDouble("leave_duration");
                temp.request_time=reader.GetDateTime("request_time");
                
                leave.total++;
                leave.leaveItem.Add(temp);
            }
            return leave;
        }

        //update请假
        public static async Task<int> Update(LeaveEntity entity)
        {
            var query = "UPDATE leave_information SET content=@content,proof=@proof,leave_duration=@duration,leave_day=@day,leave_start=@start,leave_end=@end WHERE leave_id=@id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@id", entity.leave_id);
            command.Parameters.AddWithValue("@content",entity.content);
            
            command.Parameters.AddWithValue("@proof",entity.proof);
            
            command.Parameters.AddWithValue("@duration",entity.leave_duration);
            command.Parameters.AddWithValue("@day",entity.leave_day);
            command.Parameters.AddWithValue("@start",entity.leave_start);
            command.Parameters.AddWithValue("@end",entity.leave_end);

            var row = await command.ExecuteNonQueryAsync();
            if(row>0)
            {
                return entity.leave_id;
            }
            return 0;
        }

        //delete/recall
        public static async Task<int> Delete(int leave_id)
        {
            var query = "DELETE FROM leave_information where leave_id=@id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@id", leave_id);
            var row = await command.ExecuteNonQueryAsync();
            if(row>0)
            {
                return leave_id;
            }
            return 0;
        }


    }
}