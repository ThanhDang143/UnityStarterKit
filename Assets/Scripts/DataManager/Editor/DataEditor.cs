using Cathei.BakingSheet;
using Cathei.BakingSheet.Unity;
using DG.DemiEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace DataManager
{
    public class DataEditor : OdinEditorWindow
    {
        [FolderPath, ShowInInspector, Required] public static string excelPath = "Assets/Database/RawData";
        [FolderPath, ShowInInspector, Required] public static string scriptableObjectPath = "Assets/Database/ScriptableObjectData";

        [MenuItem("Tools/Data Manager %g")]
        private static void Open()
        {
            DataEditor window = GetWindow<DataEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(600, 600);
        }

        [Button("Update Data")]
        public static async void UpdateDataExcel()
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

            Debug.Log("Convert data completed!!!", exporter.Result);
        }
    }
}
