
using UnityEngine;
using UnityEngine.Playables;
using wizardscode.digitalpainting;
using wizardscode.digitalpainting.agent;

namespace wizardscode.interaction
{
    public class Interactable : MonoBehaviour
    {
        public Transform interactionLocation;
        public Sprite sprite;
        public PlayableAsset defaultPlayableAsset;
        [Tooltip("Automatically trigger when entering the trigger zone.")]
        public bool isTrigger = false;

        public ConditionCollection[] conditionCollections = new ConditionCollection[0];

        private  PlayableDirector playableDirector;
        private DigitalPaintingManager manager;

        private void Awake()
        {
        }

        private void Start()
        {
            manager = FindObjectOfType<DigitalPaintingManager>();
            if (manager == null)
            {
                Debug.LogError("Can't find a DigitalPaintingManager in the scene, this is required.");
            }

            playableDirector = GetComponent<PlayableDirector>();
            if (playableDirector == null)
            {
                playableDirector = gameObject.AddComponent<PlayableDirector>();
            }

            if (defaultPlayableAsset == null)
            {
                Debug.LogError(gameObject.name + " has an `Interactable` component but no `defaultPlayableAsset` has been set. This is required.");
            }

            if (interactionLocation == null)
            {
                Debug.LogError(gameObject.name + " has an `Interactable` component but the `InteractionLocation` property is not set.");
            }

            manager.SetPlayableAsset(playableDirector, defaultPlayableAsset);
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

        private void OnTriggerEnter(Collider other)
        {
            BaseAgentController agent = other.gameObject.GetComponentInParent<BaseAgentController>();
            if (agent != null)
            {
                agent.Interactable = this;
                Interact(agent);
            }            
        }
    }
}
