using Microsoft.Extensions.Configuration;

IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

var configSourceFolder = configuration["ConfigSourceMount"]!;
var configDestinationFolder = configuration["ConfigDestinationMount"]!;
var secretSourceFolder = configuration["SecretSourceMount"]!;
var secretDestinationFolder = configuration["SecretDestinationMount"]!;
var configCollection = new Dictionary<string, string>();
var secretCollection = new Dictionary<string, string>();

try
{
    await UpsertConfigs(configSourceFolder, configDestinationFolder, configCollection);
    await UpsertConfigs(secretSourceFolder, secretDestinationFolder, secretCollection);

    // Run the file creation logic every minute.
    using (var timer = new PeriodicTimer(TimeSpan.FromMinutes(2)))
    {
        while (await timer.WaitForNextTickAsync())
        {
            await UpsertConfigs(configSourceFolder, configDestinationFolder, configCollection);
            await UpsertConfigs(secretSourceFolder, secretDestinationFolder, secretCollection);
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
finally
{
    Console.WriteLine("Exiting program.");
}

static async Task UpsertConfigs(string sourceFolder, string destinationFolder, Dictionary<string, string> configCollection)
{
    if (Directory.Exists(sourceFolder))
    {
        Directory.SetCurrentDirectory(sourceFolder);
        foreach (var file in Directory.GetFiles(sourceFolder))
        {
            var key = Path.GetFileName(file);
            var value = File.ReadAllText(file);
            if (configCollection.ContainsKey(key))
            {
                if (configCollection[key] != value)
                {
                    Console.WriteLine($"Value for {key} updated");
                    configCollection[key] = value;
                }
                else
                {
                    configCollection.Remove(key);
                }
            }
            else
            {
                Console.WriteLine($"Value for {key} added");
                configCollection.Add(key, value);
            }
        }
    } else
    {
        Console.WriteLine($"Source Directory {sourceFolder} not found");
    }

    if (Directory.Exists(destinationFolder))
    {
        Directory.SetCurrentDirectory(destinationFolder);
        foreach (var item in configCollection)
        {
            await Task.Run(() => File.WriteAllText(item.Key, item.Value));
            Console.WriteLine($"Key file for {item.Key} copied to {destinationFolder}");
        }
    }
    else
    {
        Console.WriteLine($"Destination directory {destinationFolder} not found");
    }
}


