using UnityEditor;
using UnityEngine;
using System;
using SSManager.IO;
using SSManager.Tool;

namespace SSManager.Manager.Editor
{
    public class TooltipGenerator : EditorWindow
    {
        enum State
        {
            IDLE,
            GENERATING,
            COMPILING,
            COMPILING_AGAIN
        }

        public enum TextType
        {
            Default,
            TextMeshPro
        }

        static class PrefKeys
        {
            public const string SCREEN_SCENE_DIRECTORY_PATH = "SS_SCREEN_SCENE_DIRECTORY_PATH";
            public const string TOOLTIP_SCENE_TEMPLATE_FILE = "TOOLTIP_SCENE_TEMPLATE_FILE";
            public const string SCREEN_WIDTH = "SS_SCREEN_WIDTH";
            public const string SCREEN_HEIGHT = "SS_SCREEN_HEIGHT";
        }

        public string sceneName;
        public string sceneDirectoryPath;
        public string sceneTemplateFile;
        public TextType textType;

        string scenePath;
        string prefabPath;
        string controllerPath;
        State state = State.IDLE;

        [MenuItem("SSManager/Tooltip Generator")]
        public static void ShowWindow()
        {
            TooltipGenerator win = ScriptableObject.CreateInstance<TooltipGenerator>();

            win.minSize = new Vector2(400, 200);
            win.maxSize = new Vector2(400, 200);

            win.ResetParams();
            win.ShowUtility();

            win.LoadPrefs();
        }

        void ResetParams()
        {
            sceneName = string.Empty;
        }

        void LoadPrefs()
        {
            sceneDirectoryPath = EditorPrefs.GetString(PrefKeys.SCREEN_SCENE_DIRECTORY_PATH, "Project/Screens/");
            sceneTemplateFile = EditorPrefs.GetString(PrefKeys.TOOLTIP_SCENE_TEMPLATE_FILE, "TooltipTemplate.prefab");
        }

        void SavePrefs()
        {
            EditorPrefs.SetString(PrefKeys.SCREEN_SCENE_DIRECTORY_PATH, sceneDirectoryPath);
            EditorPrefs.SetString(PrefKeys.TOOLTIP_SCENE_TEMPLATE_FILE, sceneTemplateFile);
        }

        void OnGUI()
        {
            GUILayout.Label("Scene Generator", EditorStyles.boldLabel);
            sceneName = EditorGUILayout.TextField("Tooltip Name", sceneName);
            sceneDirectoryPath = EditorGUILayout.TextField("Screen Directory Path", sceneDirectoryPath);
            sceneTemplateFile = EditorGUILayout.TextField("Tooltip Template File", sceneTemplateFile);

            textType = (TextType)EditorGUILayout.EnumPopup("Text Type", textType);

            switch (state)
            {
                case State.IDLE:
                    if (GUILayout.Button("Generate"))
                    {
                        if (GenerateScene())
                        {
                            state = State.GENERATING;
                        }
                    }
                    break;
                case State.GENERATING:
                    if (EditorApplication.isCompiling)
                    {
                        state = State.COMPILING;
                    }
                    break;
                case State.COMPILING:
                    if (EditorApplication.isCompiling)
                    {
                        EditorApplication.delayCall += () =>
                        {
                            EditorUtility.DisplayProgressBar("Compiling Scripts", "Wait for a few seconds...", 0.33f);
                        };
                    }
                    else
                    {
                        EditorUtility.ClearProgressBar();
                        SetupPrefab();
                        state = State.COMPILING_AGAIN;
                    }
                    break;
                case State.COMPILING_AGAIN:
                    if (EditorApplication.isCompiling)
                    {
                        EditorApplication.delayCall += () =>
                        {
                            EditorUtility.DisplayProgressBar("Compiling Scripts", "Wait for a few seconds...", 0.66f);
                        };
                    }
                    else
                    {
                        state = State.IDLE;
                        EditorUtility.ClearProgressBar();
                        SetupScene();
                        SaveScene();
                        EditorApplication.delayCall += () =>
                        {
                            EditorUtility.DisplayDialog("Successful!", "Tooltip was generated.", "OK");
                        };

                    }
                    break;
            }
        }

