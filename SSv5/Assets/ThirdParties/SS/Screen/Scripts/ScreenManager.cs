/**
 * @author Anh Pham (Zenga Inc)
 * @email anhpt.csit@gmail.com
 * @date 2024/03/29
 */

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
    [SerializeField] string m_ScreenAnimationPath = "Animations";
    [SerializeField] string m_SceneLoadingName;
    [SerializeField] string m_LoadingName;
    [SerializeField] Color m_ScreenShieldColor = new Color(0, 0, 0, 0.8f);
    [SerializeField] Camera m_BackgroundCamera;
    [SerializeField] Canvas m_Canvas;
    [SerializeField] UnscaledAnimation m_SceneShield;
    #endregion

    #region Delegate
    public delegate void OnSceneLoad<T>(T t);
    public delegate void OnScreenClosed();
    #endregion

    #region Private Member
    private Scene m_LastLoadedScene;
    private List<Component> m_ScreenList = new List<Component>();
    private GameObject m_SceneLoading;
    private GameObject m_Loading;
    private GameObject m_ScreenShield;
    #endregion

    #region Private Static
    private static ScreenManager m_Instance;

    private static ScreenManager instance
    {
        get
        {
            if (m_Instance == null)
            {
                Instantiate(Resources.Load<ScreenManager>("Prefabs/ScreenManager"));
            }

            return m_Instance;
        }
    }
    #endregion

    #region Public Static
    /// <summary>
    /// Get the scene loading operation. Can get some values like % progress.
    /// </summary>
    public static AsyncOperation asyncOperation
    {
        get;
        protected set;
    }

    /// <summary>
    /// Set some basic parameters of ScreenManager.
    /// </summary>
    /// <param name="screenShieldColor">The color of screen shield</param>
    /// <param name="screenPath">The path (in Resources folder) of screen's prefabs</param>
    /// <param name="screenAnimationPath">The path (in Resources folder) of screen's animation clips</param>
    /// <param name="sceneLoadingName">The name of the scene loading screen which is put in 'screenPath'. Set it to empty if you do not want to show the loading screen while loading a scene</param>
    /// <param name="loadingName">The name of the loading screen which is put in 'screenPath'. This screen can show/hide on the top of all screens at any time using Loading(bool). Set it to empty if you don't need</param>
    public static void Set(Color screenShieldColor, string screenPath = "Screens", string screenAnimationPath = "Animations", string sceneLoadingName = "", string loadingName = "")
    {
        instance.Setup(screenShieldColor, screenPath, screenAnimationPath, sceneLoadingName, loadingName);
    }

    /// <summary>
    /// Load a scene.
    /// </summary>
    /// <typeparam name="T">The type of a (any) component in the scene</typeparam>
    /// <param name="sceneName">The name of scene</param>
    /// <param name="mode">The load scene mode. Single or Additive</param>
    /// <param name="onSceneLoaded">The callback when the scene is loaded. [IMPORTANT] It is called after the Awake & OnEnable, before the Start.</param>
    /// <param name="clearAllScreens">Clear all screens when the scene is loaded?</param>
    public static void Load<T>(string sceneName, LoadSceneMode mode, OnSceneLoad<T> onSceneLoaded = null, bool clearAllScreens = true) where T : Component
    {
        instance.LoadScene(sceneName, mode, onSceneLoaded, clearAllScreens);
    }

    /// <summary>
    /// Add a screen on top of all screens. [IMPORTANT] The code after the 'Add' method will be called after the Awake & OnEnable, before the Start.
    /// </summary>
    /// <typeparam name="T">The type of a (any) component in the screen</typeparam>
    /// <param name="screenName">The name of screen</param>
    /// <param name="showAnimation">The name of animation clip (which is put in 'screenAnimationPath') is used to animate the screen to show it</param>
    /// <param name="hideAnimation">The name of animation clip (which is put in 'screenAnimationPath') is used to animate the screen to hide it</param>
    /// <returns>The component type T in the screen.</returns>
    public static T Add<T>(string screenName, string showAnimation = "ScaleShow", string hideAnimation = "ScaleHide", string animationObjectName = "") where T : Component
    {
        return instance.AddScreen<T>(screenName, showAnimation, hideAnimation, animationObjectName);
    }

    /// <summary>
    /// Add a screen to the canvas, on top of all screens. But it's not added to the screen list for managing.
    /// </summary>
    /// <param name="screen">The GameObject of screen</param>
    public static void AddToCanvas(GameObject screen)
    {
        instance.AddScreenToCanvas(screen);
    }

    /// <summary>
    /// Destroy immediately the screen which is at the top of all screens, without playing animation.
    /// </summary>
    public static void Destroy()
    {
        instance.DestroyScreen();
    }

    /// <summary>
    /// Destroy immediately the specific screen, without playing animation.
    /// </summary>
    /// <param name="screen">The component in screen which is returned by the Add function.</param>
    public static void Destroy(Component screen)
    {
        instance.DestroyScreen(screen);
    }

    /// <summary>
    /// Destroy immediately all screens, without playing animation.
    /// </summary>
    public static void DestroyAll()
    {
        instance.ClearAllScreen();
    }

    /// <summary>
    /// Close the screen which is at the top of all screens.
    /// </summary>
    /// <param name="onScreenClosed">The callback when the screen is closed. [IMPORTANT] It is called right after the screen is destroyed.</param>
    /// <param name="hideAnimation">The name of animation clip (which is put in 'screenAnimationPath') is used to animate the screen to hide it. If null, the 'hideAnimation' which is declared in the Add function will be used.</param>
    public static void Close(OnScreenClosed onScreenClosed = null, string hideAnimation = null)
    {
        instance.CloseScreen(onScreenClosed, hideAnimation);
    }

    /// <summary>
    /// Close a specific screen.
    /// </summary>
    /// <param name="screen">The component in screen which is returned by the Add function.</param>
    /// <param name="onScreenClosed">The callback when the screen is closed. [IMPORTANT] It is called right after the screen is destroyed.</param>
    /// <param name="hideAnimation">The name of animation clip (which is put in 'screenAnimationPath') is used to animate the screen to hide it. If null, the 'hideAnimation' which is declared in the Add function will be used.</param>
    public static void Close(Component screen, OnScreenClosed onScreenClosed = null, string hideAnimation = null)
    {
        instance.CloseScreen(screen, onScreenClosed, hideAnimation);
    }

    /// <summary>
    /// Show/Hide the loading screen (which has the name 'loadingName') on top of all screens.
    /// </summary>
    /// <param name="isShow">True if show, False if hide</param>
    public static void Loading(bool isShow)
    {
        instance.ShowLoading(isShow);
    }

    /// <summary>
    /// Remove a specific screen. But don't use this. We use this for an internal purpose.
    /// </summary>
    /// <param name="screen">The component in screen which is returned by Add function.</param>
    public static void RemoveScreen(Component screen)
    {
        instance.RemoveScreenFromList(screen);
    }

    /// <summary>
    /// Hide Shield Or Show Top Screen
    /// </summary>
    public static void HideShieldOrShowTop()
    {
        if (instance != null && instance.isActiveAndEnabled)
        {
            instance.HideScreenShieldOrShowTop();
        }
    }

    /// <summary>
    /// Find child Breadth-First Search
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static GameObject FindChildBFS(GameObject parent, string name)
    {
        Queue<Transform> queue = new Queue<Transform>();

        queue.Enqueue(parent.transform);

        while (queue.Count > 0)
        {
            Transform current = queue.Dequeue();

            foreach (Transform child in current)
            {
                if (child.name == name)
                {
                    return child.gameObject;
                }

                queue.Enqueue(child);
            }
        }

        return null;
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
    private List<Component> screenList
    {
        get
        {
            return m_ScreenList;
        }
    }

    private void Setup(Color screenShieldColor, string screenPath = "Screens", string screenAnimationPath = "Animations", string sceneLoadingName = "", string loadingName = "")
    {
        m_ScreenShieldColor = screenShieldColor;
        m_ScreenPath = screenPath;
        m_ScreenAnimationPath = screenAnimationPath;
        m_SceneLoadingName = sceneLoadingName;
        m_LoadingName = loadingName;
    }

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

            yield return new WaitForSecondsRealtime(m_SceneShield.GetLength("ShieldShow"));

            if (clearAllScreen)
            {
                if (m_ScreenShield != null)
                {
                    Destroy(m_ScreenShield);
                    m_ScreenShield = null;
                }

                ClearAllScreen();
            }
        }

        if (mode == LoadSceneMode.Single && !string.IsNullOrEmpty(m_SceneLoadingName))
        {
            if (m_SceneLoading == null)
            {
                m_SceneLoading = Instantiate(Resources.Load<GameObject>(Path.Combine(m_ScreenPath, m_SceneLoadingName)));
                m_SceneLoading.name = m_SceneLoadingName;
                AddScreenToCanvas(m_SceneLoading);
            }

            m_SceneLoading.transform.SetAsLastSibling();
            m_SceneLoading.SetActive(true);
        }

        asyncOperation = SceneManager.LoadSceneAsync(sceneName, mode);
        asyncOperation.completed += (asyncOp) =>
        {
            onSceneLoaded?.Invoke(GetSceneComponent<T>(instance.m_LastLoadedScene));
        };

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        if (mode == LoadSceneMode.Single)
        {
            m_SceneShield.Play("ShieldHide");
        }

        if (m_SceneLoading != null)
        {
            yield return 0;

            m_SceneLoading.SetActive(false);
        }
    }

    private T AddScreen<T>(string screenName, string showAnimation = "ScaleShow", string hideAnimation = "ScaleHide", string animationObjectName = "") where T : Component
    {
        if (m_ScreenShield == null)
        {
            m_ScreenShield = CreateShield();
        }

        if (m_ScreenList.Count > 0)
        {
            var topScreen = m_ScreenList[m_ScreenList.Count - 1];
            topScreen.gameObject.SetActive(false);
        }

        var screen = Instantiate(Resources.Load<T>(Path.Combine(m_ScreenPath, screenName)), m_Canvas.transform);
        screen.name = screenName;
        AddScreenToCanvas(screen.gameObject);

        var controller = AddScreenController(screen);
        controller.screen = screen;
        controller.showAnimation = showAnimation;
        controller.hideAnimation = hideAnimation;
        controller.animationObjectName = animationObjectName;

        AddAnimations(screen, animationObjectName, showAnimation, hideAnimation);
        PlayAnimation(screen, showAnimation, 4);

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
            PlayAnimation(screen, hideAnimation, 0, true, onScreenClosed);
        }
    }

    private void ClearAllScreen()
    {
        while (m_ScreenList.Count > 0)
        {
            var screen = m_ScreenList[0];
            m_ScreenList.RemoveAt(0);

            DestroyScreen(screen);
        }
    }

    private void DestroyScreen()
    {
        if (m_ScreenList.Count > 0)
        {
            var screen = m_ScreenList[m_ScreenList.Count - 1];

            DestroyScreen(screen);
        }
    }

    private void DestroyScreen(Component screen)
    {
        if (screen != null && screen.gameObject != null)
        {
            Destroy(screen.gameObject);
        }
    }

    private void ShowLoading(bool isShow)
    {
        if (isShow)
        {
            if (!string.IsNullOrEmpty(m_LoadingName))
            {
                if (m_Loading == null)
                {
                    m_Loading = Instantiate(Resources.Load<GameObject>(Path.Combine(m_ScreenPath, m_LoadingName)));
                    m_Loading.name = m_LoadingName;
                    AddScreenToCanvas(m_Loading);
                }

                m_Loading.transform.SetAsLastSibling();
                m_Loading.SetActive(true);
            }
        }
        else
        {
            if (m_Loading != null)
            {
                m_Loading.SetActive(false);
            }
        }
    }

    private void AddScreenToCanvas(GameObject screen)
    {
        screen.transform.SetParent(m_Canvas.transform);
        screen.transform.localPosition = Vector3.zero;
        screen.transform.localScale = Vector3.one;
        screen.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
    }

    private void RemoveScreenFromList(Component screen)
    {
        if (m_ScreenList.Contains(screen))
        {
            m_ScreenList.Remove(screen);
        }
    }

    private GameObject CreateShield()
    {
        var shield = Instantiate(Resources.Load<GameObject>("Prefabs/Shield"), m_Canvas.transform);
        shield.name = "Screen Shield";

        shield.GetComponent<UnscaledAnimation>().Play("ShieldShow");

        var image = shield.GetComponent<Image>();
        image.color = instance.m_ScreenShieldColor;

        return shield;
    }

    private Animation AddAnimations(Component screen, string animationObjectName = "", params string[] animationNames)
    {
        GameObject animObject = screen.gameObject;

        if (!string.IsNullOrEmpty(animationObjectName))
        {
            animObject = FindChildBFS(screen.gameObject, animationObjectName);

            if (animObject == null)
            {
                animObject = screen.gameObject;
            }
        }

        var anim = animObject.GetComponent<Animation>();
        if (anim == null)
        {
            anim = animObject.AddComponent<Animation>();
        }

        var unscaledAnim = animObject.GetComponent<UnscaledAnimation>();
        if (unscaledAnim == null)
        {
            animObject.AddComponent<UnscaledAnimation>();
        }

        anim.playAutomatically = false;

        for (int i = 0; i < animationNames.Length; i++)
        {
            if (!string.IsNullOrEmpty(animationNames[i]))
            {
                if (anim.GetClip(animationNames[i]) == null)
                {
                    var path = Path.Combine(m_ScreenAnimationPath, animationNames[i]);
                    var clip = Resources.Load<AnimationClip>(path);

                    if (clip == null)
                    {
                        var defaultPath = Path.Combine("Animations", animationNames[i]);
                        clip = Resources.Load<AnimationClip>(defaultPath);
                    }

                    if (clip != null)
                    {
                        anim.AddClip(clip, animationNames[i]);
                    }
                    else
                    {
                        Debug.LogWarning("Animation Clip not found: " + path);
                    }
                }

                switch (animationNames[i])
                {
                    case "FadeShow":
                    case "FadeHide":
                        if (animObject.GetComponent<CanvasGroup>() == null)
                        {
                            animObject.AddComponent<CanvasGroup>();
                        }
                        break;
                    case "RightShow":
                    case "LeftShow":
                    case "TopShow":
                    case "BottomShow":
                    case "RightHide":
                    case "LeftHide":
                    case "TopHide":
                    case "BottomHide":
                        if (animObject.GetComponent<AnimationPosition>() == null)
                        {
                            animObject.AddComponent<AnimationPosition>();
                        }
                        break;
                }
            }
        }

        return anim;
    }

    private void PlayAnimation(Component screen, string animationName, int delayFrames = 0, bool destroyScreenAtAnimationEnd = false, OnScreenClosed onScreenClosed = null)
    {
        var anim = AddAnimations(screen, screen.GetComponent<ScreenController>().animationObjectName, animationName);
        var animLength = 0f;

        if (anim.GetClip(animationName) != null)
        {
            StartCoroutine(CoPlayAnimation(anim, animationName, delayFrames));
            animLength = anim[animationName].length;
        }

        if (destroyScreenAtAnimationEnd)
        {
            StartCoroutine(DestroyScreen(screen, animLength, onScreenClosed));
        }
    }

    private IEnumerator CoPlayAnimation(Animation anim, string animationName, int delayFrames)
    {
        var unscaledAnim = anim.GetComponent<UnscaledAnimation>();
        unscaledAnim.PauseAtBeginning(animationName);

        var animRepos = anim.GetComponent<AnimationPosition>();
        if (animRepos != null)
        {
            animRepos.Reposition();
        }

        for (int i = 0; i < delayFrames; i++)
        {
            yield return 0;
        }

        unscaledAnim.Play(animationName);
    }

    private IEnumerator DestroyScreen(Component screen, float delay, OnScreenClosed onScreenClosed = null)
    {
        yield return new WaitForSecondsRealtime(delay);

        DestroyScreen(screen);

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

    private void HideScreenShieldOrShowTop()
    {
        if (m_ScreenList.Count == 0)
        {
            if (m_ScreenShield != null)
            {
                var anim = m_ScreenShield.GetComponent<UnscaledAnimation>();
                anim.Play("ShieldHide", anim => {
                    Destroy(m_ScreenShield);
                    m_ScreenShield = null;
                });
            }
        }
        else
        {
            var topScreen = m_ScreenList[m_ScreenList.Count - 1];

            if (!topScreen.gameObject.activeInHierarchy)
            {
                topScreen.gameObject.SetActive(true);

                PlayAnimation(topScreen, topScreen.GetComponent<ScreenController>().showAnimation);
            }
        }
    }
    #endregion
}
