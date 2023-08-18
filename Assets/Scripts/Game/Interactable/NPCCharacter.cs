namespace TVB.Core.Interactable
{
    //using Sirenix.OdinInspector;
    using TVB.Core.Graph;
    using TVB.Core.Localization;
    using TVB.Game;
    using TVB.Game.GameSignals;
    using UnityEngine;

    public class NPCCharacter : MonoBehaviour, IInteractable, ITalkable
    {
        // CONFIGURATION

        [SerializeField]
        private int m_NameTextID;

        [SerializeField]
        private ETalkableCharacter m_TalkableCharacter;

        //[DisableInEditorMode, DisableInPlayMode, ShowInInspector]
        public string NameText => TextDatabase.Localize[m_NameTextID];
        [SerializeField]
        private EInteractableAction m_InteractableAction;
        [SerializeField]
        private int m_CustomTextID;
        [SerializeField]
        private int m_ActivePlaceTextID;
        //[DisableInEditorMode, DisableInPlayMode, ShowInInspector]
        public string CustomTextID => TextDatabase.Localize[m_CustomTextID];
        [SerializeField]
        private InteractiveGraph m_InteractiveGraph;
        [SerializeField]
        private Animator m_Animator;
        [SerializeField]
        private InteractableWithItem[] m_InteractableWithItems;
        //[DisableInEditorMode, DisableInPlayMode, ShowInInspector]
        public string ActivePlaceText => TextDatabase.Localize[m_ActivePlaceTextID];

        // STATIC MEMBERS

        private static int TALKING_PARAMETER = Animator.StringToHash("Talking");
        private static readonly int TALK_VARIANT_PARAMETER = Animator.StringToHash("TalkVariant");
        private static int INTERACT_PARAMETER = Animator.StringToHash("Interact");
        private static int LEAVE_PARAMETER = Animator.StringToHash("Leave");

        // IINTERACTABLE INTERFACE

        EInteractableAction IInteractable.ActionType => m_InteractableAction;
        int IInteractable.CustomTextID => m_CustomTextID;
        int IInteractable.ActivePlaceTextID => m_ActivePlaceTextID;
        InteractiveGraph IInteractable.InteractiveGraph => m_InteractiveGraph;
        string IInteractable.Name => gameObject.name;
        Vector3 IInteractable.Position => transform.position;

        void IInteractable.OnInteract()
        {
            if (m_Animator != null)
            {
                m_Animator.SetTrigger(INTERACT_PARAMETER);
            }

            if (m_InteractiveGraph != null)
            {
                StartCoroutine(AdventureGame.Instance.GraphManager.ProcessInteractiveGraph(m_InteractiveGraph, this));
            }
        }

        void IInteractable.Destroy() { }

        ETalkableCharacter ITalkable.Character { get => m_TalkableCharacter; }

        void ITalkable.SetIsTalking(bool state)
        {
            if (m_Animator == null)
                return;

            m_Animator.SetBool(TALKING_PARAMETER, state);

            if (state == true)
            {
                m_Animator.SetInteger(TALK_VARIANT_PARAMETER, Random.Range(0, 2));
            }
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

        // MONOBEHAVIOUR

        private void OnMouseDown()
        {
            if (AdventureGame.Instance.IsBusy == true)
                return;

            if (AdventureGame.Instance.IsGamePaused == true)
                return;

            if (AdventureGame.Instance.AreActivePlacesVisible == true)
                return;

            (this as IInteractable).OnInteract();
        }


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
                    actionText = TextDatabase.Localize[m_NameTextID];
                }
            }
            else if (m_CustomTextID > 0)
            {
                actionText = TextDatabase.Localize[m_CustomTextID];
            }
            else
            {
                actionText = TextDatabase.Localize[m_NameTextID];
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
