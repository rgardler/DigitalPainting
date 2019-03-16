using UnityEngine;
using UnityEngine.UI;
using wizardscode.interaction;

namespace wizardscode.inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public Interactable[] items = new Interactable[numItemSlots];

        public const int numItemSlots = 4;

        private int storedItemCount;

        /// <summary>
        /// Add an item to the first available slot in the inventory.
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <returns>True if the item was added successfully, otherwise false.</returns>
        public bool AddItem(Interactable item)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    item.transform.SetParent(gameObject.transform, true);
                    item.transform.localPosition = Vector3.zero;
                    item.gameObject.SetActive(false);

                    items[i] = item;
                    storedItemCount++;
                    return true;
                }
            }
            return false;
        }

        public Interactable GetItem(int index)
        {
            Interactable item = items[index];
            items[index] = null;
            storedItemCount--;
            return item;
        }

        /// <summary>
        /// How many items are currently in this inventory.
        /// </summary>
        /// <returns>Number of items in the inventory.</returns>
        public int Count
        {
            get { return storedItemCount; }
        }

        public int AvailableCapacity
        {
            get { return numItemSlots - Count; }
        }
    }
}