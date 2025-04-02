using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ThanhDV.SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        #region Singleton

        private static SaveManager _instance;
        private static readonly object _lock = new object();

        public static SaveManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindFirstObjectByType(typeof(SaveManager)) as SaveManager;

                        if (_instance == null)
                        {
                            _instance = new GameObject().AddComponent<SaveManager>();
                            _instance.gameObject.name = _instance.GetType().Name;

                            Debug.Log($"<color=yellow>{_instance.GetType().Name} instance is null!!! Auto create new instance</color>");
                        }
                        DontDestroyOnLoad(_instance);
                    }
                    return _instance;
                }
            }
        }

        public static bool IsExist => _instance != null;

        public void InitializeInstance()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(_instance);
                return;
            }

            Debug.Log($"<color=red>{_instance.GetType().Name} instance is already exist!!!</color>");
            Destroy(gameObject);
        }
        #endregion

        [Header("Save File Config")]
        [SerializeField] private string fileName = "SaveData";
        [SerializeField] private bool useCustomPath;
#if UNITY_EDITOR
        [ConditionalHide("useCustomPath", true)]
#endif
        [SerializeField] private string customPath = "SaveData";
        [SerializeField] private bool useEncryption = false;

        private SaveData saveData;
        private FileDataHandler fileDataHandler;
        private List<ISavable> savables = new List<ISavable>();

        protected virtual void Awake()
        {
            InitializeInstance();
            LoadGame();
        }

        private void Start()
        {
            fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
            savables = new List<ISavable>(FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISavable>());
            LoadGame();
        }

        public void NewGame()
        {
            Debug.Log("<color=green>Creating new game...</color>");
            saveData = new SaveData();
        }

        public void LoadGame()
        {
            // Load the game data from a file
            saveData = fileDataHandler.Load();

            // Create a new SaveData instance if it doesn't exist
            NewGame();

            // Load the game data from the savables
            foreach (var savable in savables)
            {
                savable.Load(ref saveData);
            }
        }

        public void SaveGame()
        {
            // Save the game data to the savables
            foreach (var savable in savables)
            {
                savable.Save(saveData);
            }

            // Save the game data to a file
            fileDataHandler.Save(saveData);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            SaveGame();
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }
    }
}
