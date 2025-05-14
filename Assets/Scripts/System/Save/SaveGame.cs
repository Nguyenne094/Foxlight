using System;
using System.IO;
using UnityEngine;
using Utilities;

namespace Bap.Save
{
    public interface ISerializer
    {
        string Serialize<T>(T data, string fileName, bool overwrite) where T : IData;
        T Deserialize<T>(string fileName) where T : IData;
    }

    public interface IDataService
    {
        void Save(IData data, string fileName, bool overwrite = true);
        T Load<T>(string fileName) where T : IData;
        void Delete(string fileName);
        void DeleteAll();
    }

    public interface IData{}

    [Serializable]
    public struct PlayerData : IData
    {
        public Vector3 CurrentPosition;
        public Vector3 LastCheckPoint;
        public int CurrentHealth;

        public PlayerData(Vector3 currentPosition, Vector3 lastCheckPoint, int currentHealth)
        {
            this.CurrentHealth = currentHealth;
            this.LastCheckPoint = lastCheckPoint;
            this.CurrentPosition = currentPosition;
        }
    }
    
    public class SaveGame : Singleton<SaveGame>, ISerializer, IDataService
    {
        
        public readonly string PlayerDataFileName = "PlayerData";
        
        private string _savePath;

        public override void Awake()
        {
            base.Awake();
            _savePath = Application.persistentDataPath + Path.DirectorySeparatorChar;
        }

        public string Serialize<IData>(IData data, string fileName, bool overwrite) where IData : Save.IData
        {
            string saveFile = CombineWithPath(fileName);

            if (!overwrite && File.Exists(saveFile))
            {
                Debug.LogWarning($"File {fileName} already exists and overwrite is set to false.");
                return File.ReadAllText(saveFile);
            }

            string json = JsonUtility.ToJson(data, true);
            try
            {
                File.WriteAllText(saveFile, json);
            }
            catch (IOException e)
            {
                Debug.LogError($"Failed to save file {fileName}: {e.Message}");
                throw;
            }
            return json;
        }

        public IData Deserialize<IData>(string fileName) where IData : Save.IData
        {
            string saveFile = CombineWithPath(fileName);
            if (!File.Exists(saveFile))
            {
                Debug.LogError("Save file does not exist at path: " + saveFile);
                throw new FileNotFoundException("Save file not found", saveFile);
            }

            string json = File.ReadAllText(saveFile);
            try
            {
                return JsonUtility.FromJson<IData>(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to deserialize file {fileName}: {e.Message}");
                throw;
            }
        }

        public void Save(IData data, string fileName, bool overwrite = true)
        {
            Serialize(data, fileName, overwrite);
        }

        public T Load<T>(string fileName) where T : IData
        {
            return Deserialize<T>(fileName);
        }
        
        public void Delete(string fileName)
        {
            string saveFile = CombineWithPath(fileName);
            File.Delete(saveFile);
        }

        /// <summary>
        /// Delete all file from Application.persistentDataPath
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void DeleteAll()
        {
            var files = Directory.GetFiles(_savePath, "*.json");
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }

        /// <summary>
        /// Combine file name with Application.persistentDataPath 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string CombineWithPath(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name cannot be empty or null");

            string extension = Path.GetExtension(fileName).ToLower();
            if (!string.IsNullOrEmpty(extension))
                fileName = Path.GetFileNameWithoutExtension(fileName);
            return Path.Combine(_savePath, fileName + ".json");
        }
    }
}