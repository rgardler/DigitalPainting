using System.Collections.Generic;
using UnityEngine;
using wizardscode.ability;
using wizardscode.digitalpainting.agent;

namespace wizardscode.interaction
{
    /// <summary>
    /// Although this is a Reaction class it's not really a reaction, 
    /// it allows us to trigger the Ability that is causing a set of
    /// reactions. 
    /// 
    /// If the ReactionCollection does not have an AbilityTrigger
    /// then the ability will be triggered before any reactions. However,
    /// by specifying this Reaction it is possible to make the ability
    /// trigger after some of the Reactions.
    /// </summary>
    public class AbilityTrigger : DelayedReaction
    {
        public Ability ability;

        protected override void ImmediateReaction(MonoBehaviour monoBehaviour, BaseAgentController interactor, Interactable interactable)
        {
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("endTransform", interactor.transform);
            monoBehaviour.StartCoroutine(ability.TriggerAbility(interactable.gameObject, options));
        }
    }
}