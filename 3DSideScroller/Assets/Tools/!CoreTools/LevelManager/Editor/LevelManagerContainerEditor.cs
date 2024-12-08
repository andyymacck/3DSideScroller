namespace LevelManagerLoader
{
    using System;
    using UnityEditor;
    using UnityEngine;
    using System.Collections.Generic;

    [CustomEditor(typeof(LevelManagerContainer))]
    public class LevelManagerContainerEditor : Editor
    {
        private LevelManagerContainer m_container;
        private SerializedObject m_containerObject;
        private SerializedProperty m_levelGroupsList;
        private SerializedProperty m_levelsList;
        private string m_duplicateGroupName;
        private string m_duplicateLevelName;
        private string m_duplicateFileName;
        private string m_nullSceneName;

        private Vector2 m_scrollPos;
        private bool[] m_open;
        private bool m_openDuplicateFileName;
        private bool m_openButton;
        private bool m_autoCheckScenes;
        private bool m_checkScene;

        private string m_duplicateLevelNameText1 = "";
        private string m_nullSceneNameText2 = "";
        private string m_duplicateFileNameText3 = "";
        private string m_groupIsMoreText = "#";

        private int m_timerRest;

        private void OnEnable()
        {
            m_container = (LevelManagerContainer)target;
            m_containerObject = new SerializedObject(m_container);
            m_levelGroupsList = m_containerObject.FindProperty("LevelGroups");
            m_open = new bool[m_container.LevelGroups.Count];
        }

        public override void OnInspectorGUI()
        {
            Event eventCurrent = Event.current;
            m_containerObject.Update();

            EditorGUILayout.Space(2);
            EditorGUILayout.PropertyField(m_containerObject.FindProperty("StartGameScene"), new GUIContent("Start Game Scene", "Start Game Scene"), true);
            EditorGUILayout.PropertyField(m_containerObject.FindProperty("LoadingScene"), new GUIContent("Loading Scene", "Loading Scene"), true);
            EditorGUILayout.Space(3);

            m_container.LoadingSceneFileName = m_container.LoadingScene != null ? m_container.LoadingScene.name : "";

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox($"Level Groups Size {m_levelGroupsList.arraySize}", MessageType.None, true);
            if (GUILayout.Button("Add New Group"))
            {
                if (m_container.LevelGroups.Count == Enum.GetNames(typeof(LevelGroupType)).Length - 1)
                {
                    m_groupIsMoreText = "Level Group Type is More";
                    return;
                }

                m_container.LevelGroups.Add(new LevelGroup());
                m_open = new bool[m_container.LevelGroups.Count];
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Scene file name", GUILayout.MaxWidth(100), GUILayout.MaxHeight(20));
            m_openDuplicateFileName = GUILayout.Toggle(m_openDuplicateFileName, "Check Duplicate");
            EditorGUILayout.EndHorizontal();

            #region HelpBoxs

            EditorGUILayout.Space(3);
            if (m_groupIsMoreText != "#")
            {
                EditorGUILayout.HelpBox(m_groupIsMoreText, MessageType.Warning, true);
            }

            EditorGUILayout.Space(2);
            if (m_duplicateGroupName != "#")
            {
                EditorGUILayout.HelpBox(string.Concat($"DUPLICATE GROUP SELECT: {m_duplicateGroupName}", "\n"), MessageType.Error);
            }

            if (m_duplicateLevelName != "#" || m_nullSceneName != "#" || m_duplicateFileName != "#")
            {
                EditorGUILayout.HelpBox(string.Concat(m_duplicateLevelNameText1, m_nullSceneNameText2, m_duplicateFileNameText3), MessageType.Error);
            }

            EditorGUILayout.Space(2);
            EditorGUILayout.HelpBox("For Remove Group or Level press shift and button", MessageType.Info);

            m_timerRest++;
            if (m_timerRest >= 100)
            {
                m_timerRest = 0;
                m_groupIsMoreText = "#";
            }

            m_duplicateGroupName = m_duplicateLevelName = m_duplicateFileName = m_nullSceneName = "#";

            int groupIsOpen = -1;
            for (int i = 0; i < m_open.Length; i++)
            {
                if (m_open[i])
                {
                    groupIsOpen = i;
                }
            }

            EditorGUILayout.Space(4);
            if (groupIsOpen >= 0)
            {
                EditorGUILayout.HelpBox($"Group #{groupIsOpen + 1}: {m_container.LevelGroups[groupIsOpen].GroupType}", MessageType.None);
            }

            #endregion HelpBoxs

            EditorGUILayout.Space(4);
            m_scrollPos = EditorGUILayout.BeginScrollView(new Vector2(m_scrollPos.x, m_scrollPos.y));
            EditorGUILayout.Space(2);

            for (int group = 0; group < m_levelGroupsList.arraySize; group++)
            {
                SerializedProperty groupsRef = m_levelGroupsList.GetArrayElementAtIndex(group);
                SerializedProperty groupType = groupsRef.FindPropertyRelative("GroupType");

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Open", GUILayout.MaxWidth(50), GUILayout.MaxHeight(20)))
                {
                    m_openButton = !m_openButton;
                    m_open[group] = m_openButton;
                }

                EditorGUILayout.PropertyField(groupType, new GUIContent($"Group Type #{group + 1}:", $"Group Type #{group + 1}"), GUILayout.MaxWidth(500), GUILayout.MaxHeight(17));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(3);
                m_levelsList = groupsRef.FindPropertyRelative("Levels");
                for (int g = 0; g < m_levelGroupsList.arraySize; g++)
                {
                    if (m_container.LevelGroups[group].GroupType == m_container.LevelGroups[g].GroupType && group != g)
                    {
                        m_duplicateGroupName = new string($"Group #{group + 1}: {m_container.LevelGroups[g].GroupType.ToString()}");
                        break;
                    }
                }

                EditorGUILayout.Space(4);

                if (m_open[group])
                {
                    for (int level = 0; level < m_levelsList.arraySize; level++)
                    {
                        EditorGUILayout.Space(2);
                        string nameEditor = m_container.LevelGroups[group].GroupType == LevelGroupType.Menu ? "Menu" : "Level";
                        EditorGUILayout.HelpBox($"{nameEditor} #{level + 1}, Group #{group + 1}: {m_container.LevelGroups[group].GroupType}", MessageType.None);

                        EditorGUILayout.Space();
                        if (GUILayout.Button($"+ Add New Level #{level + 1}", GUILayout.MaxWidth(130), GUILayout.MaxHeight(20)))
                        {
                            List<LevelManagerLevelParam> newList = new List<LevelManagerLevelParam>();
                            newList.Add(new LevelManagerLevelParam());
                            m_container.LevelGroups[group].Levels.InsertRange(level, newList);
                            break;
                        }
                        EditorGUILayout.Space();
                        
                        SerializedProperty levelsListRef = m_levelsList.GetArrayElementAtIndex(level);
                        SerializedProperty levelType = levelsListRef.FindPropertyRelative("LevelType");
                        SerializedProperty scene = levelsListRef.FindPropertyRelative("Scene");
                        SerializedProperty sceneName = levelsListRef.FindPropertyRelative("SceneName");
                        SerializedProperty unlocked = levelsListRef.FindPropertyRelative("Unlocked");
                        SerializedProperty argument = levelsListRef.FindPropertyRelative("Argument");
                        SerializedProperty argument2 = levelsListRef.FindPropertyRelative("Argument_2");
                        SerializedProperty unlockNextLevelInGroup = levelsListRef.FindPropertyRelative("UnlockNextLevelInGroup");
                        SerializedProperty unlockNextLevel = levelsListRef.FindPropertyRelative("UnlockNextLevel");
                        SerializedProperty levelIcon = levelsListRef.FindPropertyRelative("LevelIcon");
                        
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(scene);
                        if (GUILayout.Button("-", GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)) && eventCurrent.shift)
                        {
                            m_levelsList.DeleteArrayElementAtIndex(level);
                            break;
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.PropertyField(levelType);
                        EditorGUILayout.PropertyField(sceneName, new GUIContent("SceneName", m_container.LevelGroups[group].Levels[level].SceneName), false);
                        EditorGUILayout.PropertyField(unlocked);
                        EditorGUILayout.PropertyField(argument);
                        EditorGUILayout.PropertyField(argument2, new GUIContent("Argument #2", "Argument #2"), true);
                        EditorGUILayout.PropertyField(unlockNextLevelInGroup, new GUIContent("Unlock Next Level In Group", "Unlock Next Level In Group"), true);
                        EditorGUILayout.PropertyField(unlockNextLevel, new GUIContent("Unlock Next Level", "Unlock Next Level"), true);
                        EditorGUILayout.Space(2);
                        EditorGUILayout.PropertyField(levelIcon, new GUIContent("UI Background Icon", "UI Background Icon"), false);
                        EditorGUILayout.ObjectField("UI Background Icon", m_container.LevelGroups[group].Levels[level].LevelIcon, typeof(Sprite));
                        EditorGUILayout.Space(2);

                        m_container.LevelGroups[group].Levels[level].LevelNum = level + 1;
                        m_container.LevelGroups[group].Levels[level].LevelGroupType = m_container.LevelGroups[group].GroupType;
                        m_container.LevelGroups[group].Levels[level].FileName =
                            m_container.LevelGroups[group].Levels[level].Scene == null ? "" : m_container.LevelGroups[group].Levels[level].Scene.name;

                        
                        #region Check Dublicate Name

                        for (int g = 0; g < m_levelGroupsList.arraySize; g++)
                        {
                            for (int l = 0; l < m_container.LevelGroups[g].Levels.Count; l++)
                            {
                                if (m_container.LevelGroups[g].Levels[l].Scene == null)
                                {
                                    m_nullSceneName = new string($"{m_container.LevelGroups[g].Levels[l].SceneName}, for Group #{g + 1}: {m_container.LevelGroups[g].GroupType}, Level #: {l + 1}");
                                    break;
                                }

                                if (m_container.LevelGroups[group].Levels[level].SceneName == m_container.LevelGroups[g].Levels[l].SceneName && level != l)
                                {
                                    m_duplicateLevelName =
                                        new string($"{m_container.LevelGroups[g].Levels[l].SceneName}, for Group #{g + 1}: {m_container.LevelGroups[g].GroupType}, Level #: {l + 1}");
                                }
                            }
                        }

                        #endregion Check Dublicate Name
                    }

                    m_duplicateLevelNameText1 = m_duplicateLevelName == "#" ? "" : string.Concat($"DUPLICATE LEVEL NAME : {m_duplicateLevelName}", "\n");
                    m_nullSceneNameText2 = m_nullSceneName == "#" ? "" : string.Concat($"NULL SCENE : {m_nullSceneName}", "\n");

                    EditorGUILayout.Space();
                    if (GUILayout.Button($"+ Add New Level", GUILayout.MaxWidth(130), GUILayout.MaxHeight(20)))
                    {
                        m_container.LevelGroups[group].Levels.Add(new LevelManagerLevelParam());
                    }

                    EditorGUILayout.Space();
                }

                if (m_openDuplicateFileName)
                {
                    for (int g = 0; g < m_levelGroupsList.arraySize; g++)
                    {
                        for (int level = 0; level < m_levelsList.arraySize; level++)
                        {
                            for (int l = 0; l < m_container.LevelGroups[g].Levels.Count; l++)
                            {
                                if (m_container.LevelGroups[group].Levels[level].Scene == null || m_container.LevelGroups[g].Levels[l].Scene == null) break;

                                if (m_container.LevelGroups[group].Levels[level].Scene == m_container.LevelGroups[g].Levels[l].Scene && level != l)
                                {
                                    m_duplicateFileName =
                                        new string($"{m_container.LevelGroups[g].Levels[l].Scene.name}, for Group #{g + 1}: {m_container.LevelGroups[g].GroupType}, Level #: {l + 1}");
                                }
                            }
                        }
                    }
                }

                m_duplicateFileNameText3 = m_duplicateFileName == "#" && !m_openDuplicateFileName ? "" : string.Concat($"DUPLICATE SCENE (NAME/FILE): {m_duplicateFileName}", "\n");

                #region Remove This Group List

                if (m_container.LevelGroups.Count != 0)
                {
                    if (GUILayout.Button($"Remove group ''{m_container.LevelGroups[group].GroupType}''") && eventCurrent.shift)
                    {
                        m_levelGroupsList.DeleteArrayElementAtIndex(group);
                    }
                }

                EditorGUILayout.Space();

                #endregion Remove This Group List

            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.Space(2);

            #region GUILayout.Button("Check Scene")

            EditorGUILayout.BeginHorizontal();
            m_autoCheckScenes = GUILayout.Toggle(m_autoCheckScenes, "Auto Check Scenes   or ", GUILayout.MaxWidth(160), GUILayout.MaxHeight(20));

            if (GUILayout.Button("Get info", GUILayout.MaxWidth(80), GUILayout.MaxHeight(20)))
            {
                m_checkScene = !m_checkScene;
            }

            EditorGUILayout.EndHorizontal();
            if (m_checkScene || m_autoCheckScenes)
            {
                EditorBuildSettingsScene[] buildSettingsScenes = EditorBuildSettings.scenes;
                List<string> buildSettingsScenesTemp = new List<string>();

                for (int bs = 0; bs < buildSettingsScenes.Length; bs++)
                {
                    buildSettingsScenesTemp.Add(buildSettingsScenes[bs].path);
                }

                List<string> levelManagerToBuildSettingsTemp = new List<string>();
                List<string> buildLevelManagerOkTemp = new List<string>();
                List<string> buildToLevelManagerTemp = new List<string>();

                for (int group = 0; group < m_levelGroupsList.arraySize; group++)
                {
                    if (group == 0 && m_container.StartGameScene != null)
                    {
                        levelManagerToBuildSettingsTemp.Add(AssetDatabase.GetAssetOrScenePath(m_container.StartGameScene));
                    }

                    if (group == 0 && m_container.LoadingScene != null)
                    {
                        levelManagerToBuildSettingsTemp.Add(AssetDatabase.GetAssetOrScenePath(m_container.LoadingScene));
                    }

                    for (int level = 0; level < m_container.LevelGroups[group].Levels.Count; level++)
                    {
                        if (m_container.LevelGroups[group].Levels[level].Scene != null)
                        {
                            levelManagerToBuildSettingsTemp.Add(AssetDatabase.GetAssetOrScenePath(m_container.LevelGroups[group].Levels[level].Scene));
                        }
                    }
                }

                int pp = 0;
                for (int bs = 0; bs < buildSettingsScenesTemp.Count; bs++)
                {
                    string path = buildSettingsScenesTemp[bs];
                    for (int lm = 0; lm < levelManagerToBuildSettingsTemp.Count; lm++)
                    {
                        if (path == levelManagerToBuildSettingsTemp[lm])
                        {
                            if (buildLevelManagerOkTemp.Count == 0)
                            {
                                buildLevelManagerOkTemp.Add(path);
                            }
                            else
                            {
                                for (int b_lm_ok = 0; b_lm_ok < buildLevelManagerOkTemp.Count; b_lm_ok++)
                                {
                                    if (buildLevelManagerOkTemp[b_lm_ok] == levelManagerToBuildSettingsTemp[lm])
                                    {
                                        pp++;
                                    }
                                }

                                if (pp < 1)
                                {
                                    buildLevelManagerOkTemp.Add(path);
                                }

                                pp = 0;
                            }
                        }
                    }
                }

                for (int bs = 0; bs < buildLevelManagerOkTemp.Count; bs++)
                {
                    int b_lm_ok = 0;
                    while (b_lm_ok < levelManagerToBuildSettingsTemp.Count)
                    {
                        if (levelManagerToBuildSettingsTemp[b_lm_ok] == buildLevelManagerOkTemp[bs])
                        {
                            levelManagerToBuildSettingsTemp.Remove(levelManagerToBuildSettingsTemp[b_lm_ok]);
                            b_lm_ok = -1;
                        }

                        b_lm_ok++;
                    }
                }

                for (int bs = 0; bs < buildLevelManagerOkTemp.Count; bs++)
                {
                    for (int b_lm_ok = 0; b_lm_ok < buildSettingsScenesTemp.Count; b_lm_ok++)
                    {
                        if (buildLevelManagerOkTemp[bs] == buildSettingsScenesTemp[b_lm_ok])
                        {
                            buildSettingsScenesTemp.Remove(buildSettingsScenesTemp[b_lm_ok]);
                        }
                    }
                }

                for (int LMS = 0; LMS < buildSettingsScenesTemp.Count; LMS++)
                {
                    if (buildSettingsScenesTemp[LMS] != null)
                    {
                        buildToLevelManagerTemp.Add(buildSettingsScenesTemp[LMS]);
                    }
                }

                LevelManagerSyncConfigWindow.LevelManagerToBuildSettings = levelManagerToBuildSettingsTemp;
                LevelManagerSyncConfigWindow.BuildLevelManagerOk = buildLevelManagerOkTemp;
                LevelManagerSyncConfigWindow.BuildToLevelManager = buildToLevelManagerTemp;

                if (m_checkScene)
                {
                    EditorApplication.ExecuteMenuItem("Tools/Level Manager/Open Config Synchronization");
                    Debug.Log("Check Finished");
                }

                m_checkScene = false;

                if (levelManagerToBuildSettingsTemp.Count != 0 || buildToLevelManagerTemp.Count != 0 || !m_autoCheckScenes)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (levelManagerToBuildSettingsTemp.Count != 0 || buildToLevelManagerTemp.Count != 0)
                    {
                        EditorGUILayout.HelpBox($"Need synchronization scenes!", MessageType.Error);
                    }

                    if (GUILayout.Button("Sync Scenes", GUILayout.MaxWidth(130), GUILayout.MaxHeight(40)))
                    {
                        SyncScenes();
                        m_checkScene = true;
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }

            #endregion GUILayout.Button("Check Scene")

            m_containerObject.ApplyModifiedProperties();
        }

        #region GUILayout.Button("Sync Scenes")

        [MenuItem("Tools/Level Manager/Sync Scene For Build Setting")]
        static void SyncScenes()
        {
            LevelManagerContainer container = LevelManager.GetContainer();
            int originalIndex = 0;
            EditorBuildSettings.scenes = new EditorBuildSettingsScene[originalIndex];
            for (int group = 0; group < container.LevelGroups.Count; group++)
            {
                for (int level = 0; level < container.LevelGroups[group].Levels.Count; level++)
                {
                    EditorBuildSettingsScene[] original = EditorBuildSettings.scenes;
                    if (original.Length == 0)
                    {
                        EditorBuildSettingsScene[] newSettings = new EditorBuildSettingsScene[original.Length + 1];
                        Array.Copy(original, newSettings, original.Length);
                        if (container.LevelGroups[group].Levels[level].Scene != null)
                        {
                            EditorBuildSettingsScene sceneToAdd = new(AssetDatabase.GetAssetOrScenePath(container.LevelGroups[group].Levels[level].Scene), true);
                            newSettings[newSettings.Length - 1] = sceneToAdd;
                            EditorBuildSettings.scenes = newSettings;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < original.Length; i++)
                        {
                            if (AssetDatabase.GetAssetOrScenePath(container.LevelGroups[group].Levels[level].Scene) == original[i].path)
                            {
                                originalIndex++;
                            }
                        }

                        if (originalIndex < 1 && container.LevelGroups[group].Levels[level].Scene != null)
                        {
                            EditorBuildSettingsScene[] newSettings = new EditorBuildSettingsScene[original.Length + 1];
                            Array.Copy(original, newSettings, original.Length);
                            EditorBuildSettingsScene sceneToAdd = new(AssetDatabase.GetAssetOrScenePath(container.LevelGroups[group].Levels[level].Scene), true);
                            newSettings[newSettings.Length - 1] = sceneToAdd;
                            EditorBuildSettings.scenes = newSettings;
                        }

                        originalIndex = 0;
                    }
                }

                if (group == 0 && container.LoadingScene != null)
                {
                    EditorBuildSettingsScene[] original = EditorBuildSettings.scenes;
                    EditorBuildSettingsScene[] newSettings = new EditorBuildSettingsScene[original.Length + 1];
                    Array.Copy(original, newSettings, original.Length);
                    EditorBuildSettingsScene sceneToAdd = new(AssetDatabase.GetAssetOrScenePath(container.LoadingScene), true);
                    newSettings[newSettings.Length - 1] = sceneToAdd;
                    EditorBuildSettings.scenes = newSettings;
                }

                if (group == 0 && container.StartGameScene != null)
                {
                    EditorBuildSettingsScene[] original = EditorBuildSettings.scenes;
                    EditorBuildSettingsScene[] newSettings = new EditorBuildSettingsScene[original.Length + 1];
                    Array.Copy(original, newSettings, original.Length);
                    for (int i = 0; i < original.Length; i++)
                    {
                        newSettings[i + 1] = original[i];
                    }

                    EditorBuildSettingsScene sceneToAdd = new(AssetDatabase.GetAssetOrScenePath(container.StartGameScene), true);
                    newSettings[0] = sceneToAdd;
                    EditorBuildSettings.scenes = newSettings;
                }
            }
        }

        #endregion GUILayout.Button("Sync Scene")

    }
}