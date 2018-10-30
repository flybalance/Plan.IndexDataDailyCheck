using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Plan.IndexDataDailyCheck.Dao;
using Plan.IndexDataDailyCheck.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Plan.IndexDataDailyCheck
{
    public class Task
    {
        private static readonly string CURR_DATE = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

        /// <summary>
        /// 执行检查任务
        /// </summary>
        public static void StartWork()
        {
            try
            {
                // 获取要检查的指标
                List<CoreIndex> coreIndexList = DbDao.GetIndexList();
                if (null == coreIndexList || coreIndexList.Count == 0)
                {
                    return;
                }

                // 当检查到前日所有的数据都已入库，则不会发送邮件
                List<CoreIndex> exportCoreIndexList = GetExportCoreIndexList(coreIndexList);
                if (null == exportCoreIndexList || exportCoreIndexList.Count == 0)
                {
                    return;
                }

                // 取生成后的文件路径
                string excelFilePath = GenerateExcelFile(exportCoreIndexList);
                if (string.IsNullOrEmpty(excelFilePath))
                {
                    return;
                }

                // 获取发送邮件的配置信息
                string emailSettingFilePaht = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs\\Receiver.xml");
                EmailConfiguration emailConfiguration = MySteel.Common.Helper.XmlHelper.LoadXml<EmailConfiguration>(emailSettingFilePaht);
                if (null == emailConfiguration)
                {
                    return;
                }

                string emailSubject = CURR_DATE + "指标数据未入库明细";
                string emailContent = "附件为指标数据未入库明细，请查收。";
                string receivers = string.Join(",", emailConfiguration.Receivers.Select(t => t.EmailAddr));

                Sender sender = emailConfiguration.Sender;
                Utils.EmailUtil.SendMail(sender.Host, sender.UserName, sender.Password, receivers, emailSubject, emailContent, excelFilePath);
            }
            catch (Exception ex)
            {
                Utils.Log4NetUtil.WriteErrorLog(ex.ToString());
            }
        }

        /// <summary>
        /// 获取当日无数据的指标信息
        /// </summary>
        /// <param name="originCoreIndexList"></param>
        /// <returns></returns>
        private static List<CoreIndex> GetExportCoreIndexList(List<CoreIndex> originCoreIndexList)
        {
            List<CoreIndex> exportCoreIndexList = new List<CoreIndex>();

            string tableName = string.Empty;
            foreach (CoreIndex coreIndex in originCoreIndexList)
            {
                tableName = DbDao.GetMappingTableName(coreIndex.IndexCode);
                if (string.IsNullOrEmpty(tableName))
                {
                    continue;
                }

                bool result = DbDao.FindDataByDate(tableName, coreIndex.IndexCode, CURR_DATE);
                if (!result)
                {
                    exportCoreIndexList.Add(coreIndex);
                }
            }

            return exportCoreIndexList;
        }

        /// <summary>
        /// 生成excel文件
        /// </summary>
        /// <param name="exportCoreIndexList"></param>
        /// <returns></returns>
        private static string GenerateExcelFile(List<CoreIndex> exportCoreIndexList)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\DailyCheckFile";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            filePath = Path.Combine(filePath, "指标无数据明细" + CURR_DATE + ".xlsx");

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("指标信息");
                IRow row = sheet.CreateRow(0);
                CreateSheetHead(row);

                IRow currentRow = null;
                int currentIndex = 0;
                int totalNum = exportCoreIndexList.Count;
                foreach (CoreIndex coreIndex in exportCoreIndexList)
                {
                    currentIndex++;
                    currentRow = sheet.CreateRow(currentIndex);
                    currentRow.CreateCell(0).SetCellValue(coreIndex.IndexCode);
                    currentRow.CreateCell(1).SetCellValue(coreIndex.IndexName);
                    currentRow.CreateCell(2).SetCellValue(GetIndexSourceType(coreIndex.IndexSourceType));
                    currentRow.CreateCell(3).SetCellValue(coreIndex.AdminName);
                }
                workbook.Write(fs);
            }

            return filePath;
        }

        /// <summary>
        /// 创建excel标题行
        /// </summary>
        /// <param name="row"></param>
        private static void CreateSheetHead(IRow row)
        {
            row.CreateCell(0).SetCellValue("指标编码");
            row.CreateCell(1).SetCellValue("指标名称");
            row.CreateCell(2).SetCellValue("指标来源");
            row.CreateCell(3).SetCellValue("维护人");
        }

        /// <summary>
        /// 获取指标类型
        /// </summary>
        /// <param name="indexSourceType"></param>
        /// <returns></returns>
        private static string GetIndexSourceType(int indexSourceType)
        {
            string typeStr = string.Empty;
            switch (indexSourceType)
            {
                case 0:
                    typeStr = "钢联指标";
                    break;

                case 1:
                    typeStr = "隆众网指标";
                    break;

                case 2:
                    typeStr = "其他外部指标";
                    break;

                default:
                    typeStr = "钢联指标";
                    break;
            }

            return typeStr;
        }
    }
}