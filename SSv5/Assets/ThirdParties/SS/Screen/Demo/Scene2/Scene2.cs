using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene2 : MonoBehaviour
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

    public void OnScene1ButtonTap()
    {
        ScreenManager.Load<Scene1>("Scene1", LoadSceneMode.Single, (scene1) => {
            scene1.label = "Scene1 (from Scene2)";
        });
    }

    public void OnScene3ButtonTap()
    {
        ScreenManager.Load<Scene3>("Scene3", LoadSceneMode.Additive, (scene3) => {
            scene3.cube.localScale = new Vector3(2, 1, 1);
        });
    }
}
