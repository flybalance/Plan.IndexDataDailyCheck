using Plan.IndexDataDailyCheck.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace Plan.IndexDataDailyCheck.Dao
{
    public class DbDao
    {
        /// <summary>
        /// 获取指标信息
        /// </summary>
        /// <returns></returns>
        public static List<CoreIndex> GetIndexList()
        {
            string sql = @"SELECT ci.code AS IndexCode, ci.name AS IndexName, ci.index_source_type AS IndexSourceType,
                                cia.admin_name AS AdminName,cie.data_mark FROM core_index ci
                                LEFT JOIN core_index_extend cie ON ci.code = cie.index_code
                                LEFT JOIN core_index_admin cia ON ci.code = cia.index_code
                                WHERE ci.data_type = 0 AND (cie.market_status = 0 OR cie.market_code = '') AND cie.modify_status=1
                                AND (
                                locate ('|价格|',cie.data_mark) =0 
                                AND locate ('|调研|',cie.data_mark)=0 
                                AND locate ('|库存|',cie.data_mark)=0  
                                AND locate ('|钢厂调价|',cie.data_mark)=0 
                                AND locate ('|出厂价|',cie.data_mark)=0
                                AND (locate('上海期货交易所',cie.data_mark)=0 AND locate('空单持仓',cie.data_mark)=0)
                                AND (locate('上海期货交易所',cie.data_mark)=0 AND locate('多单持仓',cie.data_mark)=0)
                                AND (locate('郑州商品交易所',cie.data_mark)=0 AND locate('空单持仓',cie.data_mark)=0)
                                AND (locate('郑州商品交易所',cie.data_mark)=0 AND locate('多单持仓',cie.data_mark)=0)
                                AND (locate('大连商品交易所',cie.data_mark)=0 AND locate('空单持仓',cie.data_mark)=0)
                                AND (locate('大连商品交易所',cie.data_mark)=0 AND locate('多单持仓',cie.data_mark)=0)
                                AND locate('货币投放',cie.data_mark)=0
                                AND locate('货币回笼',cie.data_mark)=0
                                AND locate('公开市场操作',cie.data_mark)=0
                                AND locate('利率互换',cie.data_mark)=0
                                AND locate('|中国海关总署|',cie.data_mark)=0
                                )";

            DataTable dt = Utils.MySqlHelperUtil.GetDataTable(sql);

            List<CoreIndex> coreIndexList = null;
            if (null != dt && dt.Rows.Count >= 0)
            {
                coreIndexList = Utils.DataTableEntityConvertUtil.ConvertToList<CoreIndex>(dt);
            }

            return coreIndexList;
        }

        /// <summary>
        /// 获取指标数据路由表名称
        /// </summary>
        /// <param name="indexCode"></param>
        /// <returns></returns>
        public static string GetMappingTableName(string indexCode)
        {
            string sql = "select tablename from mapping_dataindex where indexcode='" + indexCode + "'";

            return (string)Utils.OracleHelperUtil.GetScalar(sql);
        }

        /// <summary>
        /// 根据日期判断是否有数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="indexCode"></param>
        /// <param name="dataDate"></param>
        /// <returns></returns>
        public static bool FindDataByDate(string tableName, string indexCode, string dataDate)
        {
            string sql = string.Format("select count(1) as countNum from {0} where indexcode='{1}' and datadate='{2}'", tableName, indexCode, dataDate);

            int result = 0;
            object obj = Utils.OracleHelperUtil.GetScalar(sql);
            if (null != obj)
            {
                result = Convert.ToInt32(obj);
            }

            return result > 0;
        }
    }
}