using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment.thing
{
    public class CollectInteraction : AbstractInteraction
    {
        public override void Interact(GameObject interactor)
        {
            Debug.Log(interactor.name + " can now interact with " + interactor.name);
        }
    }
}