using System.Threading.Tasks;
using System.Text;
using SyaApi.Plugins;
using SyaApi.Entities;
using SyaApi.Requests;

namespace SyaApi.DataAccessors
{
    public static class UserAccessor
    {
        public static async Task<UserEntity> Read(int id)
        {
            var query = "SELECT user_id, user_role, user_name, gender, avatar, email, tel, bank FROM user WHERE user_id = @Id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new UserEntity
                {
                    user_id = reader.GetInt32("user_id"),
                    user_role = reader.GetSByte("user_role"),
                    user_name = reader.GetString("user_name"),
                    gender = reader.GetBoolean("gender"),
                    avatar = reader.GetString("avatar"),
                    email = reader.GetString("email"),
                    tel = reader.GetString("tel"),
                    bank = reader.GetString("bank"),
                };
            }
            
            return null;
        }

        public static async Task<int> Find(int id) 
        {
            var query = "SELECT user_id FROM user WHERE user_id = @Id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return 1; // exists
            }
            return 0; // not exists
        }

        public static async Task<int> CheckRole(int id)
        {
            var query = "SELECT user_role FROM user WHERE user_id = @Id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return reader.GetInt32("user_role");
            }
            return -1; // not exists
        }
        
        public static async Task<int> Create(AccountEntity account)
        {
            // id 重复、跳跃在Controller判断
            var query = "INSERT INTO user VALUES(@id, @role, @name, @gender, @avatar, @email, @tel, @bank)";
            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", account.Id);
            command.Parameters.AddWithValue("@role", account.Role);
            command.Parameters.AddWithValue("@name", account.Username);
            command.Parameters.AddWithValue("@gender", false);
            command.Parameters.AddWithValue("@avatar", "");
            command.Parameters.AddWithValue("@email", account.Email);
            command.Parameters.AddWithValue("@tel", "");
            command.Parameters.AddWithValue("@bank", "");

            //await command.ExecuteNonQueryAsync();
            //return (int)command.LastInsertedId;
            var row = await command.ExecuteNonQueryAsync();
            if(row>0)
            {
                return account.Id;
            }
            return 0;
        }

        public static async Task<int> Change(int id, UserRequest user)
        {
            var query = "UPDATE user SET gender=@gender, avatar=@avatar, tel=@tel, bank=@bank WHERE user_id=@id";
            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@gender", user.gender);
            command.Parameters.AddWithValue("@avatar", user.avatar);
            command.Parameters.AddWithValue("@tel", user.tel);
            command.Parameters.AddWithValue("@bank", user.bank);

            var row = await command.ExecuteNonQueryAsync();
            if (row>0) return id;
            else return 0;
        }

        public static async Task<int> Delete(int id)
        {
            var query = "DELETE FROM user WHERE user_id=@id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@id", id);
            var row = await command.ExecuteNonQueryAsync();
            if(row>0)
            {
                return id;
            }
            return 0;
        }
    }
}
