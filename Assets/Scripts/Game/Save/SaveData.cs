

using System.Collections.Generic;
using UnityEngine;

namespace TVB.Game.Save
{
    [System.Serializable]
    public class SaveData
    {
        public System.DateTime Date;
        public float PositionX;
        public float PositionY;
        public string[] InventoryItems;
        public string SceneName;
        public int Direction;
        public int SceneNameID;
        public ConditionSaveData[] Conditions;

        public SaveData(Vector3 position,
            EDirection direction, List<InventoryItem> items,
            Dictionary<string, bool> conditions,
            string sceneName,
            int sceneNameID,
            System.DateTime date)
        {
            PositionX = position.x;
            PositionY = position.y;
            Direction = (int)direction;
            SceneName = sceneName;
            SceneNameID = sceneNameID;
            Date = date;

            int itemsCount = items.Count;
            InventoryItems = new string[itemsCount];

            for (int idx = 0; idx < itemsCount; idx++)
            {
                InventoryItems[idx] = items[idx].ID;
            }

            int conditionsCount = conditions.Count;
            Conditions = new ConditionSaveData[conditionsCount];

            int counter = 0;
            foreach (KeyValuePair<string, bool> condition in conditions)
            {
                Conditions[counter] = new ConditionSaveData() { Name = condition.Key, Value = condition.Value };
                counter++;
            }
        }
    }

    [System.Serializable]
    public struct ConditionSaveData
    {
        public string Name;
        public bool Value;
    }
}
