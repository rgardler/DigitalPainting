using UnityEngine;

namespace wizardscode.ability
{
    public class AbilityCollection : MonoBehaviour
    {
        public Ability[] abilities = new Ability[0];

        private void Start()
        {
            for (int i = 0; i < abilities.Length; i++)
            {
                abilities[i].Initialize();
            }
        }
    }
}
