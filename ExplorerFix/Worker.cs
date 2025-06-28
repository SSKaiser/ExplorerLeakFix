using System.Diagnostics;

namespace ExplorerFix
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var processes = Process.GetProcessesByName("explorer");
                    foreach (Process process in processes)
                    {
                        if (process.WorkingSet64 > 500 * 1024 * 1024)
                        {
                            process.Kill();
                            await Task.Delay(500);
                            Process.Start("explorer");
                        }
                    }
                    await Task.Delay(3000, stoppingToken);
                } catch (Exception)
                {
                    continue;
                }
            }
        }
    }
}
