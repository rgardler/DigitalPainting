using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using wizardscode.inventory;

[CustomEditor(typeof(InventoryManager))]
public class InventoryManagerEditor : Editor {
    private SerializedProperty itemsProperty;

    private const string inventoryPropItemsName = "items";

    private bool[] showItemSlots = new bool[InventoryManager.numItemSlots];

    private void OnEnable()
    {
        itemsProperty = serializedObject.FindProperty(inventoryPropItemsName);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        for (int i = 0; i < InventoryManager.numItemSlots; i++)
        {
            ItemSlotGUI(i);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void ItemSlotGUI(int index)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;
        SerializedProperty item = itemsProperty.GetArrayElementAtIndex(index);
        
        showItemSlots[index] = EditorGUILayout.Foldout(showItemSlots[index], "Item slot " + index);
        if (showItemSlots[index])
        {
            EditorGUILayout.PropertyField(itemsProperty.GetArrayElementAtIndex(index));
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }
}
