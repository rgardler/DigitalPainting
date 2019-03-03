using UnityEngine;
using UnityEditor;
using wizardscode.extension;

namespace wizardscode.interaction
{
    [CustomEditor(typeof(Interactable))]
    public class InteractableEditor : EditorWithSubEditors<ConditionCollectionEditor, ConditionCollection>
    {
        private Interactable interactable;
        private SerializedProperty interactionLocationProperty;
        private SerializedProperty spriteProperty;
        private SerializedProperty collectionsProperty;
        private SerializedProperty defaultPlayableAssetProperty;

        private const float collectionButtonWidth = 125f;
        private const string interactablePropInteractionLocationName = "interactionLocation";
        private const string interactablePropertySpritePropName = "sprite";
        private const string interactablePropConditionCollectionsName = "conditionCollections";
        private const string interactablePropDefaultPlayableAssetName = "defaultPlayableAsset";

        private void OnEnable()
        {
            interactable = (Interactable)target;

            collectionsProperty = serializedObject.FindProperty(interactablePropConditionCollectionsName);
            interactionLocationProperty = serializedObject.FindProperty(interactablePropInteractionLocationName);
            spriteProperty = serializedObject.FindProperty(interactablePropertySpritePropName);
            defaultPlayableAssetProperty = serializedObject.FindProperty(interactablePropDefaultPlayableAssetName);

            CheckAndCreateSubEditors(interactable.conditionCollections);
        }

        private void OnDisable()
        {
            CleanupEditors();
        }

        protected override void SubEditorSetup(ConditionCollectionEditor editor)
        {
            editor.collectionsProperty = collectionsProperty;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            CheckAndCreateSubEditors(interactable.conditionCollections);

            EditorGUILayout.PropertyField(interactionLocationProperty);
            EditorGUILayout.ObjectField(spriteProperty);
            EditorGUILayout.PropertyField(defaultPlayableAssetProperty);

            for (int i = 0; i < subEditors.Length; i++)
            {
                subEditors[i].OnInspectorGUI();
                EditorGUILayout.Space();
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add Conditions", GUILayout.Width(collectionButtonWidth)))
            {
                ConditionCollection newCollection = ConditionCollectionEditor.CreateConditionCollection();
                collectionsProperty.AddToObjectArray(newCollection);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();
        }
    }
}