using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfabAccountService : IAccountService, ISavesService
{
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

    public void Initialize()
    {
        if (!m_internetService.IsConnected)
        {
            Debug.Log("Show connection error");
            return;
        }
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
