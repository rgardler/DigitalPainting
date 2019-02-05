using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.interaction
{
    public class ConditionCollection : ScriptableObject
    {
        public string description;
        public Condition[] requiredConditions = new Condition[0];
        public ReactionCollection reactionCollection;

        public bool CheckAndReact()
        {
            for(int i = 0; i < requiredConditions.Length; i++)
            {
                if (!AllConditions.CheckCondition(requiredConditions[i]))
                {
                    return false;
                }
            }

            if(reactionCollection != null) {
                reactionCollection.React();
            }

            return true;
        }
    }
}
