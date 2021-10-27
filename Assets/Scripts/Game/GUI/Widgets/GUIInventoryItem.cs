namespace TVB.Game.GUI
{
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    using TVB.Core.GUI;
    using TVB.Core.Attributes;
    using TVB.Game.GameSignals;
    using TVB.Core.Localization;
    using TVB.Game.Graph;

    public class GUIInventoryItem : GUIComponent, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [GetComponent(true), SerializeField, HideInInspector]
        private Image                  m_Image;

        [SerializeField]
        private Transform              m_View;

        private Vector3                m_Position;
        private string                 m_ItemID;
        private int                    m_ItemNameID;
        private RectTransform          m_ParentRectTransform;
        private Transform              m_OriginalParent;
        private List<InteractableWith> m_InteractableWithItems;

        public override void OnInitialized()
        {
            base.OnInitialized();

            m_ParentRectTransform = this.transform.parent.GetComponent<RectTransform>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (AdventureGame.Instance.IsBusy == true)
                return;

            if (AdventureGame.Instance.IsInventoryOpen == false)
                return;

            AdventureGame.Instance.HoveredItemID = m_ItemID;

            if (AdventureGame.Instance.SelectedItemID != null)
            {
                Signals.GUISignals.SetItemDescription.Emit(GetInteractingWithText(AdventureGame.Instance.SelectedItemID));
                Signals.GUISignals.ShowItemDescription.Emit(true);
            }
            else
            {
                Signals.GUISignals.SetItemDescription.Emit(TextDatabase.Localize[m_ItemNameID]);
                Signals.GUISignals.ShowItemDescription.Emit(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (AdventureGame.Instance.IsBusy == true)
                return;

            AdventureGame.Instance.HoveredItemID = null;
            Signals.GUISignals.ShowItemDescription.Emit(false);
        }

        public void SetData(InventoryItem item)
        {
            m_Image.sprite          = item.Sprite;
            m_Position              = RectTransform.position;
            m_ItemID                = item.ID;
            m_ItemNameID            = item.NameID;
            m_InteractableWithItems = item.InteractableWithItems;
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            m_OriginalParent = this.transform.parent;
            this.transform.SetParent(m_View);

            m_Image.raycastTarget = false;

            AdventureGame.Instance.SelectedItemID = m_ItemID;
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            RectTransform.position = Input.mousePosition;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            this.transform.SetParent(m_OriginalParent);
            RectTransform.position = m_Position;

            if (AdventureGame.Instance.IsInventoryOpen == true)
            {
                string selectedItemID = AdventureGame.Instance.SelectedItemID;
                string hoveredItemID = AdventureGame.Instance.HoveredItemID;

                if (selectedItemID != null && m_ItemID != hoveredItemID)
                {
                    InteractiveGraph graph = GetInteractiveGraphWithItem(hoveredItemID);

                    if (graph != null)
                    {
                        StartCoroutine(AdventureGame.Instance.GraphManager.ProcessInteractiveGraph(null, graph));
                    }
                }
            }
            else
            {
                AdventureGame.Instance.TryToUseItem(m_ItemID);
            }

            m_Image.raycastTarget = true;
            AdventureGame.Instance.SelectedItemID = null;
        }

        private string GetInteractingWithText(string otherItemID)
        {
            for (int idx = 0, count = m_InteractableWithItems.Count; idx < count; idx++)
            {
                InteractableWith item = m_InteractableWithItems[idx];

                if (item.OtherItemID == otherItemID)
                    return TextDatabase.Localize[item.TextID];
            }

            return null;
        }

        private InteractiveGraph GetInteractiveGraphWithItem(string otherItemID)
        {
            for (int idx = 0, count = m_InteractableWithItems.Count; idx < count; idx++)
            {
                InteractableWith item = m_InteractableWithItems[idx];

                if (item.OtherItemID == otherItemID)
                    return item.InteractiveGraph;
            }

            return null;
        }
    }
}
