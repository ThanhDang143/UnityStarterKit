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

    public void OnAddScreen2UntilSpacePressedButtonTap()
    {
        ScreenManager.Add<Screen2>("Screen2", "ScaleFadeShow", "ScaleFadeHide", "AnimationRoot", true, (screen) => {
            screen.label.text = "Screen2";
            pressedSpaceKey = false;
        }, addCondition: WaitSpaceKey);
    }

    public void OnAddScreen2And3ButtonTap()
    {
        ScreenManager.Close();

        ScreenManager.Add<Screen2>("Screen2", "ScaleFadeShow", "ScaleFadeHide", "AnimationRoot", true, (screen) => {
            screen.label.text = "Screen2";
        }, waitUntilNoScreen:true);

        ScreenManager.Add<Screen3>("Screen3", "ScaleFadeShow", "ScaleFadeHide", "AnimationRoot", true, (screen) => {
            screen.label.text = "Screen3";
        }, waitUntilNoScreen: true);
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
