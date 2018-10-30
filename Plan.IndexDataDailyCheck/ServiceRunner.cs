namespace Plan.IndexDataDailyCheck
{
    using Quartz;
    using Quartz.Impl;
    using Quartz.Simpl;
    using Quartz.Xml;

    public class ServiceRunner
    {
        private IScheduler scheduler;

        public ServiceRunner()
        {
            // 从配置中读取计划执行策略
            XMLSchedulingDataProcessor processor = new XMLSchedulingDataProcessor(new SimpleTypeLoadHelper());
            ISchedulerFactory sf = new StdSchedulerFactory();
            scheduler = sf.GetScheduler();
            // quartz_jobs.xml文件路径
            processor.ProcessFileAndScheduleJobs("~/Configs/quartz_jobs.xml", scheduler);
        }

        /// <summary>
        /// 启动quartz服务
        /// </summary>
        ///
        public void Start()
        {
            scheduler.Start();
        }

        /// <summary>
        /// true:等待正在运行的计划任务执行完毕;
        /// false:强制关闭
        /// </summary>
        public void Stop()
        {
            scheduler.Shutdown(false);
        }

        /// <summary>
        /// 继续服务
        /// </summary>
        /// <returns></returns>
        public bool Continue()
        {
            scheduler.ResumeAll();
            return true;
        }

        /// <summary>
        /// 暂停服务
        /// </summary>
        /// <returns></returns>
        public bool Pause()
        {
            scheduler.PauseAll();
            return true;
        }
    }
}