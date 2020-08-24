using System.Threading.Tasks;
using System.Text;
using SyaApi.Plugins;
using SyaApi.Entities;

namespace SyaApi.DataAccessors
{
    public static class SalaryAccessor
    {
        ///<summery>
        /// 查询student总收入
        /// dumei 08.24
        ///</summery>
        public static async Task<decimal> GetSumOfSalary(int stu_id)
        {
            var query = "SELECT sum(num) AS sum_salary FROM salary WHERE student_id=@id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", stu_id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return reader["sum_salary"] is System.DBNull ? 0 : reader.GetDecimal("sum_salary");
            }
            return 0; // the student have no salary record
        }


    }
}