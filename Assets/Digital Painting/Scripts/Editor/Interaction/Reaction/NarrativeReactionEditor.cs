using UnityEditor;
using UnityEngine;

namespace wizardscode.interaction
{
    [CustomEditor(typeof(NarrativeReaction))]
    public class NarrativeReactionEditor : ReactionEditor
    {
        private SerializedProperty messageProperty;
        private SerializedProperty textColorProperty;
        private SerializedProperty delayProperty;


        private const float messageGUILines = 3f;
        private const float areaWidthOffset = 19f;
        private const string textReactionPropMessageName = "message";
        private const string textReactionPropTextColorName = "textColor";
        private const string textReactionPropDelayName = "delay";
        
        protected override void Init()
        {
            messageProperty = serializedObject.FindProperty(textReactionPropMessageName);
            textColorProperty = serializedObject.FindProperty(textReactionPropTextColorName);
            delayProperty = serializedObject.FindProperty(textReactionPropDelayName);
        }

        protected override void DrawReaction()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Message", GUILayout.Width(EditorGUIUtility.labelWidth - areaWidthOffset));

            messageProperty.stringValue = EditorGUILayout.TextArea(messageProperty.stringValue, GUILayout.Height(EditorGUIUtility.singleLineHeight * messageGUILines));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(textColorProperty);
            EditorGUILayout.PropertyField(delayProperty);
        }

        protected override string GetFoldoutLabel()
        {
            return "Narrative Reaction";
        }
    }
}