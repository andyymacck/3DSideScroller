using System.Threading.Tasks;

public enum ServiceState
{
    None,
    Created,
    Started,
    Running,
    Failed,
    Down
}

public interface IService
{
    public Task Initialize();
    public void Shutdown();
    public bool IsRunning { get; }
    public ServiceState ServiceState { get; }
}