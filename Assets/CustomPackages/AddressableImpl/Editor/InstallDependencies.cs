using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace AddressableImpl
{
    public class InstallDependencies
    {
        private static Dictionary<string, string> packageDependencies = new Dictionary<string, string>
        {
            { "com.unity.addressables@2.3.1", "" }
        };

        private const string defineSymbol = "ThanhDV_AddressableImpl";

        [MenuItem("Tools/Addressable Impl/Initialize")]
        public static void ForceInitializeDependencies()
        {
            foreach (var package in packageDependencies)
            {
                _ = CheckDependenciesPackageInstalled(package.Key);
            }

            AddDefineSymbol(defineSymbol);
            AssetDatabase.Refresh();
        }

        private static NamedBuildTarget GetCurrentNamedBuildTarget()
        {
            BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;

            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                case BuildTarget.StandaloneOSX:
                case BuildTarget.StandaloneLinux64:
                    return NamedBuildTarget.Standalone;

                case BuildTarget.Android:
                    return NamedBuildTarget.Android;

                case BuildTarget.iOS:
                    return NamedBuildTarget.iOS;

                case BuildTarget.WebGL:
                    return NamedBuildTarget.WebGL;

                case BuildTarget.PS4:
                    return NamedBuildTarget.PS4;

                case BuildTarget.XboxOne:
                    return NamedBuildTarget.XboxOne;

                case BuildTarget.tvOS:
                    return NamedBuildTarget.tvOS;

                case BuildTarget.Switch:
                    return NamedBuildTarget.NintendoSwitch;

                case BuildTarget.PS5:
                    return NamedBuildTarget.PS5;

                default:
                    Debug.Log("<color=red>Unsupported build target: " + buildTarget + "</color>");
                    return NamedBuildTarget.Standalone;
            }
        }
        private static void AddDefineSymbol(params string[] symbols)
        {
            NamedBuildTarget namedBuildTarget = GetCurrentNamedBuildTarget();
            string defines = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);
            string[] curSymbols = defines.Split(';');

            foreach (var symbol in symbols)
            {
                if (!curSymbols.Contains(symbol))
                {
                    ArrayUtility.AddRange(ref symbols, new[] { symbol });
                }
            }

            defines = string.Join(";", symbols);
            PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, defines);
        }

        private static async Task InstallDependenciesPackage(string packageName)
        {
            string gitURL = packageDependencies[packageName];
            AddRequest request = Client.Add(string.IsNullOrEmpty(gitURL) ? packageName : gitURL);
            float timeOut = 10f;

            while (!request.IsCompleted && timeOut > 0)
            {
                timeOut -= Time.deltaTime;
                if (timeOut <= 0)
                {
                    Debug.Log($"<color=red>Check {packageName} time out!</color>");
                    return;
                }
                int delay = (int)(Time.deltaTime * 1000);
                await Task.Delay(delay);
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
            float timeOut = 10f;

            while (!request.IsCompleted && timeOut > 0)
            {
                timeOut -= Time.deltaTime;
                if (timeOut <= 0)
                {
                    Debug.Log($"<color=red>Check {packageName} time out!</color>");
                    return;
                }
                int delay = (int)(Time.deltaTime * 1000);
                await Task.Delay(delay);
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
    }
}