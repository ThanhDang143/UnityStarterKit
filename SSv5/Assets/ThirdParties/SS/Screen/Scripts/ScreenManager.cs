using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    #region SerializeField
    [SerializeField] string m_ScreenPath = "Screens";
    [SerializeField] string m_ScreenAnimationPath = "Screens/Animations";
    [SerializeField] Camera m_BackgroundCamera;
    [SerializeField] Canvas m_Canvas;
    [SerializeField] Animation m_SceneShield;
    [SerializeField] Color m_ScreenShieldColor = new Color(0, 0, 0, 0.8f);
    #endregion

    #region Delegate
    public delegate void OnSceneLoad<T>(T t);
    public delegate void OnScreenClosed();
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

    public static void Load<T>(string sceneName, LoadSceneMode mode, OnSceneLoad<T> onSceneLoaded = null, bool clearAllScreen = true) where T : Component
    {
        instance.LoadScene(sceneName, mode, onSceneLoaded, clearAllScreen);
    }

    public static T Add<T>(string screenName, string showAnimation = "ScaleShow", string hideAnimation = "ScaleHide") where T : Component
    {
        return instance.AddScreen<T>(screenName, showAnimation, hideAnimation);
    }

    public static void Close(OnScreenClosed onScreenClosed = null, string hideAnimation = null)
    {
        instance.CloseScreen(onScreenClosed, hideAnimation);
    }

    public static void Close(Component screen, OnScreenClosed onScreenClosed = null, string hideAnimation = null)
    {
        instance.CloseScreen(screen, onScreenClosed, hideAnimation);
    }

    public static void CloseAll()
    {
        instance.ClearAllScreen();
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

            name = "ScreenManager";

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
        m_BackgroundCamera.gameObject.SetActive(true);
    }

    private void SceneManager_activeSceneChanged(Scene scene1, Scene scene2)
    {
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupCameras();

        m_LastLoadedScene = scene;
    }
    #endregion

    #region Private Functions
    private void LoadScene<T>(string sceneName, LoadSceneMode mode, OnSceneLoad<T> onSceneLoaded = null, bool clearAllScreen = true) where T : Component
    {
        StartCoroutine(CoLoadScene(sceneName, mode, onSceneLoaded, clearAllScreen));
    }

    private IEnumerator CoLoadScene<T>(string sceneName, LoadSceneMode mode, OnSceneLoad<T> onSceneLoaded = null, bool clearAllScreen = true) where T : Component
    {
        m_SceneShield.transform.SetAsLastSibling();

        if (mode == LoadSceneMode.Single)
        {
            m_SceneShield.Play("ShieldShow");

            yield return new WaitForSeconds(m_SceneShield["ShieldShow"].length);
        }

        if (clearAllScreen)
        {
            ClearAllScreen();
        }

        var asyncOperation = SceneManager.LoadSceneAsync(sceneName, mode);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        if (mode == LoadSceneMode.Single)
        {
            m_SceneShield.Play("ShieldHide");
        }

        onSceneLoaded?.Invoke(GetSceneComponent<T>(instance.m_LastLoadedScene));
    }

    private T AddScreen<T>(string screenName, string showAnimation = "ScaleShow", string hideAnimation = "ScaleHide") where T : Component
    {
        var shield = CreateShield();

        var screen = Instantiate(Resources.Load<T>(Path.Combine(m_ScreenPath, screenName)), m_Canvas.transform);
        screen.name = screenName;
        screen.transform.localPosition = Vector3.zero;
        screen.transform.localScale = Vector3.one;
        screen.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        var controller = AddScreenController(screen);
        controller.shield = shield;
        controller.showAnimation = showAnimation;
        controller.hideAnimation = hideAnimation;

        AddAnimations(screen, showAnimation, hideAnimation);
        PlayAnimation(screen, showAnimation);

        m_ScreenList.Add(screen);

        return screen;
    }

    private void CloseScreen(OnScreenClosed onScreenClosed = null, string hideAnimation = null)
    {
        if (m_ScreenList.Count > 0)
        {
            var screen = m_ScreenList[m_ScreenList.Count - 1];
            CloseScreen(screen, onScreenClosed, hideAnimation);
        }
    }

    private void CloseScreen(Component screen, OnScreenClosed onScreenClosed = null, string hideAnimation = null)
    {
        if (m_ScreenList.Count > 0)
        {
            m_ScreenList.Remove(screen);

            hideAnimation = (hideAnimation != null) ? hideAnimation : screen.GetComponent<ScreenController>().hideAnimation;
            PlayAnimation(screen, hideAnimation, true, onScreenClosed);
        }
    }

    private void ClearAllScreen()
    {
        while (m_ScreenList.Count > 0)
        {
            var screen = m_ScreenList[0];
            m_ScreenList.RemoveAt(0);

            Destroy(screen.gameObject);
        }
    }

    private GameObject CreateShield()
    {
        var shield = new GameObject("Screen Shield", typeof(Image));

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
        image.color = instance.m_ScreenShieldColor;

        return shield;
    }

    private Animation AddAnimations(Component screen, params string[] animationNames)
    {
        var anim = screen.GetComponent<Animation>();

        if (anim == null)
        {
            anim = screen.gameObject.AddComponent<Animation>();
        }

        anim.playAutomatically = false;

        for (int i = 0; i < animationNames.Length; i++)
        {
            if (anim.GetClip(animationNames[i]) == null)
            {
                anim.AddClip(Resources.Load<AnimationClip>(Path.Combine(m_ScreenAnimationPath, animationNames[i])), animationNames[i]);
            }
        }

        return anim;
    }

    private void PlayAnimation(Component screen, string animationName, bool destroyScreenAtAnimationEnd = false, OnScreenClosed onScreenClosed = null)
    {
        var anim = AddAnimations(screen, animationName);

        anim.Play(animationName);

        if (destroyScreenAtAnimationEnd)
        {
            StartCoroutine(DestroyScreen(screen, anim[animationName].length, onScreenClosed));
        }
    }

    private IEnumerator DestroyScreen(Component screen, float delay, OnScreenClosed onScreenClosed = null)
    {
        yield return new WaitForSeconds(delay);

        if (screen != null && screen.gameObject != null)
        {
            Destroy(screen.gameObject);
        }

        onScreenClosed?.Invoke();
    }

    private ScreenController AddScreenController(Component screen)
    {
        var controller = screen.GetComponent<ScreenController>();

        if (controller == null)
        {
            controller = screen.gameObject.AddComponent<ScreenController>();
        }

        return controller;
    }

    private void SetupCameras()
    {
        var cameras = FindObjectsOfType<Camera>();

        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i] != m_BackgroundCamera)
            {
                if (cameras[i].clearFlags == CameraClearFlags.Skybox || cameras[i].clearFlags == CameraClearFlags.SolidColor)
                {
                    m_BackgroundCamera.gameObject.SetActive(false);
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
