namespace LevelManagerLoader
{
    using MgsTools;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    
    public class LevelManagerButton : MonoBehaviour
    {
        [Header("Enter manually")]
        [SerializeField] private int m_levelNum;
        [SerializeField] private LevelGroupType m_levelGroupType;
        [Space]
        [SerializeField] private Text m_txtLevelName;
        [SerializeField] private TMP_Text m_txtProgress;
        [SerializeField] private Image m_imgFrame;
        [SerializeField] private Image m_imgProgressBar;
        [SerializeField] private Image m_imgLevelIcon;
        [SerializeField] private GameObject m_objPlay;
        [SerializeField] private GameObject m_objLock;
        [SerializeField] private GameObject m_objProgressBar;
        [SerializeField] private Button m_btnLevel;
        [SerializeField] private Color m_colorCompleted;
        [SerializeField] private Color m_colorOpened;
        [SerializeField] private Color m_colorLocked;
        [Space]
        [SerializeField] private GameObject m_objCoinPanel;
        [SerializeField] private TMP_Text m_txtCoinAmaunt;
        [SerializeField] private GameObject m_objCrystalPanel;
        [SerializeField] private TMP_Text txtCrystalAmaunt;

        private bool m_isUnlocked;
        private bool m_progressIsFull;
        private float m_lastTimePressed;
        
        private const float c_buttonPressTimeDiff = 2f;
        
        public bool ProgressIsFull => m_progressIsFull;
        
        public void Init()
        {
            m_btnLevel.onClick.RemoveAllListeners();
            m_btnLevel.onClick.AddListener(OnButtonClick);

            UpdateButtonData();
        }

        public void UpdateButtonData()
        {
            m_isUnlocked = LevelManagerData.GetLevelUnlocked(m_levelGroupType, m_levelNum);
            bool isCurrent = LevelManagerData.GetFirstLevelUncompleted(m_levelGroupType) == m_levelNum;
            bool isCompleted = LevelManagerData.GetLevelComplete(m_levelGroupType, m_levelNum);
            LevelManagerLevelParam levelParam = LevelManager.GetLevelManagerParam(m_levelGroupType, m_levelNum);

            if (levelParam == null)
            {
                return;
            }
            
            LevelManagerData.GetLevelDataMain(m_levelGroupType, m_levelNum, out int totalItems, out int collectedItems);
            LevelManagerData.GetLevelDataCoin(m_levelGroupType, m_levelNum, out int totalCoins, out int collectedCoins);
            LevelManagerData.GetLevelDataGold(m_levelGroupType, m_levelNum, out int totalGolds, out int collectedGolds);
            
            m_objCoinPanel.gameObject.SetActive(totalCoins > 0);
            m_txtCoinAmaunt.text = $"{collectedCoins} / {totalCoins}";
            m_objCrystalPanel.gameObject.SetActive(totalGolds > 0);
            txtCrystalAmaunt.text = $"{collectedGolds} / {totalGolds}";
            
            float progress = totalItems == 0 ? 0 : (float)collectedItems / (float)totalItems;
            m_progressIsFull = progress == 1;
            m_txtProgress.text =  m_progressIsFull ? "Completed" : $"{(progress * 100f).ToString("f0")}%"; 
            m_imgProgressBar.fillAmount = progress;

            m_txtLevelName.text = levelParam.SceneName;
            m_imgLevelIcon.sprite = levelParam.LevelIcon;
        
            m_objLock.SetActive(!m_isUnlocked);
            m_objPlay.SetActive(m_isUnlocked && !m_progressIsFull);
            m_objProgressBar.SetActive(m_isUnlocked || m_progressIsFull);
            m_btnLevel.interactable = m_isUnlocked;
            m_imgFrame.color = isCompleted ? m_colorCompleted : m_isUnlocked ? m_colorOpened : m_colorLocked;
        }

        public void SetData(LevelGroupType levelGroupType, int levelNum, string text, Sprite sprite)
        {
            m_levelGroupType = levelGroupType;
            m_levelNum = levelNum;
            m_txtLevelName.text = text;
            m_imgLevelIcon.sprite = sprite;
        }

        private void OnButtonClick()
        {
            if(!m_isUnlocked) return;
            if (Time.time - c_buttonPressTimeDiff < m_lastTimePressed) return;

            m_lastTimePressed = Time.time;

            OperationsRuntime.RunWithDelay(LoadLevel, 1.0f);
        }

        private void LoadLevel()
        {
            LevelManager.LoadLevelByNum(m_levelGroupType, m_levelNum);
        }
    }
}
