namespace TVB.Core.Interactable
{
    //using Sirenix.OdinInspector;
    using TVB.Core.Graph;
    using TVB.Core.Localization;
    using UnityEngine;

    public interface IInteractable
    {
        string Name { get; }
        EInteractableAction ActionType { get; }
        InteractiveGraph InteractiveGraph { get; }
        int CustomTextID { get; }
        Vector3 Position { get; }
        int ActivePlaceTextID { get; }
        void OnInteract();
        void OnUseItem(string itemID);
        void Destroy();
    }

    public enum EInteractableAction
    {
        None,
        Talk,
        Use,
        Examine,
        Take,
        Walk,
        Custom
    }

    [System.Serializable]
    public struct InteractableWithItem
    {
        public string ItemID;
        public InteractiveGraph InteractiveGraph;
        [Tooltip("Text ID of action for this item to be displayed")]
        public int CustomTextID;
        //[DisableInEditorMode, DisableInPlayMode, ShowInInspector]
        public string Text => TextDatabase.Localize[CustomTextID];
    }
}
