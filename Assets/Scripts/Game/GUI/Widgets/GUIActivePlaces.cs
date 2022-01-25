namespace TVB.Game.GUI
{
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.UI;

    using TVB.Core.GUI;
    using TVB.Core.Interactable;

    public class GUIActivePlaces : GUIComponent
    {
        // CONSTANTS

        private const int ICONS_NUMBER = 16;

        // CONFIGURATION

        [Header("Icons")]
        [SerializeField]
        private Image         m_WalkIcon;
        [SerializeField]
        private Image         m_TalkIcon;
        [SerializeField]
        private Image         m_LookIcon;
        [SerializeField]
        private Image         m_TakeIcon;

        // PRIVATE MEMBERS

        private List<GUIIcon> m_Icons = new List<GUIIcon>(ICONS_NUMBER);
        private Camera        m_MainCamera;

        public override void OnInitialized()
        {
            base.OnInitialized();

            m_MainCamera = Camera.main;
        }

        // PUBLIC METHODS

        public void InitializeItems(List<IInteractable> interactableItems)
        {
            for (int idx = 0, count = m_Icons.Count; idx < count; idx++)
            {
                Destroy(m_Icons[idx].Icon.gameObject);
            }

            m_Icons.Clear();

            for (int idx = 0, count = interactableItems.Count; idx < count; idx++)
            {
                IInteractable item = interactableItems[idx];
                Image iconPrefab   = GetIconByActionType(item.ActionType);
                Image icon         = Instantiate<Image>(iconPrefab, RectTransform);
                icon.raycastTarget = false;

                Vector3 screenPosition = m_MainCamera.WorldToScreenPoint(item.Position);
                icon.rectTransform.position = screenPosition;

                GUIIcon iconItem = new GUIIcon
                {
                    Name = item.Name,
                    Icon = icon,
                };

                m_Icons.Add(iconItem);
            }
        }

        // UTILITIES

        private struct GUIIcon
        {
            public string Name;
            public Image  Icon;
        }

        private Image GetIconByActionType(EInteractableAction action)
        {
            switch (action)
            {
                case EInteractableAction.Talk:
                    return m_TalkIcon;
                case EInteractableAction.Walk:
                    return m_WalkIcon;
                case EInteractableAction.Take:
                    return m_TakeIcon;
                default:
                    return m_LookIcon;
            }
        }
    }
}
