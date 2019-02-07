using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public abstract void Initialize();

        /// <summary>
        /// Trigger the ability to take action.
        /// </summary>
        public abstract IEnumerator TriggerAbility(Dictionary<string, object> options = null);

        /// <summary>
        /// Trigger the ability to affect a specific target.
        /// </summary>
        public abstract IEnumerator TriggerAbility(GameObject target, Dictionary<string, object> options = null);
    }
}
