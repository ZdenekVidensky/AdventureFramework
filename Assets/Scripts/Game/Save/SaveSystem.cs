using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace TVB.Game.Save
{
    public static class SaveSystem
    {
        public static void SaveGame(Vector3 position, List<InventoryItem> items, Dictionary<string, bool> conditions, string sceneName, string saveName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/" + saveName + ".sav";
            FileStream stream = new FileStream(path, FileMode.Create);

            SaveData saveData = new SaveData(position, items, conditions, sceneName);

            formatter.Serialize(stream, saveData);
            stream.Close();
        }

        public static SaveData LoadGame(string saveName)
        {
            string path = Application.persistentDataPath + "/" + saveName + ".sav";

            if (File.Exists(path) == false)
                return null;

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData result = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return result;
        }
    }
}