        bool GenerateScene()
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogWarning("You have to input an unique name to 'Tooltip Name'");
                return false;
            }

            string targetRelativePath = System.IO.Path.Combine(sceneDirectoryPath, sceneName + "/" + sceneName + ".unity");
            string targetFullPath = Path.GetAbsolutePath(targetRelativePath);

            if (System.IO.File.Exists(targetFullPath))
            {
                Debug.LogWarning("This Tooltip is already exist!");
                return false;
            }

            if (string.IsNullOrEmpty(sceneTemplateFile))
            {
                Debug.LogWarning("You have to input Tooltip template file!");
                return false;
            }

            if (textType == TextType.TextMeshPro)
            {
                var textClass = GetAssemblyType("TMPro.TextMeshProUGUI");
                if (textClass == null)
                {
                    Debug.LogWarning("TextMeshPro package is not installed yet");
                    return false;
                }
            }

            SavePrefs();
            if (!CreatePrefab())
            {
                Debug.LogWarning("Tooltip template file is not exist!");
                return false;
            }

            CreateScene();
            CreateController();
            return true;
        }

        bool CreatePrefab()
        {
            string targetRelativePath = System.IO.Path.Combine(sceneDirectoryPath, sceneName + "/" + sceneName + ".prefab");
            string targetFullPath = File.Copy(sceneTemplateFile, targetRelativePath);

            if (targetFullPath == null)
            {
                return false;
            }

            prefabPath = Path.GetRelativePathWithAssets(targetRelativePath);

            AssetDatabase.ImportAsset(prefabPath);

            return true;
        }

        void SetupPrefab()
        {
            GameObject prefab = PrefabUtility.LoadPrefabContents(prefabPath);

            if (prefab != null)
            {
                var type = GetAssemblyType(sceneName + "Controller");

                prefab.AddComponent(type);

                var text = new GameObject("Text");
                text.transform.SetParent(prefab.transform);

                switch (textType)
                {
                    case TextType.Default:
                        text.AddComponent<UnityEngine.UI.Text>();
                        break;

                    case TextType.TextMeshPro:
                        var textClass = GetAssemblyType("TMPro.TextMeshProUGUI");
                        if (textClass != null)
                        {
                            text.AddComponent(textClass);
                        }
                        break;
                }

                PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);

                PrefabUtility.UnloadPrefabContents(prefab);
            }

            AssetDatabase.ImportAsset(prefabPath);
        }

        void CreateController()
        {
            string targetRelativePath = System.IO.Path.Combine(sceneDirectoryPath, sceneName + "/" + sceneName + "Controller.cs");
            string targetFullPath = File.Copy("TooltipTemplateController.cs", targetRelativePath);

            File.ReplaceFileContent(targetFullPath, "TooltipTemplate", sceneName);

            if (textType == TextType.TextMeshPro)
            {
                File.ReplaceFileContent(targetFullPath, "UnityEngine.UI.Text", "TMPro.TextMeshProUGUI");
            }

            controllerPath = Path.GetRelativePathWithAssets(targetRelativePath);

            AssetDatabase.ImportAsset(controllerPath);
        }

        void CreateScene()
        {
            string targetRelativePath = System.IO.Path.Combine(sceneDirectoryPath, sceneName + "/" + sceneName + ".unity");
            string targetFullPath = File.Copy("ScreenTemplate.unity", targetRelativePath);

            scenePath = Path.GetRelativePathWithAssets(targetRelativePath);

            AssetDatabase.ImportAsset(scenePath);

            Scene.OpenScene(targetFullPath);
        }

        void SetupScene()
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (prefab != null)
            {
                PrefabUtility.InstantiatePrefab(prefab, FindFirstObjectByType<Canvas>().transform);
            }
        }

        void SaveScene()
        {
            Scene.MarkCurrentSceneDirty();
            Scene.SaveScene();
        }

        Type GetAssemblyType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null)
            {
                return type;
            }

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }
    }
}