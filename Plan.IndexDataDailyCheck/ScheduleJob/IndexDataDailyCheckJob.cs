namespace Plan.IndexDataDailyCheck.ScheduleJob
{
    using Quartz;

    /// <summary>
    /// 每日定时检查导入的数据是否入库定时任务
    /// </summary>
    public class IndexDataDailyCheckJob : IJob
    {
        /// <summary>
        /// 实现Ijob接口
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            Utils.Log4NetUtil.WriteExecuteInfo("任务触发");
            Task.StartWork();
            Utils.Log4NetUtil.WriteExecuteInfo("任务执行结束");
        }
    }
}