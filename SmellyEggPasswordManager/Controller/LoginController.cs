using SmellyEggPasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmellyEggPasswordManager.Controller
{
    /// <summary>
    /// 登陆管理类
    /// </summary>
    public class LoginController : BaseSqlController
    {

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User> TryLogin(User user)
        {
            System.Data.Common.DbDataReader reader;
            try
            {
                //using (var conn = new MySqlConnection(Config._connectStr))
                //{
                //    await conn.OpenAsync();
                //    using (var cmd = conn.CreateCommand())
                //    {
                //        cmd.CommandText = "select PasswordKey from MyUser where UserName = @name and UserPassword = @password";
                //        var password = SmellyEggCrypt.CryPtService.DESEncrypt(user.UserPassword, Config.decryKey);
                //        cmd.Parameters.AddWithValue("@name", user.UserName);
                //        cmd.Parameters.AddWithValue("@password", password);
                //        using (var reader = await cmd.ExecuteReaderAsync())
                //        {
                //            while (await reader.ReadAsync())
                //            {
                //                if (reader != null && reader.FieldCount > 0)
                //                {
                //                    user.PasswordKey = SmellyEggCrypt.CryPtService.DESDecrypt(reader[0].ToString(), Config.decryKey);
                //                    return user;
                //                }
                //            }
                //        }
                //    }
                //}
                string sql = "select PasswordKey from MyUser where UserName = '{0}' and UserPassword = '{1}'";
                var password = SmellyEggCrypt.CryPtService.DESEncrypt(user.UserPassword, Config.decryKey);
                sql = string.Format(sql, user.UserName, password);
                reader = await ExcuteQuery(sql);
                while (await reader.ReadAsync())
                {
                    if (reader != null && reader.FieldCount > 0)
                    {
                        user.PasswordKey = SmellyEggCrypt.CryPtService.DESDecrypt(reader[0].ToString(), Config.decryKey);
                        reader.Close();
                        reader = null;
                        return user;
                    }
                }
                
            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }
            finally
            {
                CloseConnection();
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
                //using (var conn = new MySqlConnection(Config._connectStr))
                //{
                //    await conn.OpenAsync();
                //    using (var cmd = conn.CreateCommand())
                //    {
                //        cmd.CommandText = "INSERT INTO `MyUser`(`UserName`, `UserPassword`, `PasswordKey`) VALUES (@name,@password,@key)";
                //        var password = SmellyEggCrypt.CryPtService.DESEncrypt(user.UserPassword, Config.decryKey);
                //        var key = SmellyEggCrypt.CryPtService.DESEncrypt(user.PasswordKey, Config.decryKey);
                //        cmd.Parameters.AddWithValue("@name", user.UserName);
                //        cmd.Parameters.AddWithValue("@password", password);
                //        cmd.Parameters.AddWithValue("@key", key);
                //        var result = await cmd.ExecuteNonQueryAsync();
                //        if (result == 1) return true;
                //    }
                //}
                string sql = "INSERT INTO `MyUser`(`UserName`, `UserPassword`, `PasswordKey`) VALUES ('{0}','{1}','{2}')";
                var password = SmellyEggCrypt.CryPtService.DESEncrypt(user.UserPassword, Config.decryKey);
                var key = SmellyEggCrypt.CryPtService.DESEncrypt(user.PasswordKey, Config.decryKey);
                sql = string.Format(sql, user.UserName, password, key);
                var result = await ExcuteNonQuery(sql);
                if (result == 1) return true;
            }
            catch
            {
            }
            return false;
        }

        /// <summary>
        /// 获取账户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<List<Account>> GetAccounts(User user)
        {
            try
            {
                //using (var conn = new MySqlConnection(Config._connectStr))
                //{
                //    await conn.OpenAsync();
                //    using (var cmd = conn.CreateCommand())
                //    {
                //        cmd.CommandText = "SELECT AccountName, AccountPassword, AccountType FROM `MyAccount` WHERE UserName = @name";
                //        cmd.Parameters.AddWithValue("@name", user.UserName);
                //        using (var reader = await cmd.ExecuteReaderAsync())
                //        {
                //            List<Account> listAccount = new List<Account>();
                //            while (await reader.ReadAsync())
                //            {
                //                if (reader != null && reader.FieldCount > 0)
                //                {
                //                    var accountPassword = SmellyEggCrypt.CryPtService.DESDecrypt(reader[1].ToString(), user.PasswordKey);
                //                    Account account = new Account()
                //                    {
                //                        AccountName = reader[0].ToString(),
                //                        AccountPassword = accountPassword,
                //                        AccountType = reader[2].ToString()
                //                    };
                //                    listAccount.Add(account);
                //                }
                //            }
                //            return listAccount;
                //        }
                //    }
                //}
                string sql = "SELECT AccountName, AccountPassword, AccountType FROM `MyAccount` WHERE UserName = '{0}'";
                sql = string.Format(sql, user.UserName);
                var reader = await ExcuteQuery(sql);
                List<Account> listAccount = new List<Account>();
                while (await reader.ReadAsync())
                {
                    if (reader != null && reader.FieldCount > 0)
                    {
                        var accountPassword = SmellyEggCrypt.CryPtService.DESDecrypt(reader[1].ToString(), user.PasswordKey);
                        Account account = new Account()
                        {
                            AccountName = reader[0].ToString(),
                            AccountPassword = accountPassword,
                            AccountType = reader[2].ToString()
                        };
                        listAccount.Add(account);
                    }
                }
                reader.Close();
                reader = null;
                return listAccount;
            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AddAccount(Account account, User user)
        {
            try
            {
                //using (var conn = new MySqlConnection(Config._connectStr))
                //{
                //    await conn.OpenAsync();
                //    using (var cmd = conn.CreateCommand())
                //    {
                //        cmd.CommandText = "INSERT INTO `MyAccount` (`UserName`, `AccountName`, `AccountPassword`, `AccountType`) VALUES (@username, @accountname, " +
                //            "@accountpassword, @accounttype)";
                //        var password = SmellyEggCrypt.CryPtService.DESEncrypt(account.AccountPassword, user.PasswordKey);
                //        cmd.Parameters.AddWithValue("@username", user.UserName);
                //        cmd.Parameters.AddWithValue("@accountname", account.AccountName);
                //        cmd.Parameters.AddWithValue("@accountpassword", password);
                //        cmd.Parameters.AddWithValue("@accounttype", account.AccountType);
                //        var result = await cmd.ExecuteNonQueryAsync();
                //        if (result == 1) return true;
                //    }
                //}
                string sql = "INSERT INTO `MyAccount` (`UserName`, `AccountName`, `AccountPassword`, `AccountType`) VALUES ('{0}', '{1}', " +
                            "'{2}', '{3}')";
                var password = SmellyEggCrypt.CryPtService.DESEncrypt(account.AccountPassword, user.PasswordKey);
                sql = string.Format(sql, user.UserName, account.AccountName, password, account.AccountType);
                var result = await ExcuteNonQuery(sql);
                if (result == 1) return true;
            }
            catch
            {
            }
            return false;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAccount(Account account)
        {
            try
            {
                //using (var conn = new MySqlConnection(Config._connectStr))
                //{
                //    await conn.OpenAsync();
                //    using (var cmd = conn.CreateCommand())
                //    {
                //        cmd.CommandText = "delete from `MyAccount` where AccountName = @accountName";
                //        cmd.Parameters.AddWithValue("@accountName", account.AccountName);
                //        var result = await cmd.ExecuteNonQueryAsync();
                //        if (result == 1) return true;
                //    }
                //}
                string sql = "delete from `MyAccount` where AccountName = '{0}'";
                sql = string.Format(sql, account.AccountName);
                var result = await ExcuteNonQuery(sql);
                if (result == 1) return true;
            }
            catch (Exception ex)
            {
                string exx = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// 更新账户
        /// </summary>
        /// <param name="account"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAccount(Account account, Account oldAccount, User user)
        {
            try
            {
                //using (var conn = new MySqlConnection(Config._connectStr))
                //{
                //    await conn.OpenAsync();
                //    using (var cmd = conn.CreateCommand())
                //    {
                //        cmd.CommandText = "update `MyAccount` set AccountName = @accountName, AccountPassword = @accountpassword, " +
                //            "AccountType = @accounttype where AccountName = @oldaccountName";
                //        cmd.Parameters.AddWithValue("@accountName", account.AccountName);
                //        var password = SmellyEggCrypt.CryPtService.DESEncrypt(account.AccountPassword, user.PasswordKey);
                //        cmd.Parameters.AddWithValue("@accountpassword", password);
                //        cmd.Parameters.AddWithValue("@accounttype", account.AccountType);
                //        cmd.Parameters.AddWithValue("@oldaccountName", oldAccount.AccountName);
                //        var result = await cmd.ExecuteNonQueryAsync();
                //        if (result == 1) return true;
                //    }
                //}
                string sql = "update `MyAccount` set AccountName = '{0}', AccountPassword = '{1}', " +
                            "AccountType = '{2}' where AccountName = '{3}'";
                var password = SmellyEggCrypt.CryPtService.DESEncrypt(account.AccountPassword, user.PasswordKey);
                sql = string.Format(sql, account.AccountName, password, account.AccountType, oldAccount.AccountName);
                var result = await ExcuteNonQuery(sql);
                if (result == 1) return true;
            }
            catch
            {
            }
            return false;
        }

    }
}
