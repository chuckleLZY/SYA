using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using SyaApi.Plugins;
using SyaApi.Entities;

namespace SyaApi.DataAccessors
{
    public static class ApplyAccessor
    {
        ///<summery>
        /// 根据apply_id读取Apply
        /// dumei 08.24
        ///</summery>
        public static async Task<ApplyEntity> Read(int apply_id)
        {
            var query = "SELECT apply_id,student_id,teacher_id,work_id,resume_id,status FROM apply WHERE apply_id=@id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", apply_id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ApplyEntity()
                {
                    apply_id=reader.GetInt32("apply_id"),
                    student_id=reader.GetInt32("student_id"),
                    teacher_id=reader.GetInt32("teacher_id"),
                    work_id=reader.GetInt32("work_id"),
                    resume_id=reader.GetInt32("resume_id"),
                    status=reader.GetInt32("status")
                };
            }
            return null;
        }

        ///<summery>
        /// 查询student申请总数
        /// dumei 08.24
        ///</summery>
        public static async Task<int> GetNumOfApp(int stu_id)
        {
            var query = "SELECT count(apply_id) AS nof_app FROM apply WHERE student_id=@id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", stu_id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return reader.GetInt32("nof_app");
            }
            return -1; // the student have no application
        }

        ///<summery>
        /// Provider views all applications
        /// (非学生用户)查看所有申请
        /// dumei 08.23
        ///</summery>
        public static async Task<ApplyItemEntity> ProViewApps(int provider_id)
        {
            var apps = new ApplyItemEntity();
            apps.total = 0;
            apps.ApplyItem = new List<ApplyEntity>();
            var query = "SELECT apply_id,student_id,teacher_id,work_id,resume_id,status FROM apply WHERE teacher_id=@id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", provider_id);

            using var reader = await command.ExecuteReaderAsync();

            while ( reader.Read())
            {
                var temp = new ApplyEntity()
                {
                    apply_id=reader.GetInt32("apply_id"),
                    student_id=reader.GetInt32("student_id"),
                    teacher_id=reader.GetInt32("teacher_id"),
                    work_id=reader.GetInt32("work_id"),
                    resume_id=reader.GetInt32("resume_id"),
                    status=reader.GetInt32("status")
                };
                apps.total++;
                apps.ApplyItem.Add(temp);
            }
            return apps;
        }

        ///<summery>
        /// 设置申请状态
        /// dumei 08.23
        ///</summery>
        public static async Task<int> SetApplyStatus(int apply_id, int status)
        {
            var query = "UPDATE apply SET status=@status WHERE apply_id=@apply_id";
            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@apply_id", apply_id);
            command.Parameters.AddWithValue("@status", status);

            var row = await command.ExecuteNonQueryAsync();
            if (row>0) return apply_id;
            else return -1; // the apply not exists
        }

        public static async Task<int> Create(ApplyEntity apply)
        {
            var query = "INSERT INTO apply(student_id,teacher_id,work_id,resume_id,status)VALUES(@student_id,@teacher_id,@work_id,@resume_id,@status)";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@student_id",apply.student_id);
            command.Parameters.AddWithValue("@teacher_id",apply.teacher_id);
            command.Parameters.AddWithValue("@work_id",apply.work_id);
            command.Parameters.AddWithValue("@resume_id",apply.resume_id);
            command.Parameters.AddWithValue("@status",0);

            await command.ExecuteNonQueryAsync();
            return (int)command.LastInsertedId;

        }


        
    }
}