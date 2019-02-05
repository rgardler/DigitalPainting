using UnityEngine;
using UnityEngine.UI;

namespace wizardscode.inventory
{
    public class InventoryManager : MonoBehaviour
    {

        public Image[] itemImages = new Image[numItemSlots];
        public InventoryItem[] items = new InventoryItem[numItemSlots];

        public const int numItemSlots = 4;

        public void AddItem(InventoryItem item)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    items[i] = item;
                    itemImages[i].sprite = item.sprite;
                    itemImages[i].enabled = true;
                    return;
                }
            }
        }

        public void RemoveItem(InventoryItem item)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == item)
                {
                    items[i] = null;
                    itemImages[i].sprite = null;
                    itemImages[i].enabled = false;
                    return;
                }
            }
        }
    }
}