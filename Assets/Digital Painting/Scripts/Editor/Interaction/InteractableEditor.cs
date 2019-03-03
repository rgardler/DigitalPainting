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
        private const string interactablePropInteractionCollectionName = "interactionCollection";

        private void OnEnable()
        {
            interactable = (Interactable)target;

            interactionCollectionProperty = serializedObject.FindProperty(interactablePropInteractionCollectionName);
            interactionLocationProperty = serializedObject.FindProperty(interactablePropInteractionLocationName);
            spriteProperty = serializedObject.FindProperty(interactablePropertySpritePropName);

            CheckAndCreateSubEditors(interactable.interactionCollection);
        }

        private void OnDisable()
        {
            CleanupEditors();
        }

        protected override void SubEditorSetup(InteractionEditor editor)
        {
            editor.collectionsProperty = interactionCollectionProperty;
            editor.editorType = InteractionEditor.EditorType.InteractionCollection;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            CheckAndCreateSubEditors(interactable.interactionCollection);

            EditorGUILayout.PropertyField(interactionLocationProperty);
            EditorGUILayout.ObjectField(spriteProperty);

            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Interactions");
            for (int i = 0; i < subEditors.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                subEditors[i].OnInspectorGUI();

                if (GUILayout.Button("-", GUILayout.Width(collectionButtonWidth)))
                {
                    interactionCollectionProperty.RemoveFromObjectArrayAt(i);
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("+", GUILayout.Width(collectionButtonWidth)))
            {
                Interaction newInteraction = InteractionEditor.CreateInteraction("New Interaction");
                interactionCollectionProperty.AddToObjectArray(newInteraction);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();
        }
    }
}