﻿

using System.Collections.Generic;
using UnityEngine;

namespace TVB.Game.Save
{
    [System.Serializable]
    public class SaveData
    {
        public float PositionX;
        public float PositionY;
        public string[] InventoryItems;
        public string SceneName;
        public ConditionSaveData[] Conditions;

        // TODO: Direction

        public SaveData(Vector3 position, List<InventoryItem> items, Dictionary<string, bool> conditions, string sceneName)
        {
            PositionX = position.x;
            PositionY = position.y;
            SceneName = sceneName;

            int itemsCount = items.Count;
            InventoryItems = new string[itemsCount];

            for (int idx = 0; idx < itemsCount; idx++)
            {
                InventoryItems[idx] = items[idx].ID;
            }

            int conditionsCount = conditions.Count;
            Conditions = new ConditionSaveData[conditionsCount];

            int counter = 0;
            foreach(KeyValuePair<string, bool> condition in conditions)
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