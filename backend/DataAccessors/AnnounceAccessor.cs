using System.Threading.Tasks;
using System.Text;
using SyaApi.Plugins;
using SyaApi.Entities;

namespace SyaApi.DataAccessors
{
    public static class AnnounceAccessor
    {
       
        public static async Task<int> Create(AnnounceEntity announcement)
        {
            var query = "INSERT INTO announcement(user_id,content) values(@user_id,@content)";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@user_id", announcement.user_id);
            command.Parameters.AddWithValue("@content", announcement.content);
           
            await command.ExecuteNonQueryAsync();
            return (int)command.LastInsertedId;
        }

       
    }

}