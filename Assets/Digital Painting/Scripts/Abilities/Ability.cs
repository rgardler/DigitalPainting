using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.digitalpainting.agent;

namespace wizardscode.ability
{
    public abstract class Ability : ScriptableObject
    {
        new public string name = "New Ability";
        public Sprite sprite;
        public AudioClip sound;
        public float baseCoolDown = 1f;
        [HideInInspector] public bool isActive = false;

        public GameObject Target { get; internal set; }

        /// <summary>
        /// Called to initialize the Ability during the Start phase of the application.
        /// You can put expensive configuration in here.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Trigger the ability to take action.
        /// </summary>
        /// [Obsolete("TriggerAbility is deprecated. All functionality using this method should be transfered to timeline behaviours.")]
        public abstract IEnumerator TriggerAbility();

        /// <summary>
        /// Trigger the ability to affect a specific target.
        /// </summary>
        /// [Obsolete("TriggerAbility is deprecated. All functionality using this method should be transfered to timeline behaviours.")]
        public abstract IEnumerator TriggerAbility(BaseAgentController agent, GameObject target);

        /// <summary>
        /// Called each frame by the PlayerBehaviour that controls this spel at runtime.
        /// </summary>
        public virtual void Update() { }
    }
}
