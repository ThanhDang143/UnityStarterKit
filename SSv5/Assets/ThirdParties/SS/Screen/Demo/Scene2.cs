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

    private void Awake()
    {
        Debug.Log("Awake Scene2");
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable Scene2");
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable Scene2");
    }

    private void Start()
    {
        Debug.Log("Start Scene2");
    }

    public void OnScene1ButtonTap()
    {
        ScreenManager.Load<Scene1>("Scene1", LoadSceneMode.Single, (scene1) => {
            scene1.label = "Scene1 (from Scene2)";
        });
    }
}
