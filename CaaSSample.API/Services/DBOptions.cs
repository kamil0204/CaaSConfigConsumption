using Microsoft.Extensions.Configuration;

namespace CaaSSample.API.Services;

public class DBOptions
{
    private string? _ifsdb;

    public string IFSDB
    {
        get => Environment.ExpandEnvironmentVariables(_ifsdb);
        set => _ifsdb = value;
    }
}
