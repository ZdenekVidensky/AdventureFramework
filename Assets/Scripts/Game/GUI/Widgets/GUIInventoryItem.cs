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

    public class GUIInventoryItem : GUIComponent, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
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

            Signals.GUISignals.SetItemDescription.Emit(TextDatabase.Localize[m_ItemNameID]);
            Signals.GUISignals.ShowItemDescription.Emit(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (AdventureGame.Instance.IsBusy == true)
                return;

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

            AdventureGame.Instance.SelectedItemID = m_ItemID;
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            RectTransform.position = Input.mousePosition;

            if (AdventureGame.Instance.SelectedItemID == m_ItemID)
            {

            }
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            this.transform.SetParent(m_OriginalParent);
            RectTransform.position = m_Position;

            if (m_ParentRectTransform != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(m_ParentRectTransform);
            }

            AdventureGame.Instance.TryToUseItem(m_ItemID);
            AdventureGame.Instance.SelectedItemID = null;
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
        } 
    }
}
