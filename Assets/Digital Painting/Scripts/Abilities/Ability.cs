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
        /// Called to start the use of the ability.
        /// </summary>
        public virtual void Start()
        {
            isActive = true;
        }

        /// <summary>
        /// Called to initialize the Ability during the Start phase of the application.
        /// You can put expensive configuration in here.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Called each frame by the PlayerBehaviour that controls this spel at runtime.
        /// </summary>
        public virtual void Update() { }
    }
}
