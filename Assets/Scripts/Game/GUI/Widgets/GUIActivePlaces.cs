namespace TVB.Game.GUI
{
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.UI;

    using TVB.Core.GUI;
    using TVB.Game.Interactable;

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

        // PUBLIC METHODS

        public void Initialize(List<IInteractable> interactableItems)
        {
            for (int idx = 0, count = interactableItems.Count; idx < count; idx++)
            {
                IInteractable item = interactableItems[idx];
                Image iconPrefab = GetIconByActionType(item.ActionType);
                Image icon = Instantiate<Image>(iconPrefab, RectTransform);

                // TODO: set position

                GUIIcon iconItem = new GUIIcon
                {
                    Name = item.Name,
                    Icon = icon,
                };

                m_Icons.Add(iconItem);
            }
        }

        // UTILITIES

        private void AddIconToList(Image iconPrefab, ref List<Image> iconList)
        {
            Image icon = Instantiate<Image>(iconPrefab, RectTransform);
            iconList.Add(icon);
            icon.SetActive(false);
        }

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
