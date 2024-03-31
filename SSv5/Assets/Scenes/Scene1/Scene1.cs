using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene1 : MonoBehaviour
{
    [SerializeField] private Text label;

    public string data { get; set; }

    private void Start()
    {
        label.text = data;

        ScreenManager.Load<Scene2>("Scene2", LoadSceneMode.Additive, (scene2) =>
        {
            scene2.cube.localScale = new Vector3(2, 1, 1);
        });

        var screen1 = ScreenManager.Add<Screen1>("Screen1", "RightShow", "RightHide");
        screen1.label.text = "Screen1";
    }
}
