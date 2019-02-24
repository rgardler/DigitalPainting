using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.ability;
using wizardscode.environment;
using wizardscode.interaction;
using wizardscode.inventory;

namespace wizardscode.digitalpainting.agent
{
    /// <summary>
    /// BaseAgentController provides the core parameters and a very basic manual controller for agents.
    /// Agents are defined as GameObjects that are capable of knowingly engaging with the environment,
    /// that is, they don't simply respond to physics. For example, they can decide their own movement
    /// path/speed and/or have abilities they can use to interact with the environment.
    /// 
    /// WASD provide forward/backward and strafe left/right
    /// QE provide up and down
    /// Right mouse button _ mouse provides look
    /// </summary>
    public class BaseAgentController : MonoBehaviour
    {
        [Header("Interaction")]
        [Tooltip("Primary interaction point that the agent will use when interacting with a THing in the world.")]
        public Transform interactionPoint;

        [Header("Equipment")]
        [Tooltip("The mount point for an equipped item.")]
        public Transform equippedMountPoint;

        [Header("Lifestyle")]
        [Tooltip("Home location of the agent. If blank a home object will be created from the `homePrefab`, see below, at the agents starting position.")]
        public Thing home;
        [Tooltip("The prefab to use if we need to spawn the agents home at their starting position. If `home` is null this must be set, however, if `home` is set this can be left blank.")]
        public Thing homePrefab;

        [Header("Movement")]
        [Tooltip("Indicates whether the agent must remain on the ground.")]
        public bool isGrounded = true;
        [Tooltip("Walking speed under normal circumstances")]
        public float normalMovementSpeed = 1;
        [Tooltip("The factor by which to multiply the walking speed when moving fast.")]
        public float fastMovementFactor = 4;
        [Tooltip("The factor by which to multiply the walking speed when moving slowly.")]
        public float slowMovementFactor = 0.2f;
        [Tooltip("Speed at which the agent will climb/drop in flight. Set to 0 if you don't want them to fly.")]
        public float climbSpeed = 1;
        [Tooltip("The height above the terrain this agent should be.")]
        public float heightOffset = 0;
        [Tooltip("Speed at which the agent will rotate.")]
        public float rotationSpeed = 90;

        public enum MouseLookModeType { Never, Always, WithRightMouseButton }
        [Header("Manual Controls")]
        [Tooltip("What is needed for mouse look to be operational.")]
        public MouseLookModeType mouseLookMode = MouseLookModeType.WithRightMouseButton;
        [Tooltip("Mouse look sensitivity.")]
        public float mouseLookSensitivity = 100;

        float rotationX = 0;
        float rotationY = 0;

        internal AbilityCollection abilities;

        internal Thing _pointOfInterest;
        internal bool _interactWithPOI;
        private Interactable _interactable;
        private Interactable _equipped; // the currently equipped item

