using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AddressableImpl
{
    public class AddressableManager
    {
        /// <summary>
        /// Load only one asset.
        /// </summary>
        /// <typeparam name="T">Type of assets.</typeparam>
        /// <param name="address">Address of asset.</param>
        /// <param name="onComplete">Action on load complete.</param>
        /// <returns></returns>
        public T LoadAssetAsync<T>(string address, System.Action<T> onComplete = null) where T : Object
        {
            if (string.IsNullOrEmpty(address) || string.IsNullOrWhiteSpace(address))
            {
                Debug.Log("<color=red>Address can not null!!!</color>");
                return null;
            }

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
            handle.Completed += (result) =>
            {
                onComplete?.Invoke(result.Result);
                Addressables.Release(handle);
            };

            return handle.Result;
        }

        /// <summary>
        /// Load multi assets. Auto remove key null.
        /// </summary>
        /// <typeparam name="T">Type of assets.</typeparam>
        /// <param name="addresses">Collection address. If null address auto remove.</param>
        /// <param name="callback">Gets called for every loaded asset</param>
        /// <param name="onComplete">Action on all asset</param>
        /// <param name="mode">MergeMode</param>
        /// <param name="releaseDependenciesOnFailure">Release or Not</param>
        /// <returns></returns>
        public T LoadAssetsAsync<T>(IEnumerable<string> addresses, System.Action<T> callback = null, System.Action<IList<T>> onComplete = null, Addressables.MergeMode mode = Addressables.MergeMode.Union, bool releaseDependenciesOnFailure = true) where T : Object
        {
            if (addresses != null || addresses.Count() <= 0)
            {
                Debug.Log("<color=red>Addresses is null or empty!!!</color>");
                return null;
            }

            List<string> tempAddress = addresses.Where(a => string.IsNullOrEmpty(a) || string.IsNullOrWhiteSpace(a)).ToList();

            AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(tempAddress, callback, mode, releaseDependenciesOnFailure);
            handle.Completed += (result) =>
            {
                onComplete?.Invoke(result.Result);
                Addressables.Release(handle);
            };

            return null;
        }
    }
}
