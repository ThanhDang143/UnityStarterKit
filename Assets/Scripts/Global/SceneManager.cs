using System.Collections;
using System.Collections.Generic;
using BakingSheetImpl;
using UnityEngine;

public class SceneManager : SSSceneManager
{
    protected override void OnFirstSceneLoad()
    {
        base.OnFirstSceneLoad();

        Application.targetFrameRate = 60;
    }
}
