using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.inventory
{
    /// <summary>
    /// Represents a Thing that can be put in an inventory.
    /// </summary>
    [CreateAssetMenu(fileName = "Item", menuName = "Wizards Code/Inventory/Item", order = 100)]
    public class InventoryItem : ScriptableObject
    {
        public Sprite sprite;
    }
}
