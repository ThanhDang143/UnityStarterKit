using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen1 : MonoBehaviour
{
    public Text label;

    public void OnCloseButtonTap()
    {
        ScreenManager.Close();
    }

    public void OnAddScreen2ButtonTap()
    {
        ScreenManager.Add<Screen2>("Screen2", "ScaleFadeShow", "ScaleFadeHide", "AnimationRoot", true, (screen) => {
            screen.label.text = "Screen2";
        });
    }
}
