namespace LevelManagerLoader
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    public class LevelManagerSyncConfigWindow : EditorWindow
    {
        private LevelManagerContainer m_scriptableObject;
        private Vector2 m_scrollPos;

        public static List<string> LevelManagerToBuildSettings;
        public static List<string> BuildLevelManagerOk;
        public static List<string> BuildToLevelManager;
        
        [MenuItem("Tools/Level Manager/Open Config Synchronization")]
        static void Init()
        {
            LevelManagerContainer scriptableObject = LevelManager.GetContainer();
            Open(scriptableObject);
        }
        
        public static void Open(LevelManagerContainer scriptableObject)
        {
            var window = GetWindow<LevelManagerSyncConfigWindow>("Level Manager Sync info");
            window.m_scriptableObject = scriptableObject;
        }
        
        private void OnEnable()
        {
            m_scriptableObject = LevelManager.GetContainer();
        }

        private void OnGUI()
        {
            m_scrollPos = EditorGUILayout.BeginScrollView(new Vector2(m_scrollPos.x, m_scrollPos.y));
            
            m_scriptableObject = LevelManager.GetContainer();
            
            if (LevelManagerToBuildSettings.Count != 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Missing scenes in Build Settings:", MessageType.Error);
            }

            for (int i = 0; i < LevelManagerToBuildSettings.Count; i++)
            {
                EditorGUILayout.HelpBox(LevelManagerToBuildSettings[i], MessageType.None);
            }

            if (BuildToLevelManager.Count != 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Missing scenes in LevelManager:", MessageType.Error);
            }

            for (int i = 0; i < BuildToLevelManager.Count; i++)
            {
                EditorGUILayout.HelpBox(BuildToLevelManager[i], MessageType.None);
            }

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Scenes in Build Settings:", MessageType.Info);
            for (int i = 0; i < BuildLevelManagerOk.Count; i++)
            {
                EditorGUILayout.HelpBox(BuildLevelManagerOk[i], MessageType.None);
            }

            EditorGUILayout.Space();
            EditorGUILayout.EndScrollView();

            if (LevelManagerToBuildSettings.Count != 0 || BuildToLevelManager.Count != 0)
            {
                EditorGUILayout.Space(2);
                if (GUILayout.Button("Sync Scene"))
                {
                    EditorApplication.ExecuteMenuItem("Tools/Level Manager/Sync Scene For Build Setting");
                    this.Close();
                }
            }

            EditorGUILayout.Space(2);
            if (GUILayout.Button("Close"))
            {
                this.Close();
            }
        }
    }
}
