using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmellyEggPasswordManager.Controller
{
    public class BaseSqlController
    {
        private MySqlConnection _myConn;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseSqlController()
        {
            _myConn = new MySqlConnection(Config._connectStr);
        }

        /// <summary>
        /// 执行查询语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<DbDataReader> ExcuteQuery(string sql)
        {
            if (_myConn.State != System.Data.ConnectionState.Open)
            {
                await _myConn.OpenAsync();
            }
            var cmd = _myConn.CreateCommand();
            cmd.CommandText = sql;
            var reader = await cmd.ExecuteReaderAsync();
            //await _myConn.CloseAsync();
            return reader;
        }

        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<int> ExcuteNonQuery(string sql)
        {
            if (_myConn.State != System.Data.ConnectionState.Open)
            {
                await _myConn.OpenAsync();
            }
            using (var cmd = _myConn.CreateCommand())
            {
                cmd.CommandText = sql;
                var result = cmd.ExecuteNonQuery();
                return result;
            }
        }



    }
}
