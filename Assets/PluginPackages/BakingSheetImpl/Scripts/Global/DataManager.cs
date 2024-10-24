using System.Collections.Generic;
using System.Linq;
using Cathei.BakingSheet.Unity;
using Cathei.LinqGen;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BakingSheetImpl
{
    public class DataManager
    {
        private static DataManager _instance;
        public static DataManager Instance => _instance ??= new DataManager();

        private SheetContainer container;

        private Dictionary<string, IDataModel> allData;

        public async void InitialData()
        {
            allData = new Dictionary<string, IDataModel>();

            AsyncOperationHandle<SheetContainerScriptableObject> loadContainerHandle = Addressables.LoadAssetAsync<SheetContainerScriptableObject>(DataConstant.SHEET_CONTAINE_SO_ADDRESS);
            await loadContainerHandle.Task;
            SheetContainerScriptableObject containerSO = loadContainerHandle.Result;
            ScriptableObjectSheetImporter importer = new(containerSO);

            container = new SheetContainer();
            await container.Bake(importer);

            allData = new(container.CacheData());
        }

        /// <summary>
        /// Get a data record by ID.
        /// </summary>
        /// <typeparam name="T">Type of Data.</typeparam>
        /// <param name="id">ID of data.</param>
        /// <returns>Return a record data with type T</returns>
        public T GetData<T>(string id) where T : class
        {
            if (allData != null && allData.ContainsKey(id) && allData[id] is T)
            {
                return allData[id] as T;
            }

            Debug.LogError("Cannot Find item ID " + id);
            return null;
        }

        /// <summary>
        /// Get all data is T.
        /// </summary>
        /// <typeparam name="T">Type of data want to get.</typeparam>
        /// <returns>Return a List contain all data is T.</returns>
        public List<T> GetDatas<T>() where T : class
        {
            return new List<T>(allData.Where(d => d.Value is T).Cast<T>());
        }
    }
}
