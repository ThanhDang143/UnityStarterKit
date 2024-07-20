using UnityEditor;
using UnityEditor.SceneManagement;

public class SSShortcut : EditorWindow
{
    [MenuItem("Tools/Scene/SceneManager &1")]
    private static void OpenSceneManager()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/SceneManager.unity");
        // EditorApplication.isPlaying = true;
    }

    [MenuItem("Tools/Scene/ScreenHome &2")]
    private static void OpenHomeScene()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/ScreenHome.unity");
    }

    [MenuItem("Tools/Scene/ScreenGame &3")]
    private static void OpenGameScene()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/ScreenGame.unity");
    }
}
