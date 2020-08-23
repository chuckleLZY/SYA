using System.Threading.Tasks;
using System.Text;
using SyaApi.Plugins;
using SyaApi.Entities;

namespace SyaApi.DataAccessors
{
    public static class AccountAccessor
    {
        public static async Task<AccountEntity> Find(string username)
        {
            var query = "SELECT id, username, email, password, role, registered FROM sya_accounts WHERE username = @Username OR email = @Username";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@Username", username);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new AccountEntity
                {
                    Id = reader.GetInt32("id"),
                    Username = reader.GetString("username"),
                    Email = reader.GetString("email"),
                    Password = Encoding.UTF8.GetString((byte[])reader["password"]),
                    Role = reader.GetSByte("role"),
                    Registered = reader.GetDateTime("registered"),
                };
            }

            return null;
        }

        public static async Task<AccountEntity> Read(int id)
        {
            var query = "SELECT id, username, email, password, role, registered FROM sya_accounts WHERE id = @Id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new AccountEntity
                {
                    Id = reader.GetInt32("id"),
                    Username = reader.GetString("username"),
                    Email = reader.GetString("email"),
                    Password = Encoding.UTF8.GetString((byte[])reader["password"]),
                    Role = reader.GetSByte("role"),
                    Registered = reader.GetDateTime("registered"),
                };
            }

            return null;
        }

        public static async Task<int> Create(AccountEntity account)
        {
            var query = "INSERT INTO sya_accounts(username, email, password, role) VALUES(@Username, @Email, @Password, @Role)";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@Username", account.Username);
            command.Parameters.AddWithValue("@Email", account.Email);
            command.Parameters.AddWithValue("@Password", account.Password);
            command.Parameters.AddWithValue("@Role", account.Role);

            await command.ExecuteNonQueryAsync();
            return (int)command.LastInsertedId;
        }

        public static async Task<int> Delete(int id)
        {
            var query = "DELETE FROM account WHERE id=@id";

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
