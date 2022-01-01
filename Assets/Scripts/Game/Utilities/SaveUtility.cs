namespace TVB.Game.Utilities
{
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    using UnityEngine;

    using TVB.Game.Save;

    public static class SaveUtility
    {
        public const string SAVE_SUFFIX    = ".sav";
        public const string AUTOSAVE_NAME  = "0autosave.sav";
        public const string QUICKSAVE_NAME = "1quicksave.sav";
        public const string DEFAULT_NAME   = "savegame.sav";

        public static void SaveGame(Vector3 position, EDirection direction, List<InventoryItem> items, Dictionary<string, bool> conditions, string sceneName, string saveName, int sceneNameID)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/" + saveName;
            FileStream stream = new FileStream(path, FileMode.Create);

            SaveData saveData = new SaveData(position, direction, items, conditions, sceneName, sceneNameID, System.DateTime.Now);

            formatter.Serialize(stream, saveData);
            stream.Close();
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

        public static List<GUISaveData> GetSaveData(bool includeQuicksave)
        {
            List<GUISaveData> result = new List<GUISaveData> (8);
            string[] saveFilePaths = Directory.GetFiles(Application.persistentDataPath);
            System.Array.Sort(saveFilePaths);

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
                    bool isAutoSave = fileName  == AUTOSAVE_NAME;

                    if (includeQuicksave == false && (isQuicksave == true || isAutoSave == true))
                        continue;

                    newData.Date         = saveData.Date;
                    newData.SceneNameID  = saveData.SceneNameID;
                    newData.IsQuicksave  = isQuicksave;
                    newData.IsAutosave   = isAutoSave;
                    newData.SaveFileName = fileName;

                    result.Add(newData);
                }
            }

            return result;
        }
    }
}
