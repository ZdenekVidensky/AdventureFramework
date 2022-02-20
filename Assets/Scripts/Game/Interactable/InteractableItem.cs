namespace TVB.Core.Interactable
{
    using UnityEngine;
    using Sirenix.OdinInspector;

    using TVB.Core.Graph;
    using TVB.Game.GameSignals;
    using TVB.Core.Localization;
    using TVB.Game;

    public class InteractableItem : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private EInteractableAction      m_InteractableAction;
        [SerializeField]
        private int                      m_CustomTextID;
        [SerializeField]
        private int                      m_ActivePlaceTextID;
        [DisableInEditorMode, DisableInPlayMode, ShowInInspector]
        public string CustomTextID => TextDatabase.Localize[m_CustomTextID];
        [SerializeField]
        private InteractiveGraph         m_InteractiveGraph;
        [SerializeField]
        private int                      m_ItemNameID;
        [DisableInEditorMode, DisableInPlayMode, ShowInInspector]
        public string ItemName => TextDatabase.Localize[m_ItemNameID];
        [DisableInEditorMode, DisableInPlayMode, ShowInInspector]
        public string ActivePlaceText => TextDatabase.Localize[m_ActivePlaceTextID];

        EInteractableAction IInteractable.ActionType         => m_InteractableAction;
        int IInteractable.CustomTextID                       => m_CustomTextID;
        int IInteractable.ActivePlaceTextID                  => m_ActivePlaceTextID;
        InteractiveGraph IInteractable.InteractiveGraph      => m_InteractiveGraph;
        string IInteractable.Name                            => gameObject.name;
        Vector3 IInteractable.Position                       => transform.position;


        [SerializeField, TableList]
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
                StartCoroutine(AdventureGame.Instance.GraphManager.ProcessInteractiveGraph(m_InteractiveGraph, this));
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

        private void OnMouseOver()
        {
            if (AdventureGame.Instance.IsBusy == true)
                return;

            if (AdventureGame.Instance.IsInventoryOpen == true)
                return;

            if (AdventureGame.Instance.AreActivePlacesVisible == true)
                return;

            string selectedItem = AdventureGame.Instance.SelectedItemID;
            string actionText;

            if (selectedItem != null)
            {
                InteractableWithItem interactableWith = GetInteractableWithItem(selectedItem);

                if (interactableWith.CustomTextID > 0)
                {
                    actionText = TextDatabase.Localize[interactableWith.CustomTextID];
                }
                else
                {
                    actionText = TextDatabase.Localize[m_ItemNameID];
                }
            }
            else if (m_CustomTextID > 0)
            {
                actionText = TextDatabase.Localize[m_CustomTextID];
            }
            else
            {
                actionText = TextDatabase.Localize[m_ItemNameID];
            }

            Signals.GUISignals.SetItemDescription.Emit(actionText);
            Signals.GUISignals.ShowItemDescription.Emit(true);
        }

        private void OnMouseExit()
        {
            if (AdventureGame.Instance.IsBusy == true)
                return;

            if (AdventureGame.Instance.IsInventoryOpen == true)
                return;

            if (AdventureGame.Instance.AreActivePlacesVisible == true)
                return;

            Signals.GUISignals.ShowItemDescription.Emit(false);
        }

        private void OnMouseDown()
        {
            if (AdventureGame.Instance.IsBusy == true)
                return;

            if (AdventureGame.Instance.IsInventoryOpen == true)
                return;

            if (AdventureGame.Instance.AreActivePlacesVisible == true)
                return;

            if (AdventureGame.Instance.IsGamePaused == true)
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

            StartCoroutine(AdventureGame.Instance.GraphManager.ProcessInteractiveGraph(graph, this));
        }

        // HELPERS

        private InteractiveGraph GetGraphByItemID(string itemID)
        {
            for (int idx = 0; idx < m_InteractableWithItems.Length; idx++)
            {
                InteractableWithItem item = m_InteractableWithItems[idx];

                if (item.ItemID == itemID)
                {
                    return item.InteractiveGraph;
                }
            }

            return null;
        }

        private InteractableWithItem GetInteractableWithItem(string itemID)
        {
            for (int idx = 0; idx < m_InteractableWithItems.Length; idx++)
            {
                InteractableWithItem item = m_InteractableWithItems[idx];

                if (item.ItemID == itemID)
                    return item;
            }

            return default;
        }
    }
}
