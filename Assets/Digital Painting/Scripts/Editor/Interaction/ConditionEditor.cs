using UnityEngine;
using UnityEditor;
using wizardscode.extension;
using wizardscode.ability;

namespace wizardscode.interaction
{
    [CustomEditor(typeof(Condition), true)]
    public class ConditionEditor : Editor
    {
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
                    AllConditionsAssetGUI();
                    break;
                case EditorType.ConditionAsset:
                    ConditionAssetGUI();
                    break;
                case EditorType.ConditionCollection:
                    InteractableGUI();
                    break;
                default:
                    throw new UnityException("Unknown ConditionEditor.EditorType.");
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void AllConditionsAssetGUI()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(condition.Description);
            if (GUILayout.Button("-", GUILayout.Width(conditionButtonWidth)))
                AllConditionsEditor.RemoveCondition(condition);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        private void ConditionAssetGUI()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

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

            EditorGUI.indentLevel--;

            EditorGUILayout.EndVertical();
        }


        private void InteractableGUI()
        {
            serializedObject.Update();

            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();

            int conditionIndex = AllConditionsEditor.TryGetConditionIndex(condition);

            if (conditionIndex == -1)
            {
                conditionIndex = 0;
            }

            conditionIndex = EditorGUILayout.Popup(conditionIndex, AllConditionsEditor.AllConditionDescriptions);
            Condition globalCondition = AllConditionsEditor.TryGetConditionAt(conditionIndex);
            descriptionProperty.stringValue = globalCondition != null ? globalCondition.Description : blankDescription;
            hashProperty.intValue = Animator.StringToHash(descriptionProperty.stringValue);

            GUI.enabled = false;
            EditorGUILayout.PropertyField(satisfiedProperty, GUIContent.none);
            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel--;

            serializedObject.ApplyModifiedProperties();
        }

        public static Condition CreateCondition<T>() where T : Condition
        {

            Condition newCondition = CreateInstance<T>();
            string blankDescription = "No conditions set.";

            Condition globalCondition = AllConditionsEditor.TryGetConditionAt(0);
            newCondition.Description = globalCondition != null ? globalCondition.Description : blankDescription;

            return newCondition;
        }

        public static Condition CreateCondition<T>(string description) where T : Condition
        {
            Condition newCondition = CreateInstance<T>();

            newCondition.Description = description;
            return newCondition;
        }
    }
}