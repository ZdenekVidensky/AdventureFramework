﻿namespace TVB.Game.GUI
{
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.UI;

    using TVB.Core.GUI;
    using TVB.Core.Attributes;

    public class GUIInventory : GUIComponent
    {
        private List<GUIInventoryItem> m_Items = new List<GUIInventoryItem>(20);

        [GetComponentInChildren("Item", true), SerializeField, HideInInspector]
        private GUIInventoryItem       m_Item;

        [GetComponentInChildren("MainFrame", true), SerializeField, HideInInspector]
        private RectTransform          m_MainFrame;
        [GetComponentInChildren("Content", true), SerializeField, HideInInspector]
        private Transform              m_ContentTransform;
        [GetComponentInChildren("Content", true), SerializeField, HideInInspector]
        private GUIGridLayout          m_GridLayout;

        [SerializeField]
        private Button                 m_InventoryToggleButton;

        public override void OnInitialized()
        {
            base.OnInitialized();

            m_Item.SetActive(false);
            m_Items.Add(m_Item);

            m_InventoryToggleButton.onClick.AddListener(OnToggleInventory);
            m_MainFrame.SetActive(false);

            SetData(AdventureGame.Instance.Inventory.Items);
        }

        public override void OnDeinitialized()
        {
            m_InventoryToggleButton.onClick.RemoveAllListeners();

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

            m_GridLayout.Rearrange();
        }

        // HANDLERS

        private void OnToggleInventory()
        {
            bool inventoryActive = !m_MainFrame.gameObject.activeSelf;
            m_MainFrame.SetActive(inventoryActive);

            AdventureGame.Instance.IsInventoryOpen = inventoryActive;
        }
    }

    public struct GUIItemData
    {
        public Sprite Sprite;
        public string Description;
    }
}
