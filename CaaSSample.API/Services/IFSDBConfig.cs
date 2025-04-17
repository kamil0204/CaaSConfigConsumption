using CaaSSample.Interfaces;

namespace CaaSSample.API.Services;

public class IFSDBConfig : IDBConfig
{
    private readonly IConfiguration _configuration;

    public IFSDBConfig(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string ConnectionString => Environment.ExpandEnvironmentVariables(_configuration.GetConnectionString("IFSDB")!);

    public int Timeout => _configuration.GetValue<int>("ConnectionTimeouts:IFS");
}
