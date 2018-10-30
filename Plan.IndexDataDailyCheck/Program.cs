using log4net.Config;
using System;
using System.IO;
using Topshelf;

namespace Plan.IndexDataDailyCheck
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // topshelf 启动配置
            XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Configs\\log4net.config"));
            HostFactory.Run(x =>
            {
                x.UseLog4Net();
                x.Service<ServiceRunner>(s =>
                {
                    s.ConstructUsing(name => new ServiceRunner());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                    s.WhenPaused(tc => tc.Pause());
                    s.WhenContinued(tc => tc.Continue());
                });
                x.RunAsLocalSystem();

                x.SetDescription("每日检查导入的数据是否已成功入库");
                x.SetDisplayName("Mysteel IndexData DailyCheck");
                x.SetServiceName("IndexDataDailyCheck");

                x.EnableShutdown();
                x.EnablePauseAndContinue();
            });
        }
    }
}