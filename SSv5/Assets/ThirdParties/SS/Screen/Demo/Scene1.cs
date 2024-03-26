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

    private void Awake()
    {
        Debug.Log("Awake Scene1");
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable Scene1");
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable Scene1");
    }

    private void Start()
    {
        Debug.Log("Start Scene1");
    }

    public void OnUi1ButtonTap()
    {
        var ui1 = ScreenManager.Add<Ui1>("Ui1");
        ui1.label = "Ui1 (from Scene1)";
    }

    public void OnScene2ButtonTap()
    {
        ScreenManager.Load<Scene2>("Scene2", LoadSceneMode.Single, (scene2) => {
            scene2.label = "Scene2 (from Scene1)";
        });
    }
}
