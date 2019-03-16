using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using wizardscode.digitalpainting;
using wizardscode.inventory;

public class InventoryUI : MonoBehaviour {
    DigitalPaintingManager manager;
    public Dropdown equippedItemDropdown;
    InventoryManager inventory;
    
    private void Start()
    {
        manager = GameObject.FindObjectOfType<DigitalPaintingManager>();
        inventory = manager.AgentWithFocus.GetComponent<InventoryManager>();
    }

    void Update () {
        PopulateEquippedDropdown();
	}

    void PopulateEquippedDropdown()
    {
        equippedItemDropdown.ClearOptions();

        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        options.Add(new Dropdown.OptionData("Nothing"));
        for (int i = 0; i < inventory.items.Length; i++)
        {
            if (inventory.items[i] != null)
            {
                options.Add(new Dropdown.OptionData(inventory.items[i].name));
            }
        }
        equippedItemDropdown.AddOptions(options);
    }

    public void OnItemEquippedChange()
    {
        manager.AgentWithFocus.EquipItemFromInventory(equippedItemDropdown.value - 1);
    }
}
