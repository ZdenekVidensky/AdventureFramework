namespace TVB.Game
{
    using System.Collections.Generic;

    public class Inventory
    {
        // PUBLIC MEMBERS

        public int Count => m_Items.Count;
        public List<InventoryItem> Items => m_Items;

        // PRIVATE MEMBERS

        private List<InventoryItem> m_Items = new List<InventoryItem>(32);

        // PUBLIC METHODS

        public void AddItems(List<InventoryItem> items)
        {
            m_Items.AddRange(items);
        }

        public void AddItem(InventoryItem item)
        {
            m_Items.Add(item);
        }

        public void RemoveItem(InventoryItem item)
        {
            m_Items.Remove(item);
        }

        public void RemoveItem(string ID)
        {
            InventoryItem item = FindItem(ID);

            if (item == null)
                return;

            m_Items.Remove(item);
        }

        public bool HasItem(string ID)
        {
            InventoryItem item = FindItem(ID);
            return item != null;
        }

        // PRIVATE METHODS

        private InventoryItem FindItem(string ID)
        {
            for (int idx = 0, count = m_Items.Count; idx < count; ++idx)
            {
                InventoryItem item = m_Items[idx];

                if (item.ID == ID)
                    return item;
            }

            return null;
        }
    }
}
