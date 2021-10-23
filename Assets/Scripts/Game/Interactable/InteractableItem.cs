namespace TVB.Game.Interactable
{
    using UnityEngine;

    using TVB.Game.Graph;
    using TVB.Game.GameSignals;

    public class InteractableItem : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private EInteractableAction      m_InteractableAction;
        [SerializeField]
        private int                      m_CustomTextID;
        [SerializeField]
        private InteractiveGraph         m_InteractiveGraph;
        [SerializeField]
        private int                      m_ItemNameID;

        EInteractableAction IInteractable.ActionType         => m_InteractableAction;
        int IInteractable.CustomTextID                       => m_CustomTextID;
        InteractiveGraph IInteractable.InteractiveGraph      => m_InteractiveGraph;
        string IInteractable.Name                            => gameObject.name;


        [SerializeField]
        private InteractableWithItem[] m_InteractableWithItems;

        public void OnLeave()
        {
        }

        // IINTERACTABLE INTERFACE

        void IInteractable.Destroy()
        {
            Destroy(this.gameObject);
        }

        void IInteractable.OnInteract()
        {
            if (m_InteractiveGraph != null)
            {
                StartCoroutine(AdventureGame.Instance.GraphManager.ProcessInteractiveGraph(this, m_InteractiveGraph));
            }
        }

        // MONOBEHAVIOUR 

        private void Start()
        {
            if (AdventureGame.Instance.GetCondition($"{gameObject.name}-takencondition") == true)
            {
                Destroy(this.gameObject);
            }
        }

        // PRIVATE METHODS

        private void OnMouseEnter()
        {
            if (AdventureGame.Instance.IsBusy == true)
                return;

            if (AdventureGame.Instance.IsInventoryOpen == true)
                return;

            Signals.GUISignals.SetItemDescription.Emit($"Vzít {gameObject.name}");
            Signals.GUISignals.ShowItemDescription.Emit(true);
        }

        private void OnMouseExit()
        {
            if (AdventureGame.Instance.IsBusy == true)
                return;

            if (AdventureGame.Instance.IsInventoryOpen == true)
                return;

            Signals.GUISignals.ShowItemDescription.Emit(false);
        }

        private void OnMouseDown()
        {
            if (AdventureGame.Instance.IsBusy == true)
                return;

            (this as IInteractable).OnInteract();
        }

        void IInteractable.OnUseItem(string itemID)
        {
            if (AdventureGame.Instance.IsBusy == true)
                return;

            InteractiveGraph graph = GetGraphByItemID(itemID);

            if (graph == null)
            {
                // TODO: some default quote
                return;
            }

            StartCoroutine(AdventureGame.Instance.GraphManager.ProcessInteractiveGraph(this, graph));
        }

        // HELPERS

        private InteractiveGraph GetGraphByItemID(string itemID)
        {
            for (int idx = 0; idx < m_InteractableWithItems.Length; idx++)
            {
                InteractableWithItem item = m_InteractableWithItems[idx];

                if (item.ID == itemID)
                {
                    return item.InteractiveGraph;
                }
            }

            return null;
        }
    }
}
