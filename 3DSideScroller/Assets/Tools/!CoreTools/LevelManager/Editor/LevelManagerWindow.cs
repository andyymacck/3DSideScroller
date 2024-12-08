using UnityEditor;
using UnityEngine;
using LevelManagerLoader;

public class LevelManagerWindow : EditorWindow
{
    public LevelManagerContainer scriptableObject;
    
    
    //[MenuItem("Tools/Level Manager Open Config")]
    public static void Open()
    {
        //EditorWindow.GetWindow(typeof(LevelManagerContainer)).titleContent = new GUIContent("LM Config");

        LevelManagerContainer scriptableObject = LevelManager.GetContainer();
        LevelManagerWindow.Open(scriptableObject);
    }
    
    public static void Open(LevelManagerContainer scriptableObject)
    {
        var window = GetWindow<LevelManagerWindow>("Level Manager Window");
        window.scriptableObject = scriptableObject;
    }

    private void OnGUI()
    {
        if (scriptableObject == null)
        {
            EditorGUILayout.LabelField("No ScriptableObject selected");
            return;
        }

        EditorGUI.BeginChangeCheck();

        scriptableObject.LoadingScene = EditorGUILayout.ObjectField("Loading Scene", scriptableObject.LoadingScene, typeof(Object), false);

        EditorGUILayout.Space();
        
        for(int i = 0; i < scriptableObject.LevelGroups.Count; i++)
        {
            EditorGUILayout.Space();
            
            var group = scriptableObject.LevelGroups[i];

            group.GroupType = (LevelGroupType)EditorGUILayout.EnumPopup("GroupType", group.GroupType);
            
            for(int j = 0; j < group.Levels.Count; j++)
            {
                var level = group.Levels[j];

                level.Scene = EditorGUILayout.ObjectField("Scene", level.Scene, typeof(Object), false);
                level.SceneName = EditorGUILayout.TextField("Name Scene", level.SceneName);
                level.Unlocked = EditorGUILayout.Toggle("Unlocked", level.Unlocked);
                level.Argument = EditorGUILayout.IntField("Argument", level.Argument);
                level.Argument_2 = EditorGUILayout.TextField("Argument 2", level.Argument_2);
                level.LevelIcon = (Sprite)EditorGUILayout.ObjectField("UI Background Icon", level.LevelIcon, typeof(Sprite), false);
            }
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(scriptableObject);
            AssetDatabase.SaveAssets();
        }
    }
}   
