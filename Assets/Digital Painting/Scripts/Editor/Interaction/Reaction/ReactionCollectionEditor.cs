using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using wizardscode.extension;
using wizardscode.ability;

namespace wizardscode.interaction
{
    [CustomEditor(typeof(ReactionCollection))]
    public class ReactionCollectionEditor : EditorWithSubEditors<ReactionEditor, Reaction>
    {
        private ReactionCollection reactionCollection;
        private SerializedProperty reactionsProperty;
        private SerializedProperty abilityProperty;

        private Type[] reactionTypes;
        private string[] reactionTypeNames;
        private int selectedIndex;

        private const float dropAreaHeight = 50f;
        private const float controlSpacing = 5f;
        private const string reactionsPropName = "reactions";
        private const string abilityPropName = "ability";

        private readonly float verticalSpacing = EditorGUIUtility.standardVerticalSpacing;

        private void OnEnable()
        {
            reactionCollection = (ReactionCollection)target;

            reactionsProperty = serializedObject.FindProperty(reactionsPropName);
            abilityProperty = serializedObject.FindProperty(abilityPropName);

            CheckAndCreateSubEditors(reactionCollection.reactions);

            SetReactionNamesArray();
        }


        private void OnDisable()
        {
            CleanupEditors();
        }


        protected override void SubEditorSetup(ReactionEditor editor)
        {
            editor.reactionsProperty = reactionsProperty;
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.ObjectField(abilityProperty, typeof(Ability), new GUIContent("Ability"));

            CheckAndCreateSubEditors(reactionCollection.reactions);

            for (int i = 0; i < subEditors.Length; i++)
            {
                subEditors[i].OnInspectorGUI();
            }

            if (reactionCollection.reactions.Length > 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }

            Rect fullWidthRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(dropAreaHeight + verticalSpacing));

            Rect leftAreaRect = fullWidthRect;

            leftAreaRect.y += verticalSpacing * 0.5f;

            leftAreaRect.width *= 0.5f;
            leftAreaRect.width -= controlSpacing * 0.5f;

            leftAreaRect.height = dropAreaHeight;

            Rect rightAreaRect = leftAreaRect;

            rightAreaRect.x += rightAreaRect.width + controlSpacing;

            TypeSelectionGUI(leftAreaRect);

            DragAndDropAreaGUI(rightAreaRect);

            DraggingAndDropping(rightAreaRect, this);

            serializedObject.ApplyModifiedProperties();
        }

        private void TypeSelectionGUI(Rect containingRect)
        {
            Rect topHalf = containingRect;
            topHalf.height *= 0.5f;
            Rect bottomHalf = topHalf;
            bottomHalf.y += bottomHalf.height;

            selectedIndex = EditorGUI.Popup(topHalf, selectedIndex, reactionTypeNames);

            if (GUI.Button(bottomHalf, "Add Selected Reaction"))
            {
                Type reactionType = reactionTypes[selectedIndex];
                Reaction newReaction = ReactionEditor.CreateReaction(reactionType);
                reactionsProperty.AddToObjectArray(newReaction);
            }
        }

        private static void DragAndDropAreaGUI(Rect containingRect)
        {
            GUIStyle centredStyle = GUI.skin.box;
            centredStyle.alignment = TextAnchor.MiddleCenter;
            centredStyle.normal.textColor = GUI.skin.button.normal.textColor;

            GUI.Box(containingRect, "Drop new Reactions here", centredStyle);
        }

        private static void DraggingAndDropping(Rect dropArea, ReactionCollectionEditor editor)
        {
            Event currentEvent = Event.current;

            if (!dropArea.Contains(currentEvent.mousePosition))
                return;

            switch (currentEvent.type)
            {
                case EventType.DragUpdated:

                    DragAndDrop.visualMode = IsDragValid() ? DragAndDropVisualMode.Link : DragAndDropVisualMode.Rejected;

                    currentEvent.Use();

                    break;

                case EventType.DragPerform:

                    DragAndDrop.AcceptDrag();

                    for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                    {
                        MonoScript script = DragAndDrop.objectReferences[i] as MonoScript;

                        Type reactionType = script.GetClass();

                        Reaction newReaction = ReactionEditor.CreateReaction(reactionType);
                        editor.reactionsProperty.AddToObjectArray(newReaction);
                    }

                    currentEvent.Use();

                    break;
            }
        }

        private static bool IsDragValid()
        {
            for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
            {
                if (DragAndDrop.objectReferences[i].GetType() != typeof(MonoScript))
                    return false;

                MonoScript script = DragAndDrop.objectReferences[i] as MonoScript;
                Type scriptType = script.GetClass();

                if (!scriptType.IsSubclassOf(typeof(Reaction)))
                    return false;

                if (scriptType.IsAbstract)
                    return false;
            }

            return true;
        }


        private void SetReactionNamesArray()
        {
            Type reactionType = typeof(Reaction);

            Type[] allTypes = reactionType.Assembly.GetTypes();

            List<Type> reactionSubTypeList = new List<Type>();

            for (int i = 0; i < allTypes.Length; i++)
            {
                if (allTypes[i].IsSubclassOf(reactionType) && !allTypes[i].IsAbstract)
                {
                    reactionSubTypeList.Add(allTypes[i]);
                }
            }

            reactionTypes = reactionSubTypeList.ToArray();

            List<string> reactionTypeNameList = new List<string>();

            for (int i = 0; i < reactionTypes.Length; i++)
            {
                reactionTypeNameList.Add(reactionTypes[i].Name);
            }

            reactionTypeNames = reactionTypeNameList.ToArray();
        }
    }
}