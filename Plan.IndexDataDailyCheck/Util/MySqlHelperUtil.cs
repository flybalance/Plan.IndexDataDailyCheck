namespace Utils
{
    using MySql.Data.MySqlClient;
    using MySteel.Common.Helper;
    using Newtonsoft.Json;
    using System;
    using System.Data;

    /// <summary>
    /// MySql db操作类
    /// </summary>
    public static class MySqlHelperUtil
    {
        /// <summary>
        /// db链接字符串
        /// </summary>
        private static readonly string MYSQLCONN = AppSettingsHelper.GetStringValue("MySqlConn").Replace("$", "\"");

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql, params MySqlParameter[] param)
        {
            DataTable result = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(MYSQLCONN))
            {
                try
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(sql, conn);

                    if (null != param && param.Length > 0)
                    {
                        command.Parameters.AddRange(param);
                    }

                    //MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    MySqlDataReader reader = command.ExecuteReader();
                    result.Load(reader);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    result = null;
                    Log4NetUtil.WriteErrorLog("错误参数：" + sql + "\n" + JsonConvert.SerializeObject(param) + "\n" + ex.ToString());
                }
                finally
                {
                    conn.Close();
                }

                return result;
            }
        }

        /// <summary>
        /// 执行语句 ExecuteSql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool ExecuteSql(string sql, params MySqlParameter[] param)
        {
            bool result = false;
            using (MySqlConnection conn = new MySqlConnection(MYSQLCONN))
            {
                try
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(sql, conn);

                    if (null != param && param.Length > 0)
                    {
                        command.Parameters.AddRange(param);
                    }

                    result = command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    result = false;
                    Log4NetUtil.WriteErrorLog("错误参数：" + sql + "\n" + JsonConvert.SerializeObject(param) + "\n" + ex.ToString());
                }
                finally
                {
                    conn.Close();
                }

                return result;
            }
        }

        /// <summary>
        /// 获取单个值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static object GetScalar(string sql, params MySqlParameter[] param)
        {
            object result;
            using (MySqlConnection conn = new MySqlConnection(MYSQLCONN))
            {
                try
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(sql, conn);

                    if (null != param && param.Length > 0)
                    {
                        command.Parameters.AddRange(param);
                    }

                    result = command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    result = false;
                    Log4NetUtil.WriteErrorLog("错误参数：" + sql + "\n" + JsonConvert.SerializeObject(param) + "\n" + ex.ToString());
                }
                finally
                {
                    conn.Close();
                }

                return result;
            }
        }
    }
}