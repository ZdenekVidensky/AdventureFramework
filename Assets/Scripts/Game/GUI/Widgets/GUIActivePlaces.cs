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
        private Image          m_WalkIcon;
        [SerializeField]
        private Image          m_TalkIcon;
        [SerializeField]
        private Image          m_LookIcon;
        [SerializeField]
        private Image          m_TakeIcon;

        [Header("Prefab")]
        [SerializeField]
        private GUIActivePlace m_ActivePlacePrefab;

        // PRIVATE MEMBERS

        private List<GUIActivePlace> m_ActivePlaces = new List<GUIActivePlace>(ICONS_NUMBER);

        private Camera        m_MainCamera;

        public override void OnInitialized()
        {
            base.OnInitialized();

            m_MainCamera = Camera.main;
        }

        // PUBLIC METHODS

        public void InitializeItems(List<IInteractable> interactableItems)
        {
            for (int idx = 0, count = m_ActivePlaces.Count; idx < count; idx++)
            {
                Destroy(m_ActivePlaces[idx].gameObject);
            }

            m_ActivePlaces.Clear();

            for (int idx = 0, count = interactableItems.Count; idx < count; idx++)
            {
                IInteractable item = interactableItems[idx];
                Image iconPrefab = GetIconByActionType(item.ActionType);
                GUIActivePlace activePlace = Instantiate<GUIActivePlace>(m_ActivePlacePrefab, RectTransform);
                Vector3 screenPosition = m_MainCamera.WorldToScreenPoint(item.Position);

                activePlace.SetData(item.ActivePlaceTextID, iconPrefab.sprite, screenPosition);

                m_ActivePlaces.Add(activePlace);
            }
        }

        // UTILITIES

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
