using System.Threading.Tasks;
using System.Text;
using SyaApi.Plugins;
using SyaApi.Entities;

namespace SyaApi.DataAccessors
{
    public static class CommentAccessor
    {
       
        public static async Task<int> Create(CommentEntity comment)
        {
            var query = "INSERT INTO comment(sender_id,work_id,content) values(@sender_id,@work_id,@content)";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

           

           command.Parameters.AddWithValue("@sender_id", comment.sender_id);
           command.Parameters.AddWithValue("@work_id", comment.work_id);
            command.Parameters.AddWithValue("@content", comment.content);
            await command.ExecuteNonQueryAsync();
            return (int)command.LastInsertedId;
        }

       
    }

}