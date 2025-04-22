using UnityEditor;
using UnityEngine;
using System;
using SSManager.IO;
using SSManager.Tool;
using UnityEngine.UI;

namespace SSManager.Manager.Editor
{
    public class ScreenGenerator : EditorWindow
    {
        enum State
        {
            IDLE,
            GENERATING,
            COMPILING,
            COMPILING_AGAIN
        }

        private const int DEFAULT_SCREEN_WIDTH = 1080;
        private const int DEFAULT_SCREEN_HEIGHT = 1920;

        static class PrefKeys
        {
            public const string SCREEN_SCENE_DIRECTORY_PATH = "SS_SCREEN_SCENE_DIRECTORY_PATH";
            public const string SCREEN_SCENE_TEMPLATE_FILE = "SS_SCREEN_SCENE_TEMPLATE_FILE";
            public const string SCREEN_WIDTH = "SS_SCREEN_WIDTH";
            public const string SCREEN_HEIGHT = "SS_SCREEN_HEIGHT";
        }

        public string sceneName;
        public string sceneDirectoryPath;
        public string sceneTemplateFile;

        public string screenCanvasPath;
        public Vector2Int sceneSize = new Vector2Int(DEFAULT_SCREEN_WIDTH, DEFAULT_SCREEN_HEIGHT);


        string scenePath;
        string prefabPath;
        string controllerPath;
        State state = State.IDLE;

        [MenuItem("SSManager/Screen Generator")]
        public static void ShowWindow()
        {
            ScreenGenerator win = ScriptableObject.CreateInstance<ScreenGenerator>();

            win.titleContent = new GUIContent("Screen Generator");
            win.minSize = new Vector2(400, 250);
            win.maxSize = new Vector2(400, 250);

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
            sceneDirectoryPath = EditorPrefs.GetString(PrefKeys.SCREEN_SCENE_DIRECTORY_PATH, "Screens/");
            sceneTemplateFile = EditorPrefs.GetString(PrefKeys.SCREEN_SCENE_TEMPLATE_FILE, "ScreenTemplate.prefab");
            int screenWidth = EditorPrefs.GetInt(PrefKeys.SCREEN_WIDTH, DEFAULT_SCREEN_WIDTH);
            int screenHeight = EditorPrefs.GetInt(PrefKeys.SCREEN_HEIGHT, DEFAULT_SCREEN_HEIGHT);
            sceneSize = new Vector2Int(screenWidth, screenHeight);
        }

        void SavePrefs()
        {
            EditorPrefs.SetString("SS_SCREEN_SCENE_DIRECTORY_PATH", sceneDirectoryPath);
            EditorPrefs.SetString("SS_SCREEN_SCENE_TEMPLATE_FILE", sceneTemplateFile);
            EditorPrefs.SetInt("SS_SCREEN_WIDTH", sceneSize.x);
            EditorPrefs.SetInt("SS_SCREEN_HEIGHT", sceneSize.y);
        }

