using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Screen2 : MonoBehaviour
{
    public Text label;

    public void OnLoadScene1ButtonTap()
    {
        ScreenManager.Load<Scene1>("Scene1", LoadSceneMode.Single, (scene1) =>
        {
            scene1.data = "Scene1...";
        });
    }

    public void OnCloseButtonTap()
    {
        ScreenManager.Close();
    }

    public void OnCloseScreen1ButtonTap()
    {
        var screen1 = FindObjectOfType<Screen1>(true);

        if (screen1 != null)
        {
            ScreenManager.Destroy(screen1);
        }
    }

    public void OnCloseAllScreensButtonTap()
    {
        ScreenManager.DestroyAll();
    }

    public void OnShowLoadingButtonTap()
    {
        StartCoroutine(ShowLoadingASecond());
    }

    private IEnumerator ShowLoadingASecond()
    {
        ScreenManager.Loading(true);

        yield return new WaitForSecondsRealtime(1);

        ScreenManager.Loading(false);
    }
}
