using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenGame : SSController
{
    public void OnBtnPauseClicked()
    {
        SSSceneManager.Instance.PopUp(PopupNames.PAUSE);
    }
}
