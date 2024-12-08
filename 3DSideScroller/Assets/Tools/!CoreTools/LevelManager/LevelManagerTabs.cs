namespace LevelManagerLoader
{
    using UnityEngine;
    using System;
    using UnityEngine.UI;
    
    [Serializable]
    public class Tab
    {
        [SerializeField] private Button m_tabButton;
        [SerializeField] private Text m_tabText;
        [SerializeField] private GameObject m_tabWarning;
        [SerializeField] private string m_tabName;
        [SerializeField] private LevelManagerButtonsGroup m_levelButtonsGroup;
        [SerializeField] private LevelGroupType m_levelGroupType;

        public LevelGroupType LevelGroupType => m_levelGroupType;
        public event Action<LevelGroupType> ButtonClickedEvent;

        public void Init()
        {
            m_levelButtonsGroup.InitButtons(m_levelGroupType);
            m_tabText.text = m_tabName;
            
            m_tabButton.onClick.RemoveAllListeners();
            m_tabButton.onClick.AddListener(OnButtonClick);
        }

        public void ShowGroup(bool isShow)
        {
            if (isShow)
            {
                m_levelButtonsGroup.Show();
                if (m_tabWarning && m_tabWarning.activeSelf)
                {
                    m_tabWarning.SetActive(false);
                    LevelManagerData.RemoveGroupDataKeyValue(m_levelGroupType, GroupGameParam.Warning.ToString());
                }
            }
            else
            {
                m_levelButtonsGroup.Hide();
            }
        }

        public void SetTabColor(Color color)
        {
            m_tabButton.targetGraphic.color = color;
        }

        public void ShowWarning()
        {
            bool isShow = LevelManagerData.GetGroupData(m_levelGroupType, GroupGameParam.Warning.ToString()).Length > 0;
            if(m_tabWarning) m_tabWarning.SetActive(isShow);
        }
        
        private void OnButtonClick()
        {
            ButtonClickedEvent?.Invoke(m_levelGroupType);
        }
    }
    public class LevelManagerTabs : MonoBehaviour
    {
        [SerializeField] private Tab[] m_tabs;
        [SerializeField] private Color m_coloActive;
        [SerializeField] private Color m_coloNonActive;


        private void Awake()
        {
            for (int i = 0; i < m_tabs.Length; i++)
            {
                m_tabs[i].Init();
                m_tabs[i].ButtonClickedEvent += OnTabButtonClick;
                m_tabs[i].ShowGroup(false);
            }
            
            m_tabs[0].ShowGroup(true);
            SetColor(LevelGroupType.Classic);
        }

        private void OnEnable()
        {
            for (int i = 0; i < m_tabs.Length; i++)
            {
                m_tabs[i].ShowWarning();
            }
        }

        private void OnTabButtonClick(LevelGroupType levelGroupType)
        {
            for (int i = 0; i < m_tabs.Length; i++)
            {
                m_tabs[i].ShowGroup(levelGroupType == m_tabs[i].LevelGroupType);
            }
            
            SetColor(levelGroupType);
            
            //Vibro.Medium();
            //SoundController.PlayClick();
            
            //EventBusHub.Instance.LevelManagerEvents.Publish(new OnLevelManagerTabClick(levelGroupType));
        }

        private void SetColor(LevelGroupType levelGroupType)
        {
            for (int i = 0; i < m_tabs.Length; i++)
            {
                Color color = levelGroupType == m_tabs[i].LevelGroupType ? m_coloActive : m_coloNonActive;
                m_tabs[i].SetTabColor(color);
            }
        }
    }
}
