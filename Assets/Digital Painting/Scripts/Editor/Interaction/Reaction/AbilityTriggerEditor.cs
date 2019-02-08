using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using wizardscode.ability;

namespace wizardscode.interaction
{
    [CustomEditor(typeof(AbilityTrigger))]
    public class AbilityTriggerEditor : ReactionEditor
    {
        private SerializedProperty delayProperty;

        private const string textReactionPropDelayName = "delay";

        protected override void Init()
        {
            delayProperty = serializedObject.FindProperty(textReactionPropDelayName);
        }

        protected override string GetFoldoutLabel()
        {
            return "Ability Trigger";
        }
    }
}