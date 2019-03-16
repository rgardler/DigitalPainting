using UnityEngine;
using UnityEditor;
using wizardscode.extension;
using wizardscode.ability;
using System;
using wizardscode.condition.inventory;

namespace wizardscode.interaction
{
    [CustomEditor(typeof(Condition), true)]
    public class ConditionEditor : Editor
    {
        internal Editor parentEditor;

        public enum EditorType
        {
            ConditionAsset, AllConditionsAsset, ConditionCollection
        }

        public EditorType editorType;                       // The type of this Editor.
        public SerializedProperty conditionsProperty;       // The SerializedProperty representing an array of Conditions on a ConditionCollection.


        private SerializedProperty descriptionProperty;     // Represents a string description of this Editor's target.
        private SerializedProperty satisfiedProperty;       // Represents a bool of whether this Editor's target is satisfied.
        private SerializedProperty hashProperty;            // Represents the number that identified this Editor's target.
        
        private Condition condition;                        // Reference to the target.

        private Ability requiredAbility;
        private const float conditionButtonWidth = 30f;                     // Width in pixels of the button to remove this Condition from it's array.
        private const float toggleOffset = 30f;                             // Offset to line up the satisfied toggle with its label.
        private const string conditionPropDescriptionName = "_description";  // Name of the field that represents the description.
        private const string conditionPropSatisfiedName = "_satisfied";      // Name of the field that represents whether or not the Condition is satisfied.
        private const string conditionPropHashName = "_hash";                // Name of the field that represents the Condition's identifier.
        private const string blankDescription = "No conditions set.";       // Description to use in case no Conditions have been created yet.

        private const float buttonWidth = 30f;

        private void OnEnable()
        {
            condition = (Condition)target;

            if (target == null)
            {
                DestroyImmediate(this);
                return;
            }

            descriptionProperty = serializedObject.FindProperty(conditionPropDescriptionName);
            satisfiedProperty = serializedObject.FindProperty(conditionPropSatisfiedName);
            hashProperty = serializedObject.FindProperty(conditionPropHashName);
            }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
        
            switch (editorType)
            {
                case EditorType.AllConditionsAsset:
                    List();
                    break;
                case EditorType.ConditionAsset:
                    ConditionAssetGUI(); 
                    break;
                case EditorType.ConditionCollection:
                    List();
                    break;
                default:
                    throw new UnityException("Unknown ConditionEditor.EditorType.");
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void List()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            ConditionAssetGUI();

            EditorGUILayout.EndVertical();
        }

        private void ConditionAssetGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(condition.Description);
            EditorGUILayout.EndHorizontal();
            
            EditorGUI.indentLevel++;
            if (condition is AbilityCondition)
            {
                EditorGUILayout.BeginHorizontal();
                ((AbilityCondition)condition).ability = EditorGUILayout.ObjectField("Required Ability", ((AbilityCondition)condition).ability, typeof(Ability), false) as Ability;
                EditorGUILayout.EndHorizontal();
            }

            if (condition is InventoryCapacityCondition)
            {
                InventoryCapacityCondition capCondition = (InventoryCapacityCondition)condition;

                EditorGUILayout.BeginHorizontal();
                capCondition.targetNeedingCapacity = (InventoryCapacityCondition.Targets)EditorGUILayout.EnumPopup("Target needing capacity", capCondition.targetNeedingCapacity);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                capCondition.requiredCapacity = EditorGUILayout.IntField("Required Capacity", capCondition.requiredCapacity);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                capCondition.testType = (InventoryCapacityCondition.Tests)EditorGUILayout.EnumPopup("Capacity test type", capCondition.testType);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUI.indentLevel--;

            EditorGUILayout.EndVertical();
        }
    }
}