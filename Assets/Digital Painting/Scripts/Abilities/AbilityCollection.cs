using System;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.ability
{
    public class AbilityCollection : MonoBehaviour
    {
        public List<Ability> abilities = new List<Ability>();

        private void Start()
        {
            for (int i = 0; i < abilities.Count; i++)
            {
                abilities[i].Initialize();
            }
        }

        internal bool Contains(Ability ability)
        {
            return abilities.Contains(ability);
        }
    }
}
