using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace ThanhDV.SaveSystem
{
    public class FileDataHandler
    {
        private string dataDirPath;
        private string dataFileName;
        private bool useEncryption = false;

        private string fullPath => Path.Combine(dataDirPath, dataFileName);

        private const string ENCRYPTION_CODE = "ThanhDV";

        public FileDataHandler(string _dataDirPath, string _dataFileName, bool _useEncryption)
        {
            dataDirPath = _dataDirPath;
            dataFileName = _dataFileName;
            useEncryption = _useEncryption;
        }

        public void Save(SaveData saveData)
        {
            if (!Directory.Exists(dataDirPath)) Directory.CreateDirectory(dataDirPath);

            try
            {
                string dataAsJson = JsonConvert.SerializeObject(saveData, Formatting.Indented);

                if (useEncryption) dataAsJson = Encrypt(dataAsJson);

                using FileStream stream = new(fullPath, FileMode.Create);
                using StreamWriter writer = new(stream);
                writer.Write(dataAsJson);
            }
            catch (System.Exception)
            {
                Debug.Log($"<color=red>Failed to save data to {fullPath}</color>");
            }
        }

        public SaveData Load()
        {
            SaveData result = null;
            if (File.Exists(fullPath))
            {
                try
                {
                    string dataAsJson = "";

                    using FileStream stream = new(fullPath, FileMode.Open);
                    using StreamReader reader = new(stream);
                    dataAsJson = reader.ReadToEnd();

                    if (useEncryption) dataAsJson = Decrypt(dataAsJson);

                    result = JsonConvert.DeserializeObject<SaveData>(dataAsJson);
                }
                catch (System.Exception)
                {
                    Debug.Log($"<color=red>Failed to load data from {fullPath}</color>");
                }
            }

            Debug.Log($"<color=red>File not found: {fullPath}</color>");
            return result;
        }

        private string Encrypt(string data)
        {
            char[] modifiedData = new char[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                modifiedData[i] = (char)(data[i] ^ ENCRYPTION_CODE[i % ENCRYPTION_CODE.Length]);
            }
            return new string(modifiedData);
        }

        private string Decrypt(string data)
        {
            char[] modifiedData = new char[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                modifiedData[i] = (char)(data[i] ^ ENCRYPTION_CODE[i % ENCRYPTION_CODE.Length]);
            }
            return new string(modifiedData);
        }
    }
}
