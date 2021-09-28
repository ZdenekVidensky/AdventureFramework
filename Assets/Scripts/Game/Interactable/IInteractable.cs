namespace TVB.Game.Interactable
{
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
		public string ID;
		public InteractiveGraph InteractiveGraph;
	}
}
