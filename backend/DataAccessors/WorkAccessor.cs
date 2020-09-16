using System.Globalization;
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
            var query = "SELECT student_id,work_id,absent_num,absent_time,work_name FROM takes JOIN work USING (work_id)WHERE student_id=@id";

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
                //temp.work_time=reader.GetDouble("work_time");
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
            var query = "SELECT teacher_id,work_id,work_name,cover,work_description,address,salary,likes_num,collect_num,start_day,end_day,start_time,end_time,total_time,week_day FROM work WHERE work_id=@id";

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
                    //work_time=reader.GetString("work_time"),
                    likes_num=reader.GetInt32("likes_num"),
                    collect_num=reader.GetInt32("collect_num"),
                    
                    start_day=reader.GetString("start_day"),
                    end_day=reader.GetString("end_day"),
                    start_time=reader.GetString("start_time"),
                    end_time=reader.GetString("end_time"),
                    total_time=reader.GetDouble("total_time"),
                    week_day=reader.GetInt32("week_day")
                };
            }
            return null;
        }

        ///<summery>
        /// (非学生用户)查看历史发布工作
        /// dumei 08.23
        ///</summery>
        ///<summery>
        /// 更改:增加时间变量
        /// dumei 09.08
        ///</summery>
        public static async Task<WorkItemEntity> FindHistoryWork(int provider_id)
        {
            var provide = new WorkItemEntity();
            provide.total = 0;
            provide.workItem = new List<WorkEntity>();
            var query = @"SELECT teacher_id,work_id,work_name,cover,work_description,address,salary,
            likes_num,collect_num,start_day,end_day,start_time,end_time,total_time,week_day
            FROM work WHERE teacher_id=@id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", provider_id);

            using var reader = await command.ExecuteReaderAsync();

            while ( reader.Read())
            {
                var temp = new WorkEntity()
                {
                    work_name=reader.GetString("work_name"),
                    teacher_id=reader.GetInt32("teacher_id"),
                    work_id=reader.GetInt32("work_id"),
                    cover=reader.GetString("cover"),
                    work_description=reader.GetString("work_description"),
                    address=reader.GetString("address"),
                    salary=reader.GetInt32("salary"),
                    //work_time=reader.GetString("work_time"),
                    likes_num=reader.GetInt32("likes_num"),
                    collect_num=reader.GetInt32("collect_num"),
                    //share_num=reader.GetInt32("num"),
                    start_day=reader.GetString("start_day"),
                    end_day=reader.GetString("end_day"),
                    start_time=reader.GetString("start_time"),
                    end_time=reader.GetString("end_time"),
                    total_time=reader.GetDouble("total_time"),
                    week_day=reader.GetInt32("week_day")
                };
                provide.total++;
                provide.workItem.Add(temp);
            }
            return provide;
        }

        ///<summery>
        /// 根据work_id查找work_name
        /// 返回Constants.Error.NotFindString表示work不存在
        /// dumei 08.24
        ///</summery>
        public static async Task<string> GetWorkName(int id)
        {
            WorkEntity temp=new WorkEntity();
            var query = "SELECT work_name FROM work WHERE work_id=@id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id",id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return reader.GetString("work_name");
            }
            return Constants.Error.NotFindString; // work not exists
        }

        ///<summery>
        /// 根据work_id查找work.total_time
        /// 返回Constants.Error.NotFindString表示work不存在
        /// dumei 08.24
        ///</summery>
        public static async Task<double> GetWorkTotalTime(int id)
        {
            WorkEntity temp=new WorkEntity();
            var query = "SELECT total_time FROM work WHERE work_id=@id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id",id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return reader.GetDouble("total_time");
            }
            return Constants.Error.NotFindDouble; // work not exists
        }


        ///<summery>
        /// (非学生用户)创建工作
        /// 设置 likes_num, collect_num, share_num 为 0
        /// dumei 08.23
        ///</summery>
        ///<summery>
        /// 增加start_time,end_time,start_day,end_day,week_day
        /// dumei 09.08
        ///</summery>
        public static async Task<int> Create(WorkEntity work)
        {
            var query = @"INSERT INTO work(teacher_id,work_name,cover,work_description,address,salary,
            likes_num,collect_num,start_day,end_day,start_time,end_time,total_time,week_day) 
            VALUES(@teacher_id,@work_name,@cover,@description,@address,@salary,
            @likes,@collect,@start_day,@end_day,@start_time,@end_time,@total_time,@week_day)";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@teacher_id", work.teacher_id);
            //command.Parameters.AddWithValue("@work_id", work.work_id);
            command.Parameters.AddWithValue("@work_name", work.work_name);
            command.Parameters.AddWithValue("@cover", work.cover);
            command.Parameters.AddWithValue("@description", work.work_description);
            command.Parameters.AddWithValue("@address", work.address);
            command.Parameters.AddWithValue("@salary", work.salary);
            //command.Parameters.AddWithValue("@work_time", work.work_time);
            //command.Parameters.AddWithValue("@work_time", "none"); //to be delete
            command.Parameters.AddWithValue("@start_day", work.start_day);
            command.Parameters.AddWithValue("@end_day", work.end_day);
            command.Parameters.AddWithValue("@start_time", work.start_time);
            command.Parameters.AddWithValue("@end_time", work.end_time);
            command.Parameters.AddWithValue("@total_time", work.total_time);
            command.Parameters.AddWithValue("@week_day", work.week_day);
            command.Parameters.AddWithValue("@likes", 0);
            command.Parameters.AddWithValue("@collect", 0);
            //command.Parameters.AddWithValue("@share", 0);

            await command.ExecuteNonQueryAsync();
            return (int)command.LastInsertedId;
        }

        ///<summery>
        /// 用户查看所有工作
        /// chuckle 8.25
        ///</summery>
        public static async Task<WorkItemEntity> FindAllWork()
        {
            WorkItemEntity work=new WorkItemEntity();
            work.total=0;
            work.workItem=new List<WorkEntity>();
            var query = "SELECT teacher_id,work_id,work_name,cover,work_description,address,salary,likes_num,collect_num,start_day,end_day,start_time,end_time,total_time,week_day FROM work";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            using var reader = await command.ExecuteReaderAsync();

            while ( reader.Read())
            {
                WorkEntity temp=new WorkEntity();
                
                temp.teacher_id=reader.GetInt32("teacher_id");
                temp.work_id=reader.GetInt32("work_id");
                temp.work_name=reader.GetString("work_name");
                temp.cover=reader.GetString("cover");
                temp.work_description=reader.GetString("work_description");
                temp.address=reader.GetString("address");
                //temp.work_time=reader.GetString("work_time");
                temp.salary=reader.GetInt32("salary");
                temp.likes_num=reader.GetInt32("likes_num");
                temp.collect_num=reader.GetInt32("collect_num");

                temp.start_day=reader.GetString("start_day");
                temp.end_day=reader.GetString("end_day");
                temp.start_time=reader.GetString("start_time");
                temp.end_time=reader.GetString("end_time");
                temp.total_time=reader.GetDouble("total_time");
                temp.week_day=reader.GetInt32("week_day");
                
                work.total++;
                work.workItem.Add(temp);
            }
            return work;
        }

        ///<summery>
        /// 用户搜索工作
        /// chuckle 8.25
        ///</summery>
        public static async Task<WorkItemEntity> FindWork(string search)
        {
            WorkItemEntity work=new WorkItemEntity();
            work.total=0;
            work.workItem=new List<WorkEntity>();
            var query = "SELECT teacher_id,work_id,work_name,cover,work_description,address,salary,likes_num,collect_num,start_day,end_day,start_time,end_time,total_time,week_day FROM work WHERE work_name LIKE @search OR work_description LIKE @search OR address LIKE @search ";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@search",search);
            using var reader = await command.ExecuteReaderAsync();

            while ( reader.Read())
            {
                WorkEntity temp=new WorkEntity();
                
                temp.teacher_id=reader.GetInt32("teacher_id");
                temp.work_id=reader.GetInt32("work_id");
                temp.work_name=reader.GetString("work_name");
                temp.cover=reader.GetString("cover");
                temp.work_description=reader.GetString("work_description");
                temp.address=reader.GetString("address");
                //temp.work_time=reader.GetString("work_time");
                temp.salary=reader.GetInt32("salary");
                temp.likes_num=reader.GetInt32("likes_num");
                temp.collect_num=reader.GetInt32("collect_num");
                
                temp.start_day=reader.GetString("start_day");
                temp.end_day=reader.GetString("end_day");
                temp.start_time=reader.GetString("start_time");
                temp.end_time=reader.GetString("end_time");
                temp.total_time=reader.GetDouble("total_time");
                temp.week_day=reader.GetInt32("week_day");
                
                work.total++;
                work.workItem.Add(temp);
            }
            return work;
        }

        ///<summery>
        /// 根据work_id查找teacher_id
        /// chuckle 8.26
        ///</summery>
        public static async Task<int> GetTeacher(int id)
        {
            var query = "SELECT teacher_id FROM work WHERE work_id=@id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id",id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return reader.GetInt32("teacher_id");
            }
            return -1; // work not exists
        }

        ///<summery>
        /// 获得工作时间信息
        /// 用于判断请假是否在工作时间内,用于leaveController.CheckRestTime()
        /// dumei 09.12
        ///</summery>
        public static async Task<WorkTimeEntity> GetWorkTime(int work_id)
        {
            var query = @"SELECT start_day,end_day,start_time,end_time,week_day
             FROM work WHERE work_id=@id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id",work_id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new WorkTimeEntity {
                    start_day=reader.GetString("start_day"),
                    end_day=reader.GetString("end_day"),
                    start_time=reader.GetString("start_time"),
                    end_time=reader.GetString("end_time"),
                    week_day=reader.GetInt32("week_day")
                };
            }
            return new WorkTimeEntity {
                start_day="not found"
            }; // work not exists
        }

        ///<summery>
        /// (非学生用户)修改工作信息
        /// 设置 likes_num, collect_num, share_num 为 0
        /// chuckle 8.28
        ///</summery>
        public static async Task<int> Update(WorkEntity work)
        {
            var query = "UPDATE work SET teacher_id=@teacher_id,work_name=@work_name,cover=@cover,work_description=@work_description,address=@address,salary=@salary,start_day=@start_day,end_day=@end_day,start_time=@start_time,end_time=@end_time,total_time=@total_time,week_day=@week_day WHERE work_id =@work_id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@teacher_id", work.teacher_id);
            command.Parameters.AddWithValue("@work_id", work.work_id);
            command.Parameters.AddWithValue("@work_name", work.work_name);
            command.Parameters.AddWithValue("@cover", work.cover);
            command.Parameters.AddWithValue("@work_description", work.work_description);
            command.Parameters.AddWithValue("@address", work.address);
            command.Parameters.AddWithValue("@salary", work.salary);
            command.Parameters.AddWithValue("@start_day", work.start_day);
            command.Parameters.AddWithValue("@end_day", work.end_day);
            command.Parameters.AddWithValue("@start_time", work.start_time);
            command.Parameters.AddWithValue("@end_time", work.end_time);
            command.Parameters.AddWithValue("@total_time", work.total_time);
            command.Parameters.AddWithValue("@week_day", work.week_day);
            var row = await command.ExecuteNonQueryAsync();

            if(row>0)
            {
                return work.work_id;
            }
            return 0;
        }
    
        ///<summery>
        /// 用户点赞
        /// chuckle 9.9
        ///</summery>
        public static async Task<int> getlike(int work_id)
        {
            var query = "UPDATE work SET likes_num=likes_num+1 WHERE work_id =@work_id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@work_id", work_id);
            var row = await command.ExecuteNonQueryAsync();

            if(row>0)
            {
                return 1;
            }
            return 0;
        
        }
    
        ///<summery>
        /// 用户点赞
        /// chuckle 9.9
        ///</summery>
        public static async Task<int> getnolike(int work_id)
        {
            var query = "UPDATE work SET likes_num=likes_num-1 WHERE work_id =@work_id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@work_id", work_id);
            var row = await command.ExecuteNonQueryAsync();

            if(row>0)
            {
                return 1;
            }
            return 0;
        
        }


        ///<summery>
        /// 收藏数增加
        /// chuckle 9.9
        ///</summery>
        public static async Task<int> upfavnum(int work_id)
        {
            var query = "UPDATE work SET collect_num=collect_num+1 WHERE work_id =@work_id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@work_id", work_id);
            var row = await command.ExecuteNonQueryAsync();

            if(row>0)
            {
                return 1;
            }
            return 0;
        
        }
    
        ///<summery>
        /// 收藏数减少
        /// chuckle 9.9
        ///</summery>
        public static async Task<int> downfavnum(int work_id)
        {
            var query = "UPDATE work SET collect_num=collect_num-1 WHERE work_id =@work_id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@work_id", work_id);
            var row = await command.ExecuteNonQueryAsync();

            if(row>0)
            {
                return 1;
            }
            return 0;
        
        }

        ///<summery>
        /// 学生搜索个人拥有工作
        /// chuckle 8.25
        ///</summery>
        public static async Task<WorkItemEntity> FindOwnWork(string search,int id)
        {
            WorkItemEntity work=new WorkItemEntity();
            work.total=0;
            work.workItem=new List<WorkEntity>();
            var query = "SELECT teacher_id,work_id,work_name,cover,work_description,address,salary,likes_num,collect_num,start_day,end_day,start_time,end_time,total_time,week_day FROM work JOIN takes USING (work_id) WHERE student_id=@id AND( work_name LIKE @search OR work_description LIKE @search OR address LIKE @search) ";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@search",search);
            command.Parameters.AddWithValue("@id",id);
            using var reader = await command.ExecuteReaderAsync();

            while ( reader.Read())
            {
                WorkEntity temp=new WorkEntity();
                
                temp.teacher_id=reader.GetInt32("teacher_id");
                temp.work_id=reader.GetInt32("work_id");
                temp.work_name=reader.GetString("work_name");
                temp.cover=reader.GetString("cover");
                temp.work_description=reader.GetString("work_description");
                temp.address=reader.GetString("address");
                
                temp.salary=reader.GetInt32("salary");
                temp.likes_num=reader.GetInt32("likes_num");
                temp.collect_num=reader.GetInt32("collect_num");
                
                temp.start_day=reader.GetString("start_day");
                temp.end_day=reader.GetString("end_day");
                temp.start_time=reader.GetString("start_time");
                temp.end_time=reader.GetString("end_time");
                temp.total_time=reader.GetDouble("total_time");
                temp.week_day=reader.GetInt32("week_day");
                
                work.total++;
                work.workItem.Add(temp);
            }
            return work;
        }

        

    }
}