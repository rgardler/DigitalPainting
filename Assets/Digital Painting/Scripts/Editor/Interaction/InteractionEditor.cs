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
        private SerializedProperty requiredConditionsProperty;
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
            requiredConditionsProperty = serializedObject.FindProperty(interactionCollectionPropRequiredConditionsName);
            playableAssetProperty = serializedObject.FindProperty(interactionCollectionPropPlayableAssetName);
            hashProperty = serializedObject.FindProperty(interactionPropHashName);

            if (interaction.requiredConditions != null)
            {
                CheckAndCreateSubEditors(interaction.requiredConditions.conditions);
            }
        }

        private void OnDisable()
        {
            CleanupEditors();
        }

        protected override void SubEditorSetup(ConditionEditor editor)
        {
            editor.editorType = ConditionEditor.EditorType.ConditionCollection;
            editor.conditionsProperty = requiredConditionsProperty;
            editor.parentEditor = this;
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
            //EditorGUILayout.LabelField(interaction.Description);
            InteractionAssetGUI();
            if (GUILayout.Button("-", GUILayout.Width(interactionButtonWidth)))
            {
                ((InteractionCollectionEditor)parentEditor).RemoveInteraction(interaction);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        private void InteractionAssetGUI()
        {
            EditorGUILayout.BeginVertical();

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
            InteractionCollectionEditor parent = ((InteractionCollectionEditor)parentEditor);

            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            int interactionIndex = parent.TryGetInteractionIndex(this.interaction);
            if (interactionIndex == -1)
            {
                interactionIndex = 0;
            }

            interactionIndex = EditorGUILayout.Popup(interactionIndex, parent.AllInteractionDescriptions);
            interaction = parent.TryGetInteractionAt(interactionIndex);
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
            EditorGUILayout.PropertyField(requiredConditionsProperty);
            EditorGUILayout.Space();

            if (interaction.requiredConditions != null)
            {
                CheckAndCreateSubEditors(interaction.requiredConditions.conditions);
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.LabelField("Conditions");
                for (int i = 0; i < subEditors.Length; i++)
                {
                    subEditors[i].OnInspectorGUI();
                }

                EditorGUILayout.EndVertical();
            }
        }

        internal void AddCondition(string description)
        {
            Condition newCondition = CreateInstance<Condition>();
            newCondition.name = newCondition.Description= description;

            Undo.RecordObject(newCondition, "Created new Condition");
            AssetDatabase.AddObjectToAsset(newCondition, interaction);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newCondition));
            ArrayUtility.Add(ref interaction.requiredConditions.conditions, newCondition);
            EditorUtility.SetDirty(interaction.requiredConditions);
        }
    }
}