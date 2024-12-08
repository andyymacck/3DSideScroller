namespace LevelManagerLoader
{
   using UnityEngine;

   public class LevelManagerButtonsGroup : MonoBehaviour
   {
      [SerializeField] private LevelManagerButton[] m_levelManagerButtons;
      [SerializeField] private LevelGroupType m_levelGroupType;
      [SerializeField] private RectTransform m_scrollViewContent;

      private const float c_offsetPos = 610;
      private const int c_scrollCount = 2;
      
      public void InitButtons(LevelGroupType levelGroupType)
      {
         m_levelGroupType = levelGroupType;
         SetButtonData();
      }

      public void Hide()
      {
         gameObject.SetActive(false);
      }

      public void Show()
      {
         LevelGroup group = LevelManager.GetLevelGroup(m_levelGroupType);
         
         for (int i = 0; i < group.Levels.Count; i++)
         {
            m_levelManagerButtons[i].UpdateButtonData();
         }

         gameObject.SetActive(true);

         if (m_scrollViewContent) m_scrollViewContent.anchoredPosition = Vector2.up * GetPositionLastOpenButton();
      }

      private void OnEnable()
      {
         if (m_scrollViewContent) m_scrollViewContent.anchoredPosition = Vector2.up * GetPositionLastOpenButton();
      }

      private void SetButtonData()
      {
         LevelGroup group = LevelManager.GetLevelGroup(m_levelGroupType);
         
         for (int i = 0; i < m_levelManagerButtons.Length; i++)
         {
            if (i < group.Levels.Count)
            {
               int levelNum = i + 1;
               LevelManagerLevelParam levelParam = LevelManager.GetLevelManagerParam(m_levelGroupType, levelNum);
               m_levelManagerButtons[i].SetData(m_levelGroupType, levelNum, levelParam.SceneName, levelParam.LevelIcon);
               m_levelManagerButtons[i].Init();
            }
            else
            {
               m_levelManagerButtons[i].gameObject.SetActive(false);
            }
         }
      }

      private float GetPositionLastOpenButton()
      {
         float pos = 0f;
         int unlockedButtonsCount = 0;
         int column = 2;
         
         for (int i = 0; i < m_levelManagerButtons.Length; i++)
         {
            if (m_levelManagerButtons[i].ProgressIsFull)
            {
               unlockedButtonsCount++;
            }
         }

         if (unlockedButtonsCount > c_scrollCount)
         {
            pos = (int)((unlockedButtonsCount - c_scrollCount) / column) * c_offsetPos;
         }  

         return pos;
      }
   }
}