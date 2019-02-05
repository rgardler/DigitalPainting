using UnityEngine;
using UnityEditor;
using wizardscode.extension;

namespace wizardscode.interaction
{
    [CustomEditor(typeof(ConditionCollection))]
    public class ConditionCollectionEditor : EditorWithSubEditors<ConditionEditor, Condition>
    {
        public SerializedProperty collectionsProperty;              // Represents the array of ConditionCollections that the target belongs to.


        private ConditionCollection conditionCollection;
        private SerializedProperty descriptionProperty;
        private SerializedProperty conditionsProperty;
        private SerializedProperty reactionCollectionProperty;

        private const float conditionButtonWidth = 30f;
        private const float collectionButtonWidth = 125f;
        private const string conditionCollectionPropDescriptionName = "description";
        private const string conditionCollectionPropRequiredConditionsName = "requiredConditions";
        private const string conditionCollectionPropReactionCollectionName = "reactionCollection";
        
        private void OnEnable()
        {
            conditionCollection = (ConditionCollection)target;

            if (target == null)
            {
                DestroyImmediate(this);
                return;
            }

            descriptionProperty = serializedObject.FindProperty(conditionCollectionPropDescriptionName);
            conditionsProperty = serializedObject.FindProperty(conditionCollectionPropRequiredConditionsName);
            reactionCollectionProperty = serializedObject.FindProperty(conditionCollectionPropReactionCollectionName);

            CheckAndCreateSubEditors(conditionCollection.requiredConditions);
        }

        private void OnDisable()
        {
            CleanupEditors();
        }

        protected override void SubEditorSetup(ConditionEditor editor)
        {
            editor.editorType = ConditionEditor.EditorType.ConditionCollection;
            editor.conditionsProperty = conditionsProperty;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            CheckAndCreateSubEditors(conditionCollection.requiredConditions);

            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();

            descriptionProperty.isExpanded = EditorGUILayout.Foldout(descriptionProperty.isExpanded, descriptionProperty.stringValue);

            if (GUILayout.Button("Remove Collection", GUILayout.Width(collectionButtonWidth)))
            {
                collectionsProperty.RemoveFromObjectArray(conditionCollection);
            }

            EditorGUILayout.EndHorizontal();

            if (descriptionProperty.isExpanded)
            {
                ExpandedGUI();
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        private void ExpandedGUI()
        {
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(descriptionProperty);

            EditorGUILayout.Space();

            float space = EditorGUIUtility.currentViewWidth / 3f;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Condition", GUILayout.Width(space));
            EditorGUILayout.LabelField("Satisfied?", GUILayout.Width(space));
            EditorGUILayout.LabelField("Add/Remove", GUILayout.Width(space));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            for (int i = 0; i < subEditors.Length; i++)
            {
                subEditors[i].OnInspectorGUI();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("+", GUILayout.Width(conditionButtonWidth)))
            {
                Condition newCondition = ConditionEditor.CreateCondition();
                conditionsProperty.AddToObjectArray(newCondition);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(reactionCollectionProperty);
        }


        public static ConditionCollection CreateConditionCollection()
        {
            ConditionCollection newConditionCollection = CreateInstance<ConditionCollection>();

            newConditionCollection.description = "New condition collection";

            newConditionCollection.requiredConditions = new Condition[1];
            newConditionCollection.requiredConditions[0] = ConditionEditor.CreateCondition();
            return newConditionCollection;
        }
    }
}