        void OnGUI()
        {
            #region Screen Settings
            GUILayout.Label("Screen Size", EditorStyles.boldLabel);
            ShowScreenSize();

            if (GUILayout.Button("Update"))
            {
                if (sceneSize.x > 0 && sceneSize.y > 0)
                {
                    SavePrefs();
                    EditScreenCanvas();
                    GameWindow.Resize(sceneSize.x, sceneSize.y);
                    Close();
                }
            }
            #endregion

            #region Scene Generator 
            GUILayout.Space(10);
            GUILayout.Label("Screen Generator", EditorStyles.boldLabel);
            sceneName = EditorGUILayout.TextField("Screen Name", sceneName);
            ShowScreenDirectoryPath();
            ShowScreenTemplateFile();

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
                            EditorUtility.DisplayDialog("Successful!", "Screen was generated.", "OK");
                        };

                    }
                    break;
            }
            #endregion

            #region Clear Settings
            GUILayout.Space(10);
            GUILayout.Label("Clear Settings", EditorStyles.boldLabel);
            if (GUILayout.Button("Clear"))
            {
                EditorPrefs.DeleteAll();
                LoadPrefs();
            }
            #endregion
        }

        private void ShowScreenDirectoryPath()
        {
            EditorGUILayout.BeginHorizontal();
            sceneDirectoryPath = EditorGUILayout.TextField("Screen Directory Path", sceneDirectoryPath);
            if (GUILayout.Button(EditorGUIUtility.IconContent("Folder Icon"), GUILayout.Width(24), GUILayout.Height(18)))
            {
                string path = EditorUtility.OpenFolderPanel("Select Screen Directory", Application.dataPath, "cs");
                if (!string.IsNullOrEmpty(path))
                {
                    if (path.StartsWith(Application.dataPath))
                        sceneDirectoryPath = "Assets" + path.Substring(Application.dataPath.Length);
                    else
                        sceneDirectoryPath = path;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void ShowScreenTemplateFile()
        {
            EditorGUILayout.BeginHorizontal();
            sceneTemplateFile = EditorGUILayout.TextField("Screen Template File", sceneTemplateFile);
            if (GUILayout.Button(EditorGUIUtility.IconContent("Folder Icon"), GUILayout.Width(24), GUILayout.Height(18)))
            {
                string path = EditorUtility.OpenFilePanel("Select Screen Template Prefab", Application.dataPath, "prefab");
                if (!string.IsNullOrEmpty(path))
                {
                    if (path.StartsWith(Application.dataPath))
                        sceneTemplateFile = "Assets" + path.Substring(Application.dataPath.Length);
                    else
                        sceneTemplateFile = path;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void ShowScreenSize()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Screen Size", GUILayout.Width(EditorGUIUtility.labelWidth));
            sceneSize.x = EditorGUILayout.IntField(sceneSize.x, GUILayout.MinWidth(50));
            GUILayout.Label("x", GUILayout.Width(10));
            sceneSize.y = EditorGUILayout.IntField(sceneSize.y, GUILayout.MinWidth(50));
            EditorGUILayout.EndHorizontal();
        }


        void EditScreenCanvas()
        {
            string prefabPath = screenCanvasPath;

            var prefabInstance = PrefabUtility.LoadPrefabContents(prefabPath);

            var canvasScaler = prefabInstance.GetComponentInChildren<CanvasScaler>();
            canvasScaler.referenceResolution = new Vector2(sceneSize.x, sceneSize.y);

            PrefabUtility.SaveAsPrefabAsset(prefabInstance, prefabPath);

            PrefabUtility.UnloadPrefabContents(prefabInstance);
        }

        bool GenerateScene()
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.Log("<color=yellow>You have to input an unique name to 'Screen Name'</color>");
                return false;
            }

            string targetRelativePath = System.IO.Path.Combine(sceneDirectoryPath, sceneName + "/" + sceneName + ".unity");
            string targetFullPath = Path.GetAbsolutePath(targetRelativePath);

            if (System.IO.File.Exists(targetFullPath))
            {
                Debug.Log("<color=yellow>This screen is already exist!</color>");
                return false;
            }

            if (string.IsNullOrEmpty(sceneTemplateFile))
            {
                Debug.Log("<color=yellow>You have to input screen template file!</color>");
                return false;
            }

            SavePrefs();
            if (!CreatePrefab())
            {
                Debug.Log("<color=yellow>Screen template file is not exist!</color>");
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
                var type = GetAssemblyType($"SSManager.Manager.Template.{sceneName}Controller");

                prefab.AddComponent(type);

                PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);

                PrefabUtility.UnloadPrefabContents(prefab);
            }

            AssetDatabase.ImportAsset(prefabPath);
        }

        void CreateController()
        {
            string targetRelativePath = System.IO.Path.Combine(sceneDirectoryPath, sceneName + "/" + sceneName + "Controller.cs");
            string targetFullPath = File.Copy("ScreenTemplateController.cs", targetRelativePath);

            File.ReplaceFileContent(targetFullPath, "ScreenTemplate", sceneName);

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