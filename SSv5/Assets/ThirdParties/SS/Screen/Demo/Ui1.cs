using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ui1 : MonoBehaviour
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

    public void OnCloseButtonTap()
    {
        ScreenManager.Close();
    }

    public void OnLoadScene2ButtonTap()
    {
        ScreenManager.Load<Scene2>("Scene2", LoadSceneMode.Single, (scene2) => {
            scene2.label = "Scene2 (from Ui1)";
        });
    }

    public void OnCloseThenLoadScene2ButtonTap()
    {
        ScreenManager.Close(() =>
        {
            ScreenManager.Load<Scene2>("Scene2", LoadSceneMode.Single, (scene2) =>
            {
                scene2.label = "Scene2 (from Ui1)";
            });
        });
    }
}