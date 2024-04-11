using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 60;

        ScreenManager.Set(new Color(0, 0, 0, 0.8f), "Screens", "Animations", "SceneLoading", "Loading");

        ScreenManager.Load<Scene1>("Scene1", LoadSceneMode.Single, (scene1) =>
        {
            scene1.data = "Scene1";
        });
    }
}
