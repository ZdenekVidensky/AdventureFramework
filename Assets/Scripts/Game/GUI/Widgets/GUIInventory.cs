namespace TVB.Game.GUI
{
    using System.Collections.Generic;

    using UnityEngine;

    using TVB.Core.GUI;
    using TVB.Core.Attributes;

    public class GUIInventory : GUIComponent
    {
        private List<GUIInventoryItem> m_Items = new List<GUIInventoryItem>(20);

        [GetComponentInChildren("Item", true), SerializeField, HideInInspector]
        private GUIInventoryItem m_Item;

        public override void OnInitialized()
        {
            base.OnInitialized();

            m_Item.SetActive(false);
            m_Items.Add(m_Item);

            SetData(AdventureGame.Instance.Inventory.Items);
        }

        public void SetData(List<InventoryItem> data)
        {
            int dataCount = data.Count;

            if (dataCount > m_Items.Count)
            {
                GUIInventoryItem newItem = Instantiate(m_Item, transform);
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
        }
    }

    public struct GUIItemData
    {
        public Sprite Sprite;
        public string Description;
    }
}
