using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SSManager.Manager;

public class Screen2 : MonoBehaviour, IBtnBack
{
    public Text label;

    public void OnLoadScene1ButtonTap()
    {
        ScreenManager.Load<Scene1>(sceneName: "Scene1", mode: LoadSceneMode.Single, onSceneLoaded: (scene1) =>
        {
            scene1.data = "Scene1...";
        });
    }

    public void OnBtnBackClicked()
    {
        ScreenManager.Close();
    }

    public void OnCloseScreen1ButtonTap()
    {
        var screen1 = FindFirstObjectByType<Screen1>();

        if (screen1 != null)
        {
            ScreenManager.Destroy(screen: screen1);
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

    public void OnAddScreen1ButtonTap()
    {
        ScreenManager.Add<Screen1>(screenName: "Screen1", showAnimation: ScreenAnimation.RightShow, hideAnimation: ScreenAnimation.RightHide, useExistingScreen: true, onScreenLoad: (screen) =>
        {
            screen.label.text = "Screen1";
        });
    }

    public void OnShowTooltipButtonTap(Button button)
    {
        ScreenManager.ShowTooltip(text: "This is a long tooltip to test overflowing the screen", worldPosition: button.transform.position, targetY: Random.Range(100f, 300f));
    }

    private IEnumerator ShowLoadingASecond()
    {
        ScreenManager.Loading(true);

        yield return new WaitForSecondsRealtime(1);

        ScreenManager.Loading(false);
    }
}
