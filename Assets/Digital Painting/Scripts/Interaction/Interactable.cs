
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
        [Tooltip("Automatically trigger when entering the trigger zone.")]
        public bool isTrigger = false;

        public InteractionCollection interactionsCollection;

        internal PlayableDirector playableDirector;
        private DigitalPaintingManager manager;

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

            if (interactionLocation == null)
            {
                Debug.LogError(gameObject.name + " has an `Interactable` component but the `InteractionLocation` property is not set.");
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
            if (interactionsCollection == null || interactionsCollection.interactions == null)
            {
                Debug.LogWarning("Interactable does not have interactions defined");
                return;
            }
            for (int i = 0; i < interactionsCollection.interactions.Length; i++)
            {
                if (interactionsCollection.interactions[i].CheckValidInteraction(interactor, this))
                {
                    manager.SetPlayableAsset(playableDirector, interactionsCollection.interactions[i].playableAsset);
                    playableDirector.Play();
                    return;
                }
            }
            return;
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
