namespace TVB.Game.GUI
{
    using UnityEngine;
    using UnityEngine.UI;

    using TVB.Core.GUI;
    using TVB.Core.Attributes;
    using UnityEngine.EventSystems;

    public class GUIInventoryItem : GUIComponent, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [GetComponent(true), SerializeField, HideInInspector]
        private Image         m_Image;
        [GetComponent(true), SerializeField, HideInInspector]
        private RectTransform m_RectTransform;

        private Vector3       m_Position;
        private string        m_ItemID;
        private RectTransform m_ParentRectTransform;
        public override void OnInitialized()
        {
            base.OnInitialized();

            m_ParentRectTransform = this.transform.parent.GetComponent<RectTransform>();
        }

        public void SetData(InventoryItem item)
        {
            m_Image.sprite = item.Sprite;
            m_Position     = m_RectTransform.position;
            m_ItemID       = item.ID;
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            m_RectTransform.position = Input.mousePosition;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            m_RectTransform.position = m_Position;

            LayoutRebuilder.ForceRebuildLayoutImmediate(m_ParentRectTransform);

            AdventureGame.Instance.TryToUseItem(m_ItemID);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
        } 
    }
}
