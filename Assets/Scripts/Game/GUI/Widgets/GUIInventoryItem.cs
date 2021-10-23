namespace TVB.Game.GUI
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    using TVB.Core.GUI;
    using TVB.Core.Attributes;
    using TVB.Game.GameSignals;

    public class GUIInventoryItem : GUIComponent, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
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

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (AdventureGame.Instance.IsBusy == true)
                return;

            Signals.GUISignals.SetItemDescription.Emit($"Vzít {gameObject.name}");
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
