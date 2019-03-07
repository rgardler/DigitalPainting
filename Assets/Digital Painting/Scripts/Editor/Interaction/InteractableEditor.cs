using UnityEngine;
using UnityEditor;
using wizardscode.extension;

namespace wizardscode.interaction
{
    [CustomEditor(typeof(Interactable))]
    public class InteractableEditor : EditorWithSubEditors<InteractionEditor, Interaction>
    {
        private Interactable interactable;
        private SerializedProperty interactionLocationProperty;
        private SerializedProperty spriteProperty;
        private SerializedProperty interactionCollectionProperty;
       
        private const float collectionButtonWidth = 30f;
        private const string interactablePropInteractionLocationName = "interactionLocation";
        private const string interactablePropertySpritePropName = "sprite";
        private const string interactablePropInteractionCollectionName = "interactionsCollection";
        
        private void OnEnable()
        {
            interactable = (Interactable)target;

            interactionCollectionProperty = serializedObject.FindProperty(interactablePropInteractionCollectionName);
            interactionLocationProperty = serializedObject.FindProperty(interactablePropInteractionLocationName);
            spriteProperty = serializedObject.FindProperty(interactablePropertySpritePropName);

            if (interactable.interactionsCollection != null)
            {
                CheckAndCreateSubEditors(interactable.interactionsCollection.interactions);
            }
        }

        private void OnDisable()
        {
            CleanupEditors();
        }

        protected override void SubEditorSetup(InteractionEditor editor)
        {
            editor.collectionsProperty = interactionCollectionProperty;
            editor.editorType = InteractionEditor.EditorType.InteractionAsset;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(interactionLocationProperty);
            EditorGUILayout.ObjectField(spriteProperty);
            EditorGUILayout.PropertyField(interactionCollectionProperty);
            
            if (interactable.interactionsCollection != null)
            {
                CheckAndCreateSubEditors(interactable.interactionsCollection.interactions);
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.LabelField("Interactions");
                for (int i = 0; i < subEditors.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    subEditors[i].OnInspectorGUI();
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();
        }
    }
}