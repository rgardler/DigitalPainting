using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.environment;
using wizardscode.interaction;

namespace wizardscode.digitalpainting.agent
{
    public class AIAgentController : BaseAgentController
    {
        [Header("AI Controller")]
        [Tooltip("Is the agent automated or manual movement?")]
        public bool isFlyByWire = true;

        [Header("Objects of Interest")]
        [Tooltip("The range the agent will use to detect things in its environment")]
        public float detectionRange = 50;
        [Tooltip("The range at which the agent considers things to be withing reach - that is able to touch.")]
        public float reachRange = 2;

        [Header("Wander configuration")]
        [Tooltip("Number of positions to try, per frame, when trying to find a valid wander target.")]
        public int maxWanderTargetRetries = 3;
        [Tooltip("Minimum time between random variations in the path.")]
        [Range(0, 120)]
        public float minTimeBetweenRandomPathChanges = 5;
        [Tooltip("Maximum time between random variations in the path.")]
        [Range(0, 120)]
        public float maxTimeBetweenRandomPathChanges = 15;
        [Tooltip("Minimum angle to change path when randomly varying")]
        [Range(-180, 180)]
        public float minAngleOfRandomPathChange = -25;
        [Tooltip("Maximum angle to change path when randomly varying")]
        [Range(-180, 180)]
        public float maxAngleOfRandomPathChange = 25;
        [Tooltip("Minimum distance to set a new wander target.")]
        [Range(1, 100)]
        public float minDistanceOfRandomPathChange = 10;
        [Tooltip("Maximum distance to set a new wander target.")]
        [Range(1, 100)]
        public float maxDistanceOfRandomPathChange = 25;

        [Header("Overrides")]
        [Tooltip("Set of objects within which the agent must stay. Each object must have a collider and non-kinematic rigid body. If null a default object will be searched for using the name `" + DEFAULT_BARRIERS_NAME + "`.")]
        public GameObject barriers;

        internal const string DEFAULT_BARRIERS_NAME = "AI Barriers";

        internal Quaternion targetRotation;
        internal float timeToNextWanderPathChange = 3;
        internal Thing _pointOfInterest;
        internal bool _interactWithPOI;
        private float timeLeftLookingAtObject = float.NegativeInfinity;
        private List<Thing> visitedThings = new List<Thing>();
        internal List<Thing> nextThings = new List<Thing>();

        /// <summary>
        /// Set the thing of interest for this agent. The agent will behave
        /// appropriately in response to the new thing of interest. The
        /// Thing is only updated if it has changed since the last time it was set.
        /// </summary>
        public Thing PointOfInterest
        {
            get { return _pointOfInterest; }
            set
            {
                if (value == null)
                {
                    _pointOfInterest = null;
                    _interactWithPOI = false;
                    return;
                }

                if (!GameObject.ReferenceEquals(_pointOfInterest, value))
                {
                    _pointOfInterest = value;

                    // Decide if we will interact with the POI when we get there
                    if (PointOfInterest != null)
                    {
                        Interactable interactable = PointOfInterest.gameObject.GetComponentInChildren<Interactable>();
                        _interactWithPOI = interactable != null;
                    }
                }
            }
        }

        virtual internal void Awake()
        {
            bool hasCollider = false;
            Collider[] colliders = gameObject.GetComponents<Collider>();
            for (int i = 0; i < colliders.Length; i++)
            {
                if (!colliders[i].isTrigger)
                {
                    hasCollider = true;
                    break;
                }
            }
            if (!hasCollider)
            {
                Collider collider = gameObject.AddComponent<SphereCollider>();
                collider.isTrigger = false;

                Debug.LogWarning(gameObject.name + " is an AI Agent, but it did not have a collider that is not a trigger. One has been added automatically so that the agent will not be contained by the '" + DEFAULT_BARRIERS_NAME + "'. Consider adding one.");
            }

            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                rigidbody = gameObject.AddComponent<Rigidbody>();
                rigidbody.useGravity = false;
                rigidbody.isKinematic = false;

                Debug.LogWarning(gameObject.name + " is an AI Agent, but it did not have rigidbody. One has been added automatically so that the agent will not be contained by the '" + DEFAULT_BARRIERS_NAME + "'. Consider adding one.");
            }

            Random.InitState((int)System.DateTime.Now.Ticks);
        }

        override internal void Start()
        {
            base.Start();
            ConfigureBarriers();
        }

        /// <summary>
        /// Returns an array of objects of a given type that
        /// are within a given range of the agent.
        /// </summary>
        /// <typeparam name="T">The type of object being tested for.</typeparam>
        /// <param name="range">The range we are checking for.</param>
        /// <returns>An array of objects of the given type that are within range.</returns>
        internal List<T> WithinRange<T>(float range)
        {
            List<T> objects = new List<T>();
            Collider[] colliders = Physics.OverlapSphere(transform.position, range);
            foreach (Collider collider in colliders)
            {
                T obj = collider.gameObject.GetComponent<T>();
                if ( obj != null)
                {
                    objects.Add(obj);
                }
            }
            return objects;
        }

