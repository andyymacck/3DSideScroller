using LevelManagerLoader;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    [SerializeField] private GameObject m_canvas;

    private List<IService> services = new List<IService>();

    private enum ServicePlatform
    { 
        None, 
        Steam, 
        Epic,
        GOG
    };

    void Start()
    {
        ServicePlatform platform = GetCurrentPlaform();

        IInternetService internetService = GetInternetService(platform);
        internetService.Initialize();
        services.Add(internetService);
        
        if(internetService.IsConnected)
        {
            
        }

        ISavesService savesService = new PlayerPrefsSavesService();
        savesService.Initialize();

        PlayfabAccountService playfabAccount = new PlayfabAccountService(internetService, savesService);
        IAccountService accountService = playfabAccount;
        ISavesService accountSaveService = playfabAccount;
        accountService.SetInternetService(internetService);
        services.Add(accountService);

        playfabAccount.SaveValue("name", null);
        savesService.SaveValue("name", null);
        accountSaveService.SaveValue("name", null);


        LevelManager.Init();
        LevelManager.LoadLevelByNum(LevelGroupType.Menu, 1);
        
        LevelManagerData.UnlockNextLevels(LevelGroupType.Classic, 2);
    }

    private ServicePlatform GetCurrentPlaform()
    {
#if PLATFORM_STEAM
        return ServicePlatform.Steam;
#endif

#if PLATFORM_EPIC
        return ServicePlatform.Epic;
#endif

#if PLATFORM_GOG
        return ServicePlatform.GOG;
#endif
        return ServicePlatform.None;
    }

    private IInternetService GetInternetService(ServicePlatform servicePlatform)
    {
        if (servicePlatform == ServicePlatform.Steam)
        {
            return new SteamInternetService();
        }

        if (servicePlatform == ServicePlatform.Epic)
        {
            return new EpicGamesInternetService();
        }

        return new BasicInternetService();
    }

    private void ShutDown()
    {
        foreach (var service in services)
        {
            service.Shutdown();
        }
    }
}