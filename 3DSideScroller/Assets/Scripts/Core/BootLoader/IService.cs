public interface IService
{
    public void Initialize();

    public void Shutdown();

    public bool IsRunning { get; }

}
