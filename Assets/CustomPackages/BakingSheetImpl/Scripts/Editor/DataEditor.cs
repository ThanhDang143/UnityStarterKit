#if ThanhDV_BakingSheetImpl

using System.Threading.Tasks;
using Cathei.BakingSheet;
using Cathei.BakingSheet.Unity;
using DG.DemiEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace BakingSheetImpl
{
    public class DataEditor : OdinEditorWindow
    {
        [FolderPath, ShowInInspector, Required] public static string excelPath = "Assets/PluginPackages/BakingSheetImpl/RawData";
        [FolderPath, ShowInInspector, Required] public static string scriptableObjectPath = "Assets/PluginPackages/BakingSheetImpl/ScriptableObjectData";

        private bool showMessage = false;
        private string message;
        private Color messageColor;

        [MenuItem("Tools/BakingSheet Impl/Manager %g")]
        private static void Open()
        {
            DataEditor window = GetWindow<DataEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(600, 600);
        }

        private void OnGUI()
        {
            if (showMessage)
            {
                GUILayout.FlexibleSpace();

                Color originalColor = GUI.color;
                GUI.color = messageColor;

                GUIStyle style = new();
                style.fontSize = 20;
                style.fontStyle = FontStyle.Bold;
                style.alignment = TextAnchor.MiddleCenter;
                style.normal.textColor = messageColor;
                style.padding = new RectOffset(0, 0, 0, 50);

                EditorGUILayout.LabelField(message, style);
                GUI.color = originalColor;
            }
        }

        [Button("Update Data")]
        public async void UpdateDataExcel()
        {
            if (excelPath.IsNullOrEmpty() || scriptableObjectPath.IsNullOrEmpty())
            {
                Debug.LogError("Please set excel path and scriptable object path");
                return;
            }

            SheetContainer sheetContainer = new();

            ExcelSheetConverter sheetConverter = new(excelPath);

            await sheetContainer.Bake(sheetConverter);

            ScriptableObjectSheetExporter exporter = new(scriptableObjectPath);

            await sheetContainer.Store(exporter);

            AssetDatabase.Refresh();

            ShowMessage("Convert data completed!!! Check console for detail!!!", 2f, Color.white);
        }

        private async void ShowMessage(string _message, float duration, Color color)
        {
            message = _message;
            showMessage = true;
            messageColor = color;
            await Task.Delay((int)duration * 1000);
            showMessage = false;
        }
    }
}

#endif