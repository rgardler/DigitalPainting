using System;
using UnityEngine;
using UnityEditor;
using wizardscode.extension;

namespace wizardscode.interaction
{
    public abstract class ReactionEditor : Editor
    {
        public bool showReaction;
        public SerializedProperty reactionsProperty;

        private Reaction reaction;
        private const float buttonWidth = 30f;

        private void OnEnable()
        {
            reaction = (Reaction)target;
            Init();
        }

        protected virtual void Init() { }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();

            showReaction = EditorGUILayout.Foldout(showReaction, GetFoldoutLabel());

            if (GUILayout.Button("-", GUILayout.Width(buttonWidth)))
            {
                reactionsProperty.RemoveFromObjectArray(reaction);
            }
            EditorGUILayout.EndHorizontal();

            if (showReaction)
            {
                DrawReaction();
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }


        public static Reaction CreateReaction(Type reactionType)
        {
            return (Reaction)CreateInstance(reactionType);
        }


        protected virtual void DrawReaction()
        {
            DrawDefaultInspector();
        }
        protected abstract string GetFoldoutLabel();
    }
}