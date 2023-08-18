namespace TVB.Game
{
    using Sirenix.OdinInspector;
    using System.Collections.Generic;
    using TVB.Core.Graph;
    using TVB.Core.Localization;
    using UnityEngine;

    [CreateAssetMenu(fileName = "InventoryItem", menuName = "Items/InventoryItem")]
    [System.Serializable]
    public class InventoryItem : ScriptableObject
    {
        // STATIC NAMES

        public string ID;
        public int NameID;
        [DisableInEditorMode, DisableInPlayMode, ShowInInspector]
        public string Text => TextDatabase.Localize[NameID];

        [PreviewField(80)]
        public Sprite Sprite;

        public List<InteractableWith> InteractableWithItems;
    }

    [System.Serializable]
    public struct InteractableWith
    {
        public string OtherItemID;
        public int TextID;
        public InteractiveGraph InteractiveGraph;
    }
}
