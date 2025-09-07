namespace MinimalAPIDemo.BackgroungServices;

public class MinuteJobService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine($"Background Job that run every minute executed at {DateTime.Now}");

            // Your logic here

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}