using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayfabAccountService : IAccountService, ISavesService
{
    private ServiceState m_serviceState;
    public ServiceState ServiceState => m_serviceState;
    public bool IsRunning => throw new System.NotImplementedException();

    private IInternetService m_internetService;
    private ISavesService m_savesService;

    public PlayfabAccountService(IInternetService internetService, ISavesService savesService)
    {
        m_internetService = internetService;
        m_savesService = savesService;
    }

    public string GetPlayerData()
    {
        throw new System.NotImplementedException();
    }

    public string GetPlayerId()
    {
        throw new System.NotImplementedException();
    }

    public async Task Initialize()
    {
        await Task.Delay(2000);
    }

    public void SetInternetService(IInternetService internetService)
    {
        m_internetService = internetService;
    }

    public void Shutdown()
    {
        throw new System.NotImplementedException();
    }

    public void SaveValue(string name, object value) 
    {
    }

    public object GetValue(string name)
    {
        return null; 
    }

    public bool IsAllSaved { get; }
}
