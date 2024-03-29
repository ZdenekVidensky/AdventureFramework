﻿namespace TVB.Game.Utilities
{
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using TVB.Game.Save;
    using UnityEngine;

    public static class SaveUtility
    {
        public const string SAVE_SUFFIX = ".sav";
        public const string AUTOSAVE_NAME = "0autosave.sav";
        public const string QUICKSAVE_NAME = "1quicksave.sav";
        public const string DEFAULT_NAME = "savegame.sav";

        public static void SaveGame(Vector3 position, EDirection direction, List<InventoryItem> items, Dictionary<string, bool> conditions, string sceneName, string saveName, int sceneNameID)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/" + saveName;
            FileStream stream = new FileStream(path, FileMode.Create);

            SaveData saveData = new SaveData(position, direction, items, conditions, sceneName, sceneNameID, System.DateTime.Now);

            formatter.Serialize(stream, saveData);
            stream.Close();
        }

        public static bool SaveFilesExists()
        {
            string[] saveFilePaths = Directory.GetFiles(Application.persistentDataPath);

            foreach (string saveFilePath in saveFilePaths)
            {
                if (saveFilePath.EndsWith(SaveUtility.SAVE_SUFFIX) == true)
                    return true;
            }

            return false;
        }

        public static SaveData LoadGame(string saveName)
        {
            string path = Application.persistentDataPath + "/" + saveName;

            if (File.Exists(path) == false)
                return null;

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData result = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return result;
        }

        public static SaveData GetLatestSaveData(bool includeQuickAndAutosave = true)
        {
            List<GUISaveData> saveData = GetSaveData(includeQuickAndAutosave);

            if (saveData.Count == 0)
                return null;

            saveData.Sort((a, b) => b.Date.CompareTo(a.Date));

            return LoadGame(saveData[0].SaveFileName);
        }

        public static List<GUISaveData> GetSaveData(bool includeQuickAndAutosave)
        {
            List<GUISaveData> result = new List<GUISaveData>(8);
            string[] saveFilePaths = Directory.GetFiles(Application.persistentDataPath);

            foreach (string saveFilePath in saveFilePaths)
            {
                if (saveFilePath.EndsWith(SaveUtility.SAVE_SUFFIX) == true)
                {
                    GUISaveData newData = new GUISaveData();

                    BinaryFormatter formatter = new BinaryFormatter();
                    FileStream stream = new FileStream(saveFilePath, FileMode.Open);

                    SaveData saveData = formatter.Deserialize(stream) as SaveData;
                    stream.Close();

                    string fileName = Path.GetFileName(saveFilePath);

                    bool isQuicksave = fileName == QUICKSAVE_NAME;
                    bool isAutoSave = fileName == AUTOSAVE_NAME;

                    if (includeQuickAndAutosave == false && (isQuicksave == true || isAutoSave == true))
                        continue;

                    newData.Date = saveData.Date;
                    newData.SceneNameID = saveData.SceneNameID;
                    newData.IsQuicksave = isQuicksave;
                    newData.IsAutosave = isAutoSave;
                    newData.SaveFileName = fileName;
                    newData.Index = int.Parse(fileName.Substring(0, 1));

                    result.Add(newData);
                }
            }

            result.Sort((a, b) => a.Index.CompareTo(b.Index));

            return result;
        }
    }
}
