using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenHome : SSController
{
    public void OnBtnPlayClicked()
    {
        SSSceneManager.Instance.Screen(ScreenNames.GAME);
    }
}
