using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

[InitializeOnLoad]
public class InitDependencies
{
    static Dictionary<string, string> packageDependencies = new Dictionary<string, string>
    {
        { "com.cathei.bakingsheet", "https://github.com/cathei/BakingSheet.git?path=UnityProject/Packages/com.cathei.bakingsheet" }
    };

    static InitDependencies()
    {
        foreach (var package in packageDependencies)
        {
            _ = CheckDependenciesPackageInstalled(package.Key);
        }
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

    private static async Task CheckDependenciesPackageInstalled(string packageName)
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
}
