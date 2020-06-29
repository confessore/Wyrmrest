using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Wyrmrest.Web.Services.Interfaces;
using Wyrmrest.Web.Statics;

namespace Wyrmrest.Web.Services
{
    public class MariaService : IMariaService
    {
        public async Task<bool> AccountExistsAsync(string username)
        {
            string sql = "SELECT EXISTS(SELECT * FROM account WHERE username = @username)";
            using MySqlConnection connection = new MySqlConnection(Strings.AuthConnectionString);
            connection.Open();
            using MySqlCommand command = new MySqlCommand(sql, connection);
            var paramUsername = new MySqlParameter("username", MySqlDbType.VarChar)
            {
                Value = username.ToUpper()
            };
            command.Parameters.Add(paramUsername);
            var result = Convert.ToInt32(await command.ExecuteScalarAsync());
            connection.Close();
            return result == 1;
        }

        public async Task CreateNewAccountAsync(string username, string password, byte expansion)
        {
            string sql = "INSERT INTO account (username, sha_pass_hash, expansion) VALUES (@username, @password, @expansion)";
            using MySqlConnection connection = new MySqlConnection(Strings.AuthConnectionString);
            connection.Open();
            using MySqlCommand command = new MySqlCommand(sql, connection);
            var paramUsername = new MySqlParameter("username", MySqlDbType.VarChar)
            {
                Value = username.ToUpper()
            };
            var paramPassword = new MySqlParameter("password", MySqlDbType.VarChar)
            {
                Value = await ComputeSHA1PassHash(username, password)
            };
            var paramExpansion = new MySqlParameter("expansion", MySqlDbType.Byte)
            {
                Value = expansion
            };
            command.Parameters.Add(paramUsername);
            command.Parameters.Add(paramPassword);
            command.Parameters.Add(paramExpansion);
            await command.ExecuteNonQueryAsync();
            connection.Close();
        }

        public async Task UpdatePasswordAsync(string username, string password)
        {
            string sql = "UPDATE account SET sha_pass_hash = @password, sessionkey = @sessionkey, v = @v, s = @s WHERE username = @username";
            using MySqlConnection connection = new MySqlConnection(Strings.AuthConnectionString);
            connection.Open();
            using MySqlCommand command = new MySqlCommand(sql, connection);
            var paramUsername = new MySqlParameter("username", MySqlDbType.VarChar)
            {
                Value = username.ToUpper()
            };
            var paramPassword = new MySqlParameter("password", MySqlDbType.VarChar)
            {
                Value = await ComputeSHA1PassHash(username, password)
            };
            var paramSessionkey = new MySqlParameter("sessionkey", MySqlDbType.VarChar)
            {
                Value = string.Empty
            };
            var paramV = new MySqlParameter("v", MySqlDbType.VarChar)
            {
                Value = string.Empty
            };
            var paramS = new MySqlParameter("s", MySqlDbType.VarChar)
            {
                Value = string.Empty
            };
            command.Parameters.Add(paramUsername);
            command.Parameters.Add(paramPassword);
            command.Parameters.Add(paramSessionkey);
            command.Parameters.Add(paramV);
            command.Parameters.Add(paramS);
            await command.ExecuteNonQueryAsync();
            connection.Close();
        }
        
        public async Task<int> GetAccountIdAsync(string username)
        {
            string sql = "SELECT id FROM account WHERE username = @username";
            using MySqlConnection connection = new MySqlConnection(Strings.AuthConnectionString);
            connection.Open();
            using MySqlCommand command = new MySqlCommand(sql, connection);
            var paramUsername = new MySqlParameter("username", MySqlDbType.VarChar)
            {
                Value = username.ToUpper()
            };
            command.Parameters.Add(paramUsername);
            var result = Convert.ToInt32(await command.ExecuteScalarAsync());
            connection.Close();
            return result;
        }

        public async Task<bool> BanExistsAsync(int id)
        {
            string sql = "SELECT EXISTS(SELECT * FROM account_banned WHERE id = @id)";
            using MySqlConnection connection = new MySqlConnection(Strings.AuthConnectionString);
            connection.Open();
            using MySqlCommand command = new MySqlCommand(sql, connection);
            var paramId = new MySqlParameter("id", MySqlDbType.Int32)
            {
                Value = id
            };
            command.Parameters.Add(paramId);
            var result = Convert.ToInt32(await command.ExecuteScalarAsync());
            connection.Close();
            return result == 1;
        }

        public async Task<bool> BanActiveAsync(int id)
        {
            string sql = "SELECT active FROM account_banned WHERE id = @id";
            using MySqlConnection connection = new MySqlConnection(Strings.AuthConnectionString);
            connection.Open();
            using MySqlCommand command = new MySqlCommand(sql, connection);
            var paramId = new MySqlParameter("id", MySqlDbType.Int32)
            {
                Value = id
            };
            command.Parameters.Add(paramId);
            var result = Convert.ToInt32(await command.ExecuteScalarAsync());
            connection.Close();
            return result == 1;
        }

        public async Task AddBanAsync(int id, int banDate, int unbanDate, string bannedBy, string banReason, bool active)
        {
            string sql = "INSERT INTO account_banned (id, bandate, unbandate, bannedby, banreason, active) VALUES (@id, @bandate, @unbandate, @bannedby, @banreason, @active)";
            using MySqlConnection connection = new MySqlConnection(Strings.AuthConnectionString);
            connection.Open();
            using MySqlCommand command = new MySqlCommand(sql, connection);
            var paramId = new MySqlParameter("id", MySqlDbType.Int32)
            {
                Value = id
            };
            var paramBanDate = new MySqlParameter("bandate", MySqlDbType.Int32)
            {
                Value = banDate
            };
            var paramUnbanDate = new MySqlParameter("unbandate", MySqlDbType.Int32)
            {
                Value = unbanDate
            };
            var paramBannedBy = new MySqlParameter("bannedby", MySqlDbType.VarChar)
            {
                Value = bannedBy
            };
            var paramBanReason = new MySqlParameter("banreason", MySqlDbType.VarChar)
            {
                Value = banReason
            };
            var paramActive = new MySqlParameter("active", MySqlDbType.Byte)
            {
                Value = active
            };
            command.Parameters.Add(paramId);
            command.Parameters.Add(paramBanDate);
            command.Parameters.Add(paramUnbanDate);
            command.Parameters.Add(paramBannedBy);
            command.Parameters.Add(paramBanReason);
            command.Parameters.Add(paramActive);
            await command.ExecuteNonQueryAsync();
            connection.Close();
        }

        public async Task RemoveBanAsync(int id)
        {
            string sql = "DELETE FROM account_banned WHERE id = @id";
            using MySqlConnection connection = new MySqlConnection(Strings.AuthConnectionString);
            connection.Open();
            using MySqlCommand command = new MySqlCommand(sql, connection);
            var paramId = new MySqlParameter("id", MySqlDbType.Int32)
            {
                Value = id
            };
            command.Parameters.Add(paramId);
            await command.ExecuteNonQueryAsync();
            connection.Close();
        }

        Task<string> ComputeSHA1PassHash(string username, string password)
        {
            var sha = new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes($"{username.ToUpper()}:{password.ToUpper()}"));
            return Task.FromResult(string.Concat(sha.Select(x => x.ToString("x2"))));
        }
    }
}
