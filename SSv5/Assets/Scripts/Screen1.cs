using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen1 : MonoBehaviour, IKeyBack
{
    public Text label;

    private bool pressedSpaceKey;

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }

    public void OnAddScreen2ButtonTap()
    {
        ScreenManager.Add<Screen2>("Screen2", "ScaleFadeShow", "ScaleFadeHide", "AnimationRoot", true, (screen) => {
            screen.label.text = "Screen2";
        });
    }

    public void OnAddScreen2UntilNoScreenButtonTap()
    {
        ScreenManager.Add<Screen2>("Screen2", "ScaleFadeShow", "ScaleFadeHide", "AnimationRoot", false, (screen) => {
            screen.label.text = "Screen2";
        }, waitUntilNoScreen: true);
    }

    public void OnAddScreen2UntilSpacePressedButtonTap()
    {
        ScreenManager.Add<Screen2>("Screen2", "ScaleFadeShow", "ScaleFadeHide", "AnimationRoot", false, (screen) => {
            screen.label.text = "Screen2";
            pressedSpaceKey = false;
        }, addCondition: WaitSpaceKey);
    }

    public void OnAddScreen3AndDestroyMeButtonTap()
    {
        ScreenManager.Add<Screen3>("Screen3", "ScaleFadeShow", "ScaleFadeHide", "AnimationRoot", false, (screen) => {
            screen.label.text = "Screen3";
        }, destroyTopScreen: true);
    }

    private bool WaitSpaceKey()
    {
        return pressedSpaceKey;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pressedSpaceKey = true;
        }
    }
}
