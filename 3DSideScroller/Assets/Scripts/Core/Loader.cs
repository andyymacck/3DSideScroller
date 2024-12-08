using LevelManagerLoader;
using UnityEngine;

public class Loader : MonoBehaviour
{

    void Start()
    {
        LevelManager.Init();
        LevelManager.LoadLevelByNum(LevelGroupType.Menu, 1);
        
        LevelManagerData.UnlockNextLevels(LevelGroupType.Classic, 2);
    }
}
