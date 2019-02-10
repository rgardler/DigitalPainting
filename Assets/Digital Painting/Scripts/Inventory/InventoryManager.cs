using UnityEngine;
using UnityEngine.UI;
using wizardscode.interaction;

namespace wizardscode.inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public Interactable[] items = new Interactable[numItemSlots];

        public const int numItemSlots = 4;

        public void AddItem(Interactable item)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    items[i] = item;
                    return;
                }
            }
        }

        public void RemoveItem(Interactable item)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == item)
                {
                    items[i] = null;
                    return;
                }
            }
        }
    }
}