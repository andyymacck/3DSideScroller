using LevelManagerLoader;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    [SerializeField] private GameObject m_canvas;

    [SerializeField] private Image m_loadbarImage;

    private int m_loadingStep = 0;
    private int m_loadStepsAmout = 3;

    private async void Start()
    {
        ServiceProvider.Initialize();

        m_loadbarImage.fillAmount = 0f;

        ServicePlatform platform = GetCurrentPlaform();

        IInternetService internetService = new InternetService();
        await StartService(internetService);

        Debug.Log($"IsConnected {internetService.IsConnected}");
        //TODO: HOME TASK - Fix an issue with the InternetService that always returns a false;

        if(internetService.IsConnected)
        {
            
        }

        ISavesService savesService = new PlayerPrefsSavesService(); //Dummy
        await StartService(savesService);
     
        PlayfabAccountService playfabAccount = new PlayfabAccountService(internetService, savesService); //Dummy
        IAccountService accountService = playfabAccount;
        ISavesService accountSaveService = playfabAccount;
        await StartService(playfabAccount);

        LevelManager.Init();
        LevelManager.LoadLevelByNum(LevelGroupType.Menu, 1);
        LevelManagerData.UnlockNextLevels(LevelGroupType.Classic, 2);
    }

    private async Task StartService<T>(T service) where T : IService
    {
        await service.Initialize();
        ServiceProvider.Register(service);
        IncrementLoading();
    }

    private void IncrementLoading()
    {
        m_loadingStep++;
        float fillAmout = (float)m_loadingStep / (float)m_loadStepsAmout;
        m_loadbarImage.fillAmount = fillAmout;
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

    private void ShutDown()
    {
        IInternetService internetService = ServiceProvider.GetService<IInternetService>();
        internetService.Shutdown();
    }
}