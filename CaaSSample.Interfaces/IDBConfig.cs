namespace CaaSSample.Interfaces;

public interface IDBConfig
{
    public string ConnectionString
    {
        get;
    }

    public int Timeout
    {
        get;
    }
}
