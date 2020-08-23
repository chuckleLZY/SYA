using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using SyaApi.Plugins;
using SyaApi.Entities;

namespace SyaApi.DataAccessors
{
    public static class WorkAccessor
    {
        //找到已拥有工作的work_id
        public static async Task<TakesItemEntity> FindOwnWork(int id)
        {
            TakesItemEntity takes=new TakesItemEntity();
            takes.total=0;
            takes.TakesItem=new List<TakesEntity>();
            var query = "SELECT student_id,work_id,takes.work_time,absent_num,absent_time,work_name FROM takes JOIN work USING (work_id)WHERE student_id=@id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id",id);

            using var reader = await command.ExecuteReaderAsync();

            while ( reader.Read())
            {
                TakesEntity temp=new TakesEntity();
                
                temp.student_id=reader.GetInt32("student_id");
                temp.work_id=reader.GetInt32("work_id");
                temp.work_time=reader.GetDouble("work_time");
                temp.absent_num=reader.GetInt32("absent_num");
                temp.absent_time=reader.GetDouble("absent_time");
                temp.work_name=reader.GetString("work_name");
                takes.total++;
                takes.TakesItem.Add(temp);
            }
            return takes;
        }

        //根据work_id查找工作详细信息
        public static async Task<WorkEntity> FindWorkInfo(int id)
        {
            WorkEntity temp=new WorkEntity();
            var query = "SELECT teacher_id,work_id,work_name,cover,work_description,address,salary,work_time,likes_num,collect_num,share_num FROM work WHERE work_id=@id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id",id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new WorkEntity
                {
                    work_name=reader.GetString("work_name"),
                    teacher_id=reader.GetInt32("teacher_id"),
                    work_id=reader.GetInt32("work_id"),
                    cover=reader.GetString("cover"),
                    work_description=reader.GetString("work_description"),
                    address=reader.GetString("address"),
                    salary=reader.GetInt32("salary"),
                    work_time=reader.GetString("work_time"),
                    likes_num=reader.GetInt32("likes_num"),
                    collect_num=reader.GetInt32("collect_num"),
                    share_num=reader.GetInt32("share_num")
                };
            }
            return null;
        }

    }
}