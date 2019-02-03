using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment.thing
{
    public abstract class AbstractInteraction : MonoBehaviour
    {
        /// <summary>
        /// Called whenever a game object moves within the interaction trigger zone for a Thing.
        /// </summary>
        /// <param name="interactor"></param>
        abstract public void Interact(GameObject interactor);
    }
}
