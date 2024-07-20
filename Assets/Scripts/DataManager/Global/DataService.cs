using Cathei.BakingSheet.Unity;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using ZBase.Collections.Pooled.Generic;

namespace DataManager
{
    public class DataService
    {
        private static DataService _instance;
        public static DataService Instance => _instance ??= new DataService();

        private SheetContainer container;

        private Dictionary<string, IDataModel> allData;

        public async void InitialData()
        {
            allData = new Dictionary<string, IDataModel>();

            AsyncOperationHandle<SheetContainerScriptableObject> loadContainerHandle = Addressables.LoadAssetAsync<SheetContainerScriptableObject>(Data.SHEET_CONTAINE_SO_ADDRESS);
            await loadContainerHandle.Task;
            SheetContainerScriptableObject containerSO = loadContainerHandle.Result;
            ScriptableObjectSheetImporter importer = new(containerSO);

            container = new SheetContainer();
            await container.Bake(importer);

            container.Demo.ForEach(d => allData.Add(d.Id, d));
        }

        public T GetDataById<T>(string id) where T : class
        {
            if (allData != null && allData.ContainsKey(id) && allData[id] is T)
            {
                return allData[id] as T;
            }

            Debug.LogError("Cannot Find item ID " + id);
            return null;
        }
    }
}
