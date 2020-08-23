using System.Threading.Tasks;
using System.Text;
using SyaApi.Plugins;
using SyaApi.Entities;

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


        // id 重复、跳跃在Controller判断
        public static async Task<int> Create(UserEntity user)
        {
            var query = "INSERT INTO user VALUES(@id, @role, @name, @gender, @avatar, @email, @tel, @bank)";
            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", user.user_id);
            command.Parameters.AddWithValue("@role", user.user_role);
            command.Parameters.AddWithValue("@name", user.user_name);
            command.Parameters.AddWithValue("@gender", user.gender);
            command.Parameters.AddWithValue("@avatar", user.avatar);
            command.Parameters.AddWithValue("@email", user.email);
            command.Parameters.AddWithValue("@tel", user.tel);
            command.Parameters.AddWithValue("@bank", user.bank);

            //await command.ExecuteNonQueryAsync();
            //return (int)command.LastInsertedId;
            var row = await command.ExecuteNonQueryAsync();
            if(row>0)
            {
                return user.user_id;
            }
            return 0;
        }

        public static async Task<int> Change(UserEntity user)
        {
            var query = "UPDATE user SET user_name=@name, gender=@gender, avatar=@avatar, email=@email, tel=@tel, bank=@bank WHERE user_id=@id";
            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@id", user.user_id);
            command.Parameters.AddWithValue("@name", user.user_name);
            command.Parameters.AddWithValue("@gender", user.gender);
            command.Parameters.AddWithValue("@avatar", user.avatar);
            command.Parameters.AddWithValue("@email", user.email);
            command.Parameters.AddWithValue("@tel", user.tel);
            command.Parameters.AddWithValue("@bank", user.bank);

            var row = await command.ExecuteNonQueryAsync();
            if (row>0) return user.user_id;
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
