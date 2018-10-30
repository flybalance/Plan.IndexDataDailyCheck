using MySteel.Common.Helper;
using Oracle.DataAccess.Client;
using System;

namespace Utils
{
    public class OracleHelperUtil
    {
        private static readonly string ORACLECONN = AppSettingsHelper.GetStringValue("OracleConn").Replace("$", "\"");

        /// <summary>
        /// 获取单个值
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static object GetScalar(string sql)
        {
            object result;
            using (OracleConnection conn = new OracleConnection(ORACLECONN))
            {
                try
                {
                    conn.Open();
                    OracleCommand command = new OracleCommand(sql, conn);

                    result = command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    result = false;
                    Log4NetUtil.WriteErrorLog("错误参数：" + sql);
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