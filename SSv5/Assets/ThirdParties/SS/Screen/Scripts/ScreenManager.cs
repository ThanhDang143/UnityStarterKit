using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    #region SerializeField
    [SerializeField] string m_UiPath = "UI/Prefabs";
    [SerializeField] string m_UiAnimationPath = "UI/Animations";
    [SerializeField] Camera m_BgCamera;
    [SerializeField] Canvas m_Canvas;
    [SerializeField] Animation m_SceneShield;
    [SerializeField] Color m_UIShieldColor = new Color(0, 0, 0, 0.8f);
    #endregion

    #region Delegate
    public delegate void OnSceneLoad<T>(T t);
    #endregion

    #region Private Member
    private Scene m_LastLoadedScene;
    private List<Component> m_ScreenList = new List<Component>();
    #endregion

    #region Static
    private static ScreenManager m_Instance;
    public static ScreenManager instance
    {
        get
        {
            if (m_Instance == null)
            {
                Instantiate(Resources.Load<ScreenManager>("ScreenManager"));
            }

            return m_Instance;
        }
    }

    public static void Load<T>(string sceneName, LoadSceneMode mode, OnSceneLoad<T> onSceneLoaded = null) where T : Component
    {
        instance.LoadScene(sceneName, mode, onSceneLoaded);
    }

    public static T Add<T>(string uiName, string showAnimation = "ScaleShow", string hideAnimation = "ScaleHide") where T : Component
    {
        return instance.AddScreen<T>(uiName, showAnimation, hideAnimation);
    }

    public static void Close()
    {
        instance.CloseScreen();
    }

    public static void Close(string hideAnimation = null)
    {
        instance.CloseScreen(hideAnimation);
    }

    public static void Close(Component ui, string hideAnimation = null)
    {
        instance.CloseScreen(ui, hideAnimation);
    }
    #endregion

    #region Unity Functions
    private void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;

            SetupCameras();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    private void SceneManager_sceneUnloaded(Scene scene)
    {
        Debug.Log("SceneManager_sceneUnloaded: " + scene.name);

        m_BgCamera.gameObject.SetActive(true);
    }

    private void SceneManager_activeSceneChanged(Scene scene1, Scene scene2)
    {
        Debug.Log("SceneManager_activeSceneChanged: " + scene1.name + " " + scene2.name);
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("SceneManager_sceneLoaded: " + scene.name + " " + mode);

        SetupCameras();

        m_LastLoadedScene = scene;
    }
    #endregion

    #region Private Functions
    private void LoadScene<T>(string sceneName, LoadSceneMode mode, OnSceneLoad<T> onSceneLoaded = null) where T : Component
    {
        StartCoroutine(CoLoadScene(sceneName, mode, onSceneLoaded));
    }

    private IEnumerator CoLoadScene<T>(string sceneName, LoadSceneMode mode, OnSceneLoad<T> onSceneLoaded = null) where T : Component
    {
        m_SceneShield.transform.SetAsLastSibling();
        m_SceneShield.Play("ShieldShow");

        yield return new WaitForSeconds(m_SceneShield["ShieldShow"].length);

        var asyncOperation = SceneManager.LoadSceneAsync(sceneName, mode);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        m_SceneShield.Play("ShieldHide");

        onSceneLoaded?.Invoke(GetSceneComponent<T>(instance.m_LastLoadedScene));
    }

    private T AddScreen<T>(string uiName, string showAnimation = "ScaleShow", string hideAnimation = "ScaleHide") where T : Component
    {
        var shield = CreateShield();

        var ui = Instantiate(Resources.Load<T>(Path.Combine(m_UiPath, uiName)), m_Canvas.transform);
        ui.transform.localPosition = Vector3.zero;
        ui.transform.localScale = Vector3.one;
        ui.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        var controller = AddScreenController(ui);
        controller.shield = shield;
        controller.showAnimation = showAnimation;
        controller.hideAnimation = hideAnimation;

        AddAnimations(ui, showAnimation, hideAnimation);
        PlayAnimation(ui, showAnimation);

        m_ScreenList.Add(ui);

        return ui;
    }

    private void CloseScreen(string hideAnimation = null)
    {
        if (m_ScreenList.Count > 0)
        {
            var ui = m_ScreenList[m_ScreenList.Count - 1];
            CloseScreen(ui, hideAnimation);
        }
    }

    private void CloseScreen(Component ui, string hideAnimation = null)
    {
        if (m_ScreenList.Count > 0)
        {
            m_ScreenList.Remove(ui);

            hideAnimation = (hideAnimation != null) ? hideAnimation : ui.GetComponent<ScreenController>().hideAnimation;
            PlayAnimation(ui, hideAnimation, true);
        }
    }

    private GameObject CreateShield()
    {
        var shield = new GameObject("Shield", typeof(Image));

        Transform t = shield.transform;
        t.SetParent(m_Canvas.transform);
        t.SetSiblingIndex(0);
        t.localScale = Vector3.one;
        t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, 0);

        RectTransform rt = t.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.offsetMax = new Vector2(2, 2);
        rt.offsetMin = new Vector2(-2, -2);

        var image = shield.GetComponent<Image>();
        image.color = instance.m_UIShieldColor;

        return shield;
    }

    private Animation AddAnimations(Component ui, params string[] animationNames)
    {
        var anim = ui.GetComponent<Animation>();

        if (anim == null)
        {
            anim = ui.gameObject.AddComponent<Animation>();
        }

        anim.playAutomatically = false;

        for (int i = 0; i < animationNames.Length; i++)
        {
            if (anim.GetClip(animationNames[i]) == null)
            {
                anim.AddClip(Resources.Load<AnimationClip>(Path.Combine(m_UiAnimationPath, animationNames[i])), animationNames[i]);
            }
        }

        return anim;
    }

    private void PlayAnimation(Component ui, string animationName, bool destroyUIAtAnimationEnd = false)
    {
        var anim = AddAnimations(ui, animationName);

        anim.Play(animationName);

        if (destroyUIAtAnimationEnd)
        {
            Destroy(ui.gameObject, anim[animationName].length);
        }
    }

    private ScreenController AddScreenController(Component ui)
    {
        var controller = ui.GetComponent<ScreenController>();

        if (controller == null)
        {
            controller = ui.gameObject.AddComponent<ScreenController>();
        }

        return controller;
    }

    private void SetupCameras()
    {
        var cameras = FindObjectsOfType<Camera>();

        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i] != m_BgCamera)
            {
                if (cameras[i].clearFlags == CameraClearFlags.Skybox || cameras[i].clearFlags == CameraClearFlags.SolidColor)
                {
                    m_BgCamera.gameObject.SetActive(false);
                    break;
                }
            }
        }
    }

    private T GetSceneComponent<T>(Scene scene) where T : Component
    {
        var objects = scene.GetRootGameObjects();

        for (int i = 0; i < objects.Length; i++)
        {
            var t = objects[i].GetComponentInChildren<T>();

            if (t != null)
            {
                return t;
            }
        }

        return null;
    }
    #endregion
}
