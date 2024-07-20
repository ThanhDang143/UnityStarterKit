using UnityEngine;

public class SceneManager : SSSceneManager
{
    protected override void OnFirstSceneLoad()
    {
        base.OnFirstSceneLoad();

        // Initialize here!
        Application.targetFrameRate = 60;
    }
}