        /// <summary>
        /// Barriers are a group of colliders that are used to keep agents within a defined area.
        /// </summary>
        private void ConfigureBarriers()
        {
            if (barriers == null)
            {
                GameObject barriers = GameObject.Find(DEFAULT_BARRIERS_NAME);
                if (barriers == null)
                {
                    Debug.LogError("No `"+ DEFAULT_BARRIERS_NAME + "` to contain the AI Agents found. Create an empty object with children that enclose the area your AI should move within (the children need non-kinematic rigid bodies and colliders). If you If you call it `" + DEFAULT_BARRIERS_NAME + "` then the agent will automatically pick it up, if you need to use a different name drag it into the `Barriers` field of the controller component on your agent.");
                }
            }
        }

        internal override void Update()
        {
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            if (!isFlyByWire)
            {
                base.Update();
            }
            else
            {
                MakeNextMove();
            }

            if (PointOfInterest == null)
            {
                UpdatePointOfInterest();
            }
        }

        internal void UpdatePointOfInterest()
        {
            if (PointOfInterest == null && nextThings.Count > 0)
            {
                PointOfInterest = nextThings[0];
                nextThings.RemoveAt(0);
                return;
            }

            // Look for new points of interest
            if (PointOfInterest == null && nextThings.Count == 0 && Random.value <= 0.001)
            {
                Thing poi = FindPointOfInterest();
                if (poi != null)
                {
                    PointOfInterest = poi;
                }
            }
        }

        /// <summary>
        /// Sets up the state of the agent such that the next movement can be made by changing position and rotation of the agent
        /// </summary>
        internal void MakeNextMove()
        {
            if (PointOfInterest != null)
            {
                targetRotation = Quaternion.LookRotation(PointOfInterest.AgentViewingTransform.position - transform.position, Vector3.up);
            }
            else
            {
                // add some randomness to the flight 
                timeToNextWanderPathChange -= Time.deltaTime;
                if (timeToNextWanderPathChange <= 0)
                {
                    float rotation = Random.Range(minAngleOfRandomPathChange, maxAngleOfRandomPathChange);
                    Vector3 newRotation = targetRotation.eulerAngles;
                    newRotation.y += rotation;
                    targetRotation = Quaternion.Euler(newRotation);
                    timeToNextWanderPathChange = Random.Range(minTimeBetweenRandomPathChanges, maxTimeBetweenRandomPathChanges);
                }
            }

            Vector3 position = transform.position;
            if (PointOfInterest != null && Vector3.Distance(position, PointOfInterest.AgentViewingTransform.position) > PointOfInterest.distanceToTriggerViewingCamera)
            {
                position += transform.forward * normalMovementSpeed * Time.deltaTime;
            }
            else if (PointOfInterest != null)
            {
                ViewPOI();
            }
            else
            {
                // calculate the new position and height
                position += transform.forward * normalMovementSpeed * Time.deltaTime;
                float desiredHeight = Terrain.activeTerrain.SampleHeight(position) + heightOffset;
                position.y += (desiredHeight - position.y) * Time.deltaTime;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.position = position;
        }

        /// <summary>
        /// View a Thing of Interest that is within range. If it is interactable then consider interacting with it.
        /// </summary>
        internal void ViewPOI()
        {
            if (timeLeftLookingAtObject == float.NegativeInfinity)
            {
                timeLeftLookingAtObject = PointOfInterest.timeToLookAtObject;
            }

            CinemachineVirtualCamera virtualCamera = PointOfInterest.virtualCamera;
            virtualCamera.enabled = true;
            
            timeLeftLookingAtObject -= Time.deltaTime;
            if ( _interactWithPOI )
            {
                PointOfInterest.gameObject.GetComponentInChildren<Interactable>().Interact();
                _interactWithPOI = false;
            }

            if (timeLeftLookingAtObject <= 0)
            {
                // Remember we have been here so we don't come again
                visitedThings.Add(PointOfInterest);

                // when we start moving again move away from the object as we are pretty close by now and might move into it
                targetRotation = Quaternion.LookRotation(-transform.forward, Vector3.up);

                // we no longer care about this thing so turn the camera off and don't focus on it anymore
                PointOfInterest = null;
                timeLeftLookingAtObject = float.NegativeInfinity;
                virtualCamera.enabled = false;
            }
        }

        Thing FindPointOfInterest()
        {
            Collider[] things = Physics.OverlapSphere(transform.position, detectionRange);
            if (things.Length > 0)
            {
                for (int i = 0; i < things.Length; i++)
                {
                    Thing thing = things[i].gameObject.GetComponent<Thing>();
                    if (thing != null && thing.isPOI)
                    {
                        if (!visitedThings.Contains(thing))
                        {
                            return thing;
                        }
                    }
                }
            }
            return null;
        }

        void OnCollisionEnter(Collision collision)
        {
            targetRotation = Quaternion.LookRotation(home.transform.position - transform.position, Vector3.up);
        }
    }
}
