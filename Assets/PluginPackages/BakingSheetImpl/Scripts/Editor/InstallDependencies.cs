using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace BakingSheetImpl
{
    [InitializeOnLoad]
    public class InstallDependencies
    {
        static bool isInitialized = false;

        static Dictionary<string, string> packageDependencies = new Dictionary<string, string>
        {
            { "com.cathei.bakingsheet", "https://github.com/cathei/BakingSheet.git?path=UnityProject/Packages/com.cathei.bakingsheet" }
        };

        static InstallDependencies()
        {
            InitializeDependencies();
        }

        public static void InitializeDependencies()
        {
            if (isInitialized) return;

            ForceInitializeDependencies();
        }

        [MenuItem("Tools/BakingSheet Impl/Initialize")]
        public static void ForceInitializeDependencies()
        {
            isInitialized = true;
            foreach (var package in packageDependencies)
            {
                _ = CheckDependenciesPackageInstalled(package.Key);
            }

            UncommentDataEditor();
        }

        private static async Task InstallDependenciesPackage(string packageName)
        {
            string gitURL = packageDependencies[packageName];
            AddRequest request = Client.Add(gitURL);
            float timeOut = 5f;

            while (!request.IsCompleted && timeOut > 0)
            {
                timeOut -= Time.deltaTime;
                await Task.Delay((int)Time.deltaTime * 1000);
            }

            if (request.Status == StatusCode.Success)
            {
                Debug.Log($"<color=green>Install {packageName} success</color>");
            }
            else if (request.Status >= StatusCode.Failure)
            {
                Debug.Log($"<color=red>Install {packageName} fail with error {request.Error.message}</color>");
            }
        }

        public static async Task CheckDependenciesPackageInstalled(string packageName)
        {
            ListRequest request = Client.List();
            float timeOut = 5f;

            while (!request.IsCompleted && timeOut > 0)
            {
                timeOut -= Time.deltaTime;
                await Task.Delay((int)Time.deltaTime * 1000);
            }

            if (request.Status == StatusCode.Success)
            {
                if (request.Result.Any(package => package.name == packageName))
                {
                    Debug.Log($"<color=green>{packageName} is installed</color>");
                }
                else
                {
                    _ = InstallDependenciesPackage(packageName);
                }
            }
            else if (request.Status >= StatusCode.Failure)
            {
                Debug.Log($"<color=red>Check {packageName} fail with error {request.Error.message}</color>");
            }
        }

        private static void UncommentDataEditor()
        {
            string dataEditorPath = FindScriptPath("DataEditor");
            if (string.IsNullOrEmpty(dataEditorPath)) return;

            string[] dataEditorContent = System.IO.File.ReadAllLines(dataEditorPath);
            List<string> newContent = new List<string>(dataEditorContent);

            if (newContent[0].Equals("/*") || string.IsNullOrEmpty(newContent[0])) newContent.RemoveAt(0);
            if (newContent[^1].Equals("*/") || string.IsNullOrEmpty(newContent[^1])) newContent.RemoveAt(newContent.Count - 1);

            System.IO.File.WriteAllLines(dataEditorPath, newContent.ToArray());
            AssetDatabase.Refresh();
        }

        private static string FindScriptPath(string scriptName)
        {
            string[] guids = AssetDatabase.FindAssets(scriptName + " t:Script");
            if (guids.Length <= 0)
            {
                Debug.LogError($"<color=red>Cannot find {scriptName}.cs in the project</color>");
                return string.Empty;
            }

            return AssetDatabase.GUIDToAssetPath(guids[0]);
        }
    }
}
