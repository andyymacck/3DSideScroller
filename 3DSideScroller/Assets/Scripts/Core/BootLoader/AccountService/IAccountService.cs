
public interface IAccountService : IService
{
    public void SetInternetService(IInternetService internetService);

    public string GetPlayerId();

    public string GetPlayerData();
}
