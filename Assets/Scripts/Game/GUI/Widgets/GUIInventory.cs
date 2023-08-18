namespace TVB.Game.GUI
{
    using System.Collections.Generic;
    using TVB.Core.Attributes;
    using TVB.Core.GUI;
    using TVB.Game.GameSignals;
    using UnityEngine;
    using UnityEngine.UI;

    public class GUIInventory : GUIComponent
    {
        private List<GUIInventoryItem> m_Items = new List<GUIInventoryItem>(20);

        [GetComponentInChildren("Item", true), SerializeField, HideInInspector]
        private GUIInventoryItem m_Item;

        [GetComponentInChildren("MainFrame", true), SerializeField, HideInInspector]
        private RectTransform m_MainFrame;
        [GetComponentInChildren("Content", true), SerializeField, HideInInspector]
        private Transform m_ContentTransform;
        [GetComponentInChildren("Content", true), SerializeField, HideInInspector]
        private GUIGridLayout m_GridLayout;

        [SerializeField]
        private Button m_InventoryToggleButton;

        public override void OnInitialized()
        {
            base.OnInitialized();

            m_Item.SetActive(false);
            m_Items.Add(m_Item);

            m_InventoryToggleButton.onClick.AddListener(OnToggleInventory);
            Signals.GUISignals.SetInventoryOpen.Connect(OnSetInventoryIsOpen);

            m_MainFrame.SetActive(false);
            SetData(AdventureGame.Instance.Inventory.Items);
        }

        public override void OnDeinitialized()
        {
            m_InventoryToggleButton.onClick.RemoveAllListeners();
            Signals.GUISignals.SetInventoryOpen.Disconnect(OnSetInventoryIsOpen);

            base.OnDeinitialized();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();


            if (Input.GetMouseButtonDown(1) == true && AdventureGame.Instance.IsInventoryOpen == true)
            {
                m_MainFrame.SetActive(false);
                AdventureGame.Instance.IsInventoryOpen = false;
            }
        }

        public void SetData(List<InventoryItem> data)
        {
            int dataCount = data.Count;

            if (dataCount > m_Items.Count)
            {
                GUIInventoryItem newItem = Instantiate(m_Item, m_ContentTransform);
                m_Items.Add(newItem);
            }

            m_GridLayout.Rearrange();

            for (int idx = 0, count = m_Items.Count; idx < count; idx++)
            {
                if (idx >= dataCount)
                {
                    m_Items[idx].SetActive(false);
                }
                else
                {
                    m_Items[idx].SetData(data[idx]);
                    m_Items[idx].SetActive(true);
                }
            }
        }

        // HANDLERS

        private void OnToggleInventory()
        {
            if (AdventureGame.Instance.IsBusy == true)
                return;

            bool inventoryActive = !m_MainFrame.gameObject.activeSelf;
            AdventureGame.Instance.IsInventoryOpen = inventoryActive;
        }

        private void OnSetInventoryIsOpen(bool open)
        {
            m_MainFrame.SetActive(open);
        }
    }

    public struct GUIItemData
    {
        public Sprite Sprite;
        public string Description;
    }
}
