using LevelManagerLoader;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Loader : MonoBehaviour
{
    [SerializeField] private GameObject m_canvas;

    private List<IService> services = new List<IService>();

    private async void Start()
    {
        ServicePlatform platform = GetCurrentPlaform();

        IInternetService internetService = GetInternetService(platform);
        await StartService(internetService);

        Debug.Log($"IsConnected {internetService.IsConnected}");
        if(internetService.IsConnected)
        {
            
        }

        ISavesService savesService = new PlayerPrefsSavesService();
        await StartService(savesService);
     
        PlayfabAccountService playfabAccount = new PlayfabAccountService(internetService, savesService);
        IAccountService accountService = playfabAccount;
        ISavesService accountSaveService = playfabAccount;
        await StartService(playfabAccount);

        LevelManager.Init();
        LevelManager.LoadLevelByNum(LevelGroupType.Menu, 1);
        LevelManagerData.UnlockNextLevels(LevelGroupType.Classic, 2);
    }

    private async Task StartService(IService service)
    {
        await service.Initialize();
        services.Add(service);
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
    //fix function to get created new internet service instead of basic internet service
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