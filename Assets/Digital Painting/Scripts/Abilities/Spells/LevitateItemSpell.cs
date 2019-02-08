using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        [Tooltip("Maximum range over which this spell will operate.")]
        public float maximumRange = 5f;
        [Tooltip("Maximum speed at which an item can be moved.")]
        public float maximumSpeed = 1f;

        public override void Initialize()
        {

        }

        public override IEnumerator TriggerAbility(Dictionary<string, object> options = null)
        {
            Debug.LogError(name + " ability was triggered but this ability requires a target");
            return null;
        }

        public override IEnumerator TriggerAbility(GameObject target, Dictionary<string, object> options = null)
        {
            if (options == null)
            {
                throw new KeyNotFoundException("LevitateItemSpell requires a 'endTransform' option that identifies the final position and rotation of the item being levitated.");
            }
            Transform endTransform = (Transform)options["endTransform"];
            if (endTransform == null)
            {
                throw new KeyNotFoundException("LevitateItemSpell requires a 'endTransform' option that identifies the final position and rotation of the item being levitated.");
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
    }
}
