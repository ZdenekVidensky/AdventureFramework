namespace TVB.Game
{
    using UnityEngine;
    using Sirenix.OdinInspector;

    using TVB.Core.Localization;

    [CreateAssetMenu(fileName = "InventoryItem", menuName = "Items/InventoryItem")]
    [System.Serializable]
    public class InventoryItem : ScriptableObject
    {
        // STATIC NAMES

        public const int SWORD_ID = 1;

        public string ID;
        public int NameID;
        [DisableInEditorMode, DisableInPlayMode, ShowInInspector]
        public string Text => TextDatabase.Localize[NameID];

        [PreviewField(80)]
        public Sprite Sprite;
    }
}