        private DigitalPaintingManager manager;
        private InventoryManager inventory;

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
                    Interactable = PointOfInterest.gameObject.GetComponentInParent<Interactable>();
                    _interactWithPOI = Interactable != null;
                }
            }
        }

        /// <summary>
        /// Get or set the interactable component that we are currently focusing on.
        /// </summary>
        public Interactable Interactable
        {
            get { return _interactable; }
            set { _interactable = value; }
        }

        /// <summary>
        /// Equip a given item or get the currently equipped item. 
        /// If there is already an equipped item drop it.
        /// </summary>
        /// <param name="item">The item to equip.</param>
        public Interactable Using
        {
            get { return _equipped;  }
            set
            {
                if (GameObject.ReferenceEquals(_equipped, value))
                {
                    return;
                }

                if (_equipped != null)
                {
                    DropItem(_equipped);
                }

                _equipped = value;
                if (_equipped != null)
                {
                    _equipped.transform.SetParent(equippedMountPoint, true);
                    _equipped.transform.localPosition = Vector3.zero;
                    _equipped.gameObject.SetActive(true);
                }
            }
        }

        /// <summary>
        /// Unequip the currently equipped item, optionally putting it into the inventory if there is space, otherwise
        /// dropping it.
        /// <paramref name="storeInInventory">Set to false if the item is to be dropped rather than stored in the
        /// inventory. Note that if you are unequipping because it is being given to someone else or stored elsewhere
        /// this should be false.
        /// </summary>
        public void Unequip(bool storeInInventory = true)
        {
            if (Using == null)
            {
                return;
            }

            if (storeInInventory && inventory != null)
            {
                if (inventory.AddItem(_equipped))
                {
                    _equipped.gameObject.SetActive(false);
                    _equipped = null;
                } else
                {
                    DropItem(Using);
                }
            } else
            {
                DropItem(Using);
            }
        }

        public void DropItem(Interactable item)
        {
            item.enabled = true;
            item.transform.parent = null;
            Vector3 pos = transform.position;
            pos += Vector3.forward;
            item.transform.position = pos;
        }

        /// <summary>
        /// Take an item from the inventory and equip it. If an item is already equipped then 
        /// put that into the inventory.
        /// </summary>
        /// <param name="index">The index of the item in the inventory to equip</param>
        public void EquipItemFromInventory(int index)
        {
            if (inventory == null)
            {
                Debug.LogError("Trying to equip an item from the inventory, but the agent does not have an inventory.");
                return;
            }
            Using = inventory.GetItem(index);
        }

        virtual internal void Start()
        {
            manager = FindObjectOfType<DigitalPaintingManager>();

            if (home == null)
            {
                if (homePrefab == null)
                {
                    Debug.LogError("Neither `home` nor `homePrefab` are set on " + gameObject.name + " at least one must be set.");
                } else
                {
                    Vector3 offset = new Vector3(0, 0, 6);
                    home = Instantiate(homePrefab, transform.position + offset, Quaternion.identity);
                }
            }

            abilities = GetComponent<AbilityCollection>();
            if (abilities == null)
            {
                Debug.LogWarning(gameObject.name + " is an agent but it does not have an abilities collection component. Adding one, with no abilities to prevent null pointer errors. You should add one to the GameObject to remove this warning.");
                abilities = gameObject.AddComponent<AbilityCollection>();
            }

            inventory = GetComponent<InventoryManager>();
        }

        internal virtual void Update()
        {
            // Mouse Look
            switch (mouseLookMode) {
                case MouseLookModeType.Always:
                    MouseLook();
                    break;
                case MouseLookModeType.WithRightMouseButton:
                    if (Input.GetMouseButton(1))
                    {
                        MouseLook();
                    }
                    break;
                default:
                    break;
            }

            Vector3 pos = transform.position;

            // Move with the keyboard controls 
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                pos += transform.forward * (normalMovementSpeed * fastMovementFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
                pos += transform.right * (normalMovementSpeed * fastMovementFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                pos += transform.forward * (normalMovementSpeed * slowMovementFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
                pos += transform.right * (normalMovementSpeed * slowMovementFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
            }
            else
            {
                pos += transform.forward * normalMovementSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
                pos += transform.right * normalMovementSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
            }

            if (isGrounded)
            {
                pos.y = Terrain.activeTerrain.SampleHeight(pos) + heightOffset;
            }

            transform.position = pos;

            if (!isGrounded)
            {
                if (Input.GetKey(KeyCode.Q))
                {
                    heightOffset += climbSpeed * Time.deltaTime;
                }

                if (Input.GetKey(KeyCode.E))
                {
                    heightOffset -= climbSpeed * Time.deltaTime;
                }
            }
        }

        /// <summary>
        /// PrepareToInteract ensures the agent is ready to perform an interaction
        /// on an object that it has approached and is close to.
        /// 
        /// Agent controllers may override this method, by default it moves the agents
        /// interaction point nearer to the Thing with which to interact.
        /// 
        /// </summary>
        internal void PrepareToInteract()
        {
            Debug.LogWarning("TODO: implement prepare interaction");
        }

        private void MouseLook()
        {
            if (!isGrounded)
            {
                rotationY += Input.GetAxis("Mouse Y") * mouseLookSensitivity * Time.deltaTime;
                rotationY = Mathf.Clamp(rotationY, -90, 90);
                transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
            } else
            {
                transform.localRotation *= Quaternion.AngleAxis(0, Vector3.left);
            }

            rotationX += Input.GetAxis("Mouse X") * mouseLookSensitivity * Time.deltaTime;
            transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        }
    }
}
