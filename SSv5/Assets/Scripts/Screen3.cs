using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Screen3 : MonoBehaviour, IKeyBack
{
    public Text label;

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}
