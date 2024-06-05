using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen1 : MonoBehaviour
{
    public Text label;

    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(0.283f);

        var screen2 = ScreenManager.Add<Screen2>("Screen2", "ScaleFadeShow", "ScaleFadeHide", "AnimationRoot");
        screen2.label.text = "Screen2";
    }

    public void OnCloseButtonTap()
    {
        ScreenManager.Close();
    }
}
