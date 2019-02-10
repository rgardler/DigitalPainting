
using UnityEngine;
using UnityEngine.Playables;
using wizardscode.digitalpainting.agent;

namespace wizardscode.interaction
{
    public class Interactable : MonoBehaviour
    {
        public Transform interactionLocation;
        public Sprite sprite;

        public ConditionCollection[] conditionCollections = new ConditionCollection[0];

        private  PlayableDirector playableDirector;


        private void Start()
        {
            playableDirector = GetComponent<PlayableDirector>();
            if (playableDirector == null)
            {
                Debug.LogError(gameObject.name + " has an Interactable component, but it does not have a PlayableDirector, which is required.");
            }
        }

        /// <summary>
        /// Interact with the interactable using an ability.
        /// 
        /// The correct collection of reactions will be identified by checking
        /// the conditions for each collection. When the conditions are satisfied
        /// the ability and reactions will be executed.
        /// </summary>
        /// <param name="interactor">The GameObject using the ability to interact.</param>
        /// <param name="ability">The ability to perform at the indicated spot within the ReactionsCollection.</param>
        public void Interact(BaseAgentController interactor = null)
        {    
            if (conditionCollections.Length == 0)
            {
                playableDirector. Play();
            }
            for (int i = 0; i < conditionCollections.Length; i++)
            {
                if (conditionCollections[i].CheckAndReact(interactor, this))
                {
                    return;
                }

                playableDirector.Play();
            }
        }
    }
}
