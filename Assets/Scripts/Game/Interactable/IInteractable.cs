namespace TVB.Game.Interactable
{
    using UnityEngine;
    using Sirenix.OdinInspector;

    using TVB.Core.Localization;
    using TVB.Game.Graph;

    public interface IInteractable
	{
		string                   Name             { get; }
		EInteractableAction      ActionType       { get; }
		InteractiveGraph         InteractiveGraph { get; }
		int                      CustomTextID     { get; }
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
		Custom
	}

	[System.Serializable]
	public struct InteractableWithItem
	{
		public string ItemID;
		public InteractiveGraph InteractiveGraph;
		[Tooltip("Text ID of action for this item to be displayed")]
		public int CustomTextID;
		[DisableInEditorMode, DisableInPlayMode, ShowInInspector]
		public string Text => TextDatabase.Localize[CustomTextID];
	}
}
