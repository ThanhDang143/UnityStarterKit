// This code is part of the SS-Scene library, released by Anh Pham (anhpt.csit@gmail.com).

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSettings : EditorWindow
{
    public string screenManagerPath;
    public int screenWidth = 720;
    public int screenHeight = 1600;

    [MenuItem("SS/Screen Settings")]
    public static void ShowScreenSettings()
    {
        ScreenSettings win = ScriptableObject.CreateInstance<ScreenSettings>();

        win.minSize = new Vector2(400, 200);
        win.maxSize = new Vector2(400, 200);

        win.ShowUtility();

        win.LoadPrefs();
    }

    [MenuItem("SS/Resize Game Window")]
    public static void ResizeGameWindow()
    {
        var prefabInstance = PrefabUtility.LoadPrefabContents(SS.IO.Searcher.SearchFileInProject("ScreenManager.prefab", SS.IO.Searcher.PathType.Relative));

        var canvasScaler = prefabInstance.GetComponentInChildren<CanvasScaler>();
        var width = canvasScaler.referenceResolution.x;
        var height = canvasScaler.referenceResolution.y;

        SS.Tool.GameWindow.Resize((int)width, (int)height);
    }

    void LoadPrefs()
    {
        screenWidth = EditorPrefs.GetInt("SS_SCREEN_WIDTH", 720);
        screenHeight = EditorPrefs.GetInt("SS_SCREEN_HEIGHT", 1600);
        screenManagerPath = SS.IO.Searcher.SearchFileInProject("ScreenManager.prefab", SS.IO.Searcher.PathType.Relative);
    }

    void SavePrefs()
    {
        EditorPrefs.SetInt("SS_SCREEN_WIDTH", screenWidth);
        EditorPrefs.SetInt("SS_SCREEN_HEIGHT", screenHeight);
    }

    void OnGUI()
    {
        GUILayout.Label("Scene Generator", EditorStyles.boldLabel);
        screenManagerPath = EditorGUILayout.TextField("Screen Manager Path", screenManagerPath);
        screenWidth = EditorGUILayout.IntField("Screen Width", screenWidth);
        screenHeight = EditorGUILayout.IntField("Screen Height", screenHeight);

        if (GUILayout.Button("Save"))
        {
            if (screenWidth > 0 && screenHeight > 0)
            {
                SavePrefs();
                EditScreenManager();
                SS.Tool.GameWindow.Resize(screenWidth, screenHeight);
                Close();
            }
        }
    }

    void EditScreenManager()
    {
        string prefabPath = screenManagerPath;

        var prefabInstance = PrefabUtility.LoadPrefabContents(prefabPath);

        var canvasScaler = prefabInstance.GetComponentInChildren<CanvasScaler>();
        canvasScaler.referenceResolution = new Vector2(screenWidth, screenHeight);

        PrefabUtility.SaveAsPrefabAsset(prefabInstance, prefabPath);

        PrefabUtility.UnloadPrefabContents(prefabInstance);
    }
}