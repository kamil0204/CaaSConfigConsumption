namespace CaaSSample.API.Worker;

public class ConfigWatcherWorker : BackgroundService
{
    private readonly ILogger<ConfigWatcherWorker> _logger;
    private readonly IConfiguration _config;

    public ConfigWatcherWorker(ILogger<ConfigWatcherWorker> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var configMapDirectory = _config["ConfigMapMount"]!;
        var secretMapDirectory = _config["SecretMapMount"]!;
        _logger.LogInformation("ConfigWatcherWorker is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            CheckForNewAndUpdatedConfigs(configMapDirectory);
            CheckForNewAndUpdatedConfigs(secretMapDirectory);
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); 
        }

        _logger.LogInformation("ConfigWatcherWorker is stopping.");
    }

    private void CheckForNewAndUpdatedConfigs(string directory)
    {        
        if (Directory.Exists(directory))
        {
            var files = Directory.GetFiles(directory);
            foreach (var file in files)
            {
                string key = Path.GetFileName(file);
                string value = File.ReadAllText(file);
                Environment.SetEnvironmentVariable(key, value);
                File.Delete(file);
                _logger.LogInformation($"{file} was created or updated at {DateTime.UtcNow}");
            }

        } else
        {
            _logger.LogInformation($"Directory - {directory} was not found");
        }   
    }
}
