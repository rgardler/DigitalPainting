using UnityEngine;
using UnityEditor;
using wizardscode.extension;

namespace wizardscode.interaction
{
    [CustomEditor(typeof(Interaction))]
    public class InteractionEditor : EditorWithSubEditors<ConditionEditor, Condition>
    {
        public enum EditorType
        {
            InteractionAsset, AllInteractionsAsset, InteractionCollection
        }
        public EditorType editorType;

        public SerializedProperty collectionsProperty; 
        
        private Interaction interaction;
        private SerializedProperty descriptionProperty;
        private SerializedProperty conditionsProperty;
        private SerializedProperty playableAssetProperty;
        private SerializedProperty hashProperty;

        private const float interactionButtonWidth = 30f;
        private const string interactionCollectionPropDescriptionName = "Description";
        private const string interactionCollectionPropRequiredConditionsName = "requiredConditions";
        private const string interactionCollectionPropPlayableAssetName = "playableAsset";
        private const string interactionPropHashName = "_hash";

        private void OnEnable()
        {
            interaction = (Interaction)target;

            if (target == null)
            {
                DestroyImmediate(this);
                return;
            }

            descriptionProperty = serializedObject.FindProperty(interactionCollectionPropDescriptionName);
            conditionsProperty = serializedObject.FindProperty(interactionCollectionPropRequiredConditionsName);
            playableAssetProperty = serializedObject.FindProperty(interactionCollectionPropPlayableAssetName);
            hashProperty = serializedObject.FindProperty(interactionPropHashName);

            CheckAndCreateSubEditors(interaction.requiredConditions);
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

            switch (editorType)
            {
                case EditorType.AllInteractionsAsset:
                    AllInteractionsAssetGUI();
                    break;
                case EditorType.InteractionAsset:
                    InteractionAssetGUI();
                    break;
                case EditorType.InteractionCollection:
                    InteractionCollectionGUI();
                    break;
                default:
                    throw new UnityException("Unknown InteractionEditor.EditorType.");
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void AllInteractionsAssetGUI()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(interaction.Description);
            if (GUILayout.Button("-", GUILayout.Width(interactionButtonWidth)))
                AllInteractionsEditor.RemoveInteraction(interaction);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        private void InteractionAssetGUI()
        {
            CheckAndCreateSubEditors(interaction.requiredConditions);

            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.BeginHorizontal();

            descriptionProperty.isExpanded = EditorGUILayout.Foldout(descriptionProperty.isExpanded, descriptionProperty.stringValue);

            EditorGUILayout.EndHorizontal();

            if (descriptionProperty.isExpanded)
            {
                ExpandedGUI();
            }
            EditorGUILayout.EndVertical();
        }

        private void InteractionCollectionGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            int interactionIndex = AllInteractionsEditor.TryGetInteractionIndex(this.interaction);
            if (interactionIndex == -1)
            {
                interactionIndex = 0;
            }

            interactionIndex = EditorGUILayout.Popup(interactionIndex, AllInteractionsEditor.AllInteractionDescriptions);
            interaction = AllInteractionsEditor.TryGetInteractionAt(interactionIndex);
            descriptionProperty.stringValue = interaction != null ? interaction.Description : "No Interaction Set";
            hashProperty.intValue = Animator.StringToHash(descriptionProperty.stringValue);
            playableAssetProperty.objectReferenceValue = interaction != null ? interaction.playableAsset : null;

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        private void ExpandedGUI()
        {
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(descriptionProperty);
            EditorGUILayout.PropertyField(playableAssetProperty);
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Conditions");
            for (int i = 0; i < subEditors.Length; i++)
            {
                subEditors[i].OnInspectorGUI();
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("+", GUILayout.Width(interactionButtonWidth)))
            {
                Condition newCondition = ConditionEditor.CreateCondition<Condition>();
                conditionsProperty.AddToObjectArray(newCondition);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }


        public static Interaction CreateInteraction(string description)
        {
            Interaction newConditionCollection = CreateInstance<Interaction>();

            newConditionCollection.Description = description;

            newConditionCollection.requiredConditions = new Condition[1];
            newConditionCollection.requiredConditions[0] = ConditionEditor.CreateCondition<Condition>();
            return newConditionCollection;
        }
    }
}