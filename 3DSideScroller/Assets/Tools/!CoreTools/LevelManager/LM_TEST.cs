using System.Collections;
using System.Collections.Generic;
using LevelManagerLoader;
using UnityEngine;

public class LM_TEST : MonoBehaviour
{
    [ContextMenu("Test")]
    public void TT()
    {
        //LevelData.SetLevelData1(LevelGroupType.Levels, 4, LevelGameType.Main, "123/0/99");
        //LevelData.SetLevelData1(LevelGroupType.Levels, 4, LevelGameType.Thief, "123/455/uuf");
        LevelManagerData.SetLevelData(LevelGroupType.Classic, 4, LevelGameParam.Main, "1tt/tt5/uuf");
        //string[] s = LevelData.GetLevelData(LevelGroupType.Levels, 4, LevelGameParam.Main);
        //
        LevelManagerData.SetLevelComplete(LevelGroupType.Classic, 2, true);

        Debug.Log("Level " + LevelManagerData.GetLevelComplete(LevelGroupType.Classic, 1));
        Debug.Log("Level - " + LevelManagerData.GetFirstLevelUncompleted(LevelGroupType.Classic));
    }

}
