using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen1 : MonoBehaviour
{
    public Text label;

    private void Start()
    {
        var screen2 = ScreenManager.Add<Screen2>("Screen2", "ScaleFadeShow", "ScaleFadeHide");
        screen2.label.text = "Screen2";
    }

    public void OnCloseButtonTap()
    {
        ScreenManager.Close(null, "ScaleHide");
    }
}
