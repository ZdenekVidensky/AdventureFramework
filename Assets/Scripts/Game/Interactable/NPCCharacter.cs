namespace TVB.Game.Interactable
{
    using UnityEngine;

    using TVB.Game.Graph;

    public class NPCCharacter : MonoBehaviour, IInteractable, ITalkable
    {
        // CONFIGURATION

        [SerializeField]
        private EInteractableAction      m_InteractableAction;
        [SerializeField]
        private int                      m_CustomTextID;
        [SerializeField]
        private InteractiveGraph         m_InteractiveGraph;
        [SerializeField]
        private Animator                 m_Animator;
        [SerializeField]
        private InteractableWithItem[]   m_InteractableWithItems;

        // STATIC MEMBERS

        private static int TALKING_PARAMETER      = Animator.StringToHash("Talking");
        private static int TALK_VARIANT_PARAMETER = Animator.StringToHash("TalkVariant");
        private static int INTERACT_PARAMETER     = Animator.StringToHash("Interact");
        private static int LEAVE_PARAMETER        = Animator.StringToHash("Leave");

        // IINTERACTABLE INTERFACE

        EInteractableAction IInteractable.ActionType         => m_InteractableAction;
        int IInteractable.CustomTextID                       => m_CustomTextID;
        InteractiveGraph IInteractable.InteractiveGraph      => m_InteractiveGraph;
        string IInteractable.Name                            => gameObject.name;

        void IInteractable.OnInteract()
        {
            if (m_Animator != null)
            {
                m_Animator.SetTrigger(INTERACT_PARAMETER);
            }

            if (m_InteractiveGraph != null)
            {
                StartCoroutine(AdventureGame.Instance.GraphManager.ProcessInteractiveGraph(this, m_InteractiveGraph));
            }
        }

        void IInteractable.Destroy() {}

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

            StartCoroutine(AdventureGame.Instance.GraphManager.ProcessInteractiveGraph(this, graph));
        }

        private void OnMouseDown()
        {
            if (AdventureGame.Instance.IsBusy == true)
                return;

            (this as IInteractable).OnInteract();
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
