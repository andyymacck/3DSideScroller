

using LevelManagerLoader;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SideScroller 
{
    public class FinishMenu : BaseMenu
    {
        [SerializeField] private Button m_nextButton;

        public override void Show()
        {
            m_nextButton.onClick.AddListener(LoadNextLevel);
            FinishLevel();

            base.Show();
        }

        public override void Close()
        {
            m_nextButton.onClick.RemoveAllListeners();

            base.Close();
        }

        private void FinishLevel()
        {
            int levelnumber = LevelManagerData.LastLevelNum;
            string levelGroupType = LevelManagerData.LastLevelGroup;

            LevelManagerData.SetLevelComplete(LevelGroupType.Classic, levelnumber, true);
        }

        private void LoadNextLevel() 
        {
            Time.timeScale = 1f;

            int nextLevelnumber = LevelManagerData.LastLevelNum + 1;
            string levelGroupType = LevelManagerData.LastLevelGroup;

            bool isNextLevelExist = LevelManager.IsLevelExist(LevelGroupType.Classic, nextLevelnumber);

            if (!isNextLevelExist)
            {
                // go to the map or swith to the next world
            }
            else
            {
                LevelManager.LoadLevelByNum(LevelGroupType.Classic, nextLevelnumber);
            }
        }
    }
}