namespace Utils
{
    using log4net;

    public class Log4NetUtil
    {
        private static readonly ILog sysException = LogManager.GetLogger("applicationRuntimeLogError");

        private static readonly ILog executeInfo = LogManager.GetLogger("applicationExecuteInfo");

        /// <summary>
        /// 系统异常日志
        /// </summary>
        /// <param name="error"></param>
        public static void WriteErrorLog(string error)
        {
            if (sysException.IsErrorEnabled)
            {
                sysException.Error(error);
            }
        }

        /// <summary>
        /// 记录任务触发信息
        /// </summary>
        /// <param name="info"></param>
        public static void WriteExecuteInfo(string info)
        {
            if (executeInfo.IsErrorEnabled)
            {
                executeInfo.Info(info);
            }
        }
    }
}