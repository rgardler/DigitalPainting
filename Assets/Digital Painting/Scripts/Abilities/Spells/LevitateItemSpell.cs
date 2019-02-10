using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.digitalpainting;
using wizardscode.digitalpainting.agent;

namespace wizardscode.ability
{
    /// <summary>
    /// Levitate an item from a its current position to an ending position.
    /// 
    /// To use:
    /// 
    /// ```
    /// Dictionary<string, object> options = new Dictionary<string, object>();
    /// options.Add("endTransform", endPositionAndRotation);
    /// StartCoroutine(levitateItemSpell.TriggerAbility(targetObject, options));
    /// ```
    /// </summary>
    [CreateAssetMenu (menuName = "Wizards Code/Ability/Spell/Levitate Item")]
    public class LevitateItemSpell : Ability
    {
        [Tooltip("The end transform for the levitation, that is where the object will be moved to using levitation.")]
        public Transform endTransform;
        [Tooltip("Maximum range over which this spell will operate.")]
        public float maximumRange = 5f;
        [Tooltip("Maximum speed at which an item can be moved.")]
        public float maximumSpeed = 1f;

        private DigitalPaintingManager manager; 

        public override void Initialize()
        {
            manager = FindObjectOfType<DigitalPaintingManager>();
        }

        public override IEnumerator TriggerAbility()
        {
            Debug.LogError(name + " ability was triggered but this ability requires a target");
            return null;
        }

        public override IEnumerator TriggerAbility(BaseAgentController agent, GameObject target)
        {
            if (endTransform == null)
            {
                throw new MisconfiguredAbilityException("LevitateItemSpell requires that you set the value of " +
                    " 'endTransform' that identifies the final position and rotation of the item being levitated. " +
                    "This must be set before calling TriggerAbility(...).");
            }

            isActive = true;
            while (Vector3.SqrMagnitude(target.transform.position - endTransform.position) > 0.2)
            {
                float step = maximumSpeed * Time.deltaTime; // calculate distance to move
                target.transform.position = Vector3.MoveTowards(target.transform.position, endTransform.position, step);
                yield return new WaitForSeconds(0.03f);
            }
            isActive = false;
        }

        /// <summary>
        /// Called each frame by the PlayerBehaviour that controls this spel at runtime.
        /// </summary>
        public override void Update()
        {
            BaseAgentController agent = manager.AgentWithFocus;
            GameObject target = manager.AgentWithFocus.PointOfInterest.gameObject;
            endTransform = agent.transform;
            if (Vector3.SqrMagnitude(target.transform.position - endTransform.position) > 0.2)
            {
                float step = maximumSpeed * Time.deltaTime; // calculate distance to move
                target.transform.position = Vector3.MoveTowards(target.transform.position, endTransform.position, step);
            }
        }
    }
}
