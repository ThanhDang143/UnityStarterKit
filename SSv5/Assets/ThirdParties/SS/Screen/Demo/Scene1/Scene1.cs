using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene1 : MonoBehaviour
{
    [SerializeField] Text m_Label;

    public string label
    {
        get
        {
            return m_Label.text;
        }

        set
        {
            m_Label.text = value;
        }
    }

    public void OnScreen1ButtonTap()
    {
        var screen1 = ScreenManager.Add<Screen1>("Screen1", "FadeShow", "FadeHide");
        screen1.label = "Screen1 (from Scene1)";
    }

    public void OnScene2ButtonTap()
    {
        ScreenManager.Load<Scene2>("Scene2", LoadSceneMode.Single, (scene2) => {
            scene2.label = "Scene2 (from Scene1)";
        });
    }
}
