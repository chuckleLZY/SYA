using System.Threading.Tasks;
using System.Text;
using System;
using SyaApi.Plugins;
using SyaApi.Entities;

namespace SyaApi.DataAccessors
{
    public static class TakesAccessor
    {

        ///<summery>
        /// 创建takes,由ApplyController.ProManageApp调用
        /// work_time, absent_num, absent_time 自动设为0
        /// dumei 08.24
        ///</summery>
        ///<summery>
        /// 更新：takes.work_time赋值为work.total_time
        /// dumei 09.08
        ///</summery>
        public static async Task<int> Create(TakesEntity takes)
        {
            var query = @"INSERT INTO takes(student_id,work_id,work_time,absent_num,absent_time)
             VALUES(@student_id,@work_id,@work_time,@absent_num,@absent_time)";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@student_id",takes.student_id);
            command.Parameters.AddWithValue("@work_id",takes.work_id);
            command.Parameters.AddWithValue("@work_time",takes.work_time);
            command.Parameters.AddWithValue("@absent_num",0);
            command.Parameters.AddWithValue("@absent_time",0);

            await command.ExecuteNonQueryAsync();
            return (int)command.LastInsertedId;
        }

        ///<summery>
        /// 查询student工作总时长和请假总次数
        /// 更改：double to decimal
        /// dumei 08.24
        ///</summery>
        ///<summery>
        /// 更新：增加请假总时长
        /// dumei 09.08
        ///</summery>
        public static async Task<SumWorkAndAbsent> GetSumOfWorkAndAbsent(int stu_id)
        {
            var query = @"SELECT sum(work_time) AS sum_work_time, 
            CAST(sum(absent_num) AS SIGNED) AS sum_absent_num,
            sum(absent_time) AS sum_absent_time
            FROM takes WHERE student_id=@id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", stu_id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                SumWorkAndAbsent temp = new SumWorkAndAbsent()
                {
                    sum_work_time = reader["sum_work_time"] is System.DBNull ? 0 : reader.GetDecimal("sum_work_time"),
                    sum_absent_num = reader["sum_absent_num"] is System.DBNull ? 0 : Convert.ToInt32(reader.GetInt64("sum_absent_num")),
                    sum_absent_time = reader["sum_absent_time"] is System.DBNull ? 0 : reader.GetDecimal("sum_absent_time")
                };
                return temp;
            }
            return null; // the student have no takes
        }

        ///<summery>
        /// 增加请假次数和请假时间
        /// 由LeaveController.ProManageLeave调用
        /// dumei 08.24
        public static async Task<int> UpdateAbsent(int stu_id, int work_id, decimal ab_time)
        {
            var query = @"UPDATE takes SET absent_num=absent_num+1, absent_time=absent_time+@ab_time
            WHERE student_id=@stu_id AND work_id=@work_id";
            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@ab_time", ab_time);
            command.Parameters.AddWithValue("@stu_id", stu_id);
            command.Parameters.AddWithValue("@work_id", work_id);

            var row = await command.ExecuteNonQueryAsync();
            if (row>0) return 1; //success
            else return 0;
        }

    }
}