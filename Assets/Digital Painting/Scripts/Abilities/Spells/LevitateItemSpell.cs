using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.digitalpainting;
using wizardscode.digitalpainting.agent;
using wizardscode.interaction;

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
        [Tooltip("The end transform for the levitation, that is where the object will be moved to using levitation. If this is null in the inspector it must be set before the spell is cast.")]
        public Transform endTransform;
        [Tooltip("The item being levitated. If this is set to null in the inspector it must be set to a value before the spell is cast.")]
        public Interactable item;
        [Tooltip("Maximum range over which this spell will operate.")]
        public float maximumRange = 5f;
        [Tooltip("Maximum speed at which an item can be moved.")]
        public float maximumSpeed = 1f;
        [Tooltip("Maximum speed the spell will rotate an object")]
        public float maximumRotationSpeed = 180;

        private DigitalPaintingManager manager; 

        public override void Initialize()
        {
            manager = FindObjectOfType<DigitalPaintingManager>();
        }

        /// <summary>
        /// Called each frame by the PlayerBehaviour that controls this spell at runtime.
        /// </summary>
        public override void Update()
        {
            if (Vector3.SqrMagnitude(item.transform.position - endTransform.position) > 0.005)
            {
                isActive = true;
                float moveStep = maximumSpeed * Time.deltaTime;
                float rotateStep = maximumRotationSpeed * Time.deltaTime;
                item.transform.position = Vector3.MoveTowards(item.transform.position, endTransform.position, moveStep);
                item.transform.rotation = Quaternion.RotateTowards(item.transform.rotation, endTransform.rotation, rotateStep);
            }
            else
            {
                isActive = false;
            }
        }
    }
}
