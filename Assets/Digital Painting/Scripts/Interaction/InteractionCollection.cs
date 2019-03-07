using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.interaction
{
    [CreateAssetMenu(fileName = "InteractionsCollection", menuName = "Wizards Code/Interactions/Collection", order = 980)]
    public class InteractionCollection : ResettableScriptableObject
    {
        public Interaction[] interactions = new Interaction[0];
        
        public override void Reset()
        {
            if (interactions == null)
            {
                return;
            }

            for (int i = 0; i < interactions.Length; i++)
            {
                interactions[i].Reset();
            }
        }
    }
}
