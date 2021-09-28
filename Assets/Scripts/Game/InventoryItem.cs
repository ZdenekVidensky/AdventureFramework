namespace TVB.Game
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "InventoryItem", menuName = "Items/InventoryItem")]
    [System.Serializable]
    public class InventoryItem : ScriptableObject
    {
        // STATIC NAMES

        public const int SWORD_ID = 1;

        public string ID;
        public Sprite Sprite;
    }
}
