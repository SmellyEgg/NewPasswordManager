using MySql.Data.MySqlClient;
using SmellyEggPasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmellyEggPasswordManager.Controller
{
    /// <summary>
    /// 登陆管理类
    /// </summary>
    public class LoginController
    {
        public async Task<User> TryLogin(User user)
        {
            try
            {
                using (var conn = new MySqlConnection(Config._connectStr))
                {
                    await conn.OpenAsync();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select PasswordKey from MyUser where UserName = @name and UserPassword = @password";
                        var password = SmellyEggCrypt.CryPtService.DESEncrypt(user.UserPassword, Config.decryKey);
                        cmd.Parameters.AddWithValue("@name", user.UserName);
                        cmd.Parameters.AddWithValue("@password", password);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                if (reader != null && reader.FieldCount > 0)
                                {
                                    user.PasswordKey = reader[0].ToString();
                                    return user;
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                string text = ex.Message;
            }
            return null;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> TryRegister(User user)
        {
            try
            {
                using (var conn = new MySqlConnection(Config._connectStr))
                {
                    await conn.OpenAsync();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO `MyUser`(`UserName`, `UserPassword`, `PasswordKey`) VALUES (@name,@password,@key)";
                        var password = SmellyEggCrypt.CryPtService.DESEncrypt(user.UserPassword, Config.decryKey);
                        var key = SmellyEggCrypt.CryPtService.DESEncrypt(user.PasswordKey, Config.decryKey);
                        cmd.Parameters.AddWithValue("@name", user.UserName);
                        cmd.Parameters.AddWithValue("@password", password);
                        cmd.Parameters.AddWithValue("@key", key);
                        var result = await cmd.ExecuteNonQueryAsync();
                        if (result == 1) return true;
                    }
                }

            }
            catch 
            {
            }
            return false;
        }
    }
}
