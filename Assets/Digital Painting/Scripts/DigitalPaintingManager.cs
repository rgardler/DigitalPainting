﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.digitalpainting.agent;

namespace wizardscode.digitalpainting
{
    public class DigitalPaintingManager : MonoBehaviour
    {
        [Tooltip("The clear shot camera rig prefab to use. If this is null a Clearshot camera will be lookd for in the scene.")]
        public Cinemachine.CinemachineClearShot cameraRigPrefab;
        [Tooltip("The Camera prefab to use if no main camera exists in the scene.")]
        public Camera cameraPrefab;

        [Header("Agent")]
        [Tooltip("The Agent prefab to use as the primary character - that is the one the camera will follow.")]
        public BaseAgentController agentPrefab;
        [Tooltip("The position in which to spawn the agent.")]
        public Vector3 agentSpawnPosition;
        [Tooltip("Render agent - if this is set to off (unchecked) then the agent will not be visible.")]
        public bool renderAgent = true;

        private Cinemachine.CinemachineClearShot _clearshot;

        private BaseAgentController _agent;
        /// <summary>
        /// Get or set the agent that has the current focus of the camera.
        /// </summary>
        public BaseAgentController AgentWithFocus {
            get { return _agent; }
            set
            {
                _agent = value;
                _clearshot.Follow = _agent.transform;
                _clearshot.LookAt = _agent.transform;
            }
        }

        void Awake()
        {
            SetupBarriers();
            CreateCamera();
            AgentWithFocus = CreateAgent();
        }

        /// <summary>
        /// Create the default camera rig. If there is a Main Camera in the scene it will be configured appropriately,
        /// otherwise a camera will be added to the scene.
        /// </summary>
        private void CreateCamera()
        {
            _clearshot = FindObjectOfType<CinemachineClearShot>();
            if (_clearshot == null)
            {
                _clearshot = GameObject.Instantiate(cameraRigPrefab);
            }

            Camera camera = Camera.main;
            if (camera == null)
            {
                camera = Instantiate(cameraPrefab);
                return;
            }
            
            if (camera.GetComponent<CinemachineBrain>() == null)
            {
                Debug.LogWarning("Camera did not have a Cinemachine brain, adding one. You should probably add one to your camera in the scene.");
                camera.gameObject.AddComponent<CinemachineBrain>();
            }

            if (camera.GetComponent<AudioListener>() == null)
            {
                Debug.LogWarning("Camera did not have an audio listener, adding one. You should probably add one to your camera in the scene.");
                camera.gameObject.AddComponent<AudioListener>();
            }

            if (camera.GetComponent<FlareLayer>() == null)
            {
                Debug.LogWarning("Camera did not have an Flare Layer, adding one. You should probably add one to your camera in the scene.");
                camera.gameObject.AddComponent<FlareLayer>();
            }
        }

        /// <summary>
        /// Create the main agent that the cameras will follow initially.
        /// </summary>
        /// <returns></returns>
        private BaseAgentController CreateAgent()
        {
            GameObject agent = GameObject.Instantiate(agentPrefab).gameObject;
            BaseAgentController controller = agent.GetComponent<BaseAgentController>();

            Renderer renderer = agent.GetComponent<Renderer>();
            if (renderer != null) {
                renderer.enabled = renderAgent;
            }

            Vector3 position = agentSpawnPosition;
            float y = Terrain.activeTerrain.SampleHeight(position);
            position.y = y + controller.heightOffset;            
            agent.transform.position = position;

            return controller;
        }

        /// <summary>
        /// Create default barriers in the scene. These will be 10% in from the edge of the terrain borders on each side.
        /// </summary>
        private void SetupBarriers()
        {
            GameObject barriers = GameObject.Find(AIAgentController.DEFAULT_BARRIERS_NAME);
            if (barriers != null)
            {
                return;
            }

            Vector3 size = Terrain.activeTerrain.terrainData.size;
            float x = size.x;
            float z = size.z;

            size.x = 2;

            // Parent
            barriers = new GameObject(AIAgentController.DEFAULT_BARRIERS_NAME);
            
            // Top
            GameObject barrier = GameObject.CreatePrimitive(PrimitiveType.Cube);
            barrier.transform.parent = barriers.transform;
            barrier.name = "Barrier 1";
            barrier.transform.localScale = size;
            barrier.transform.position = new Vector3(x * 0.1f, 0, z / 2);
            barrier.GetComponent<Renderer>().enabled = false;

            // Bottom
            barrier = GameObject.CreatePrimitive(PrimitiveType.Cube);
            barrier.transform.parent = barriers.transform;
            barrier.name = "Barrier 2";
            barrier.transform.localScale = size;
            barrier.transform.position = new Vector3(x * 0.9f, 0, z / 2);
            barrier.GetComponent<Renderer>().enabled = false;

            // Left
            barrier = GameObject.CreatePrimitive(PrimitiveType.Cube);
            barrier.transform.parent = barriers.transform;
            barrier.name = "Barrier 3";
            barrier.transform.localScale = size;
            barrier.transform.rotation = Quaternion.Euler(0, 90, 0);
            barrier.transform.position = new Vector3(x / 2, 0, z * 0.1f);
            barrier.GetComponent<Renderer>().enabled = false;

            // Right
            barrier = GameObject.CreatePrimitive(PrimitiveType.Cube);
            barrier.transform.parent = barriers.transform;
            barrier.name = "Barrier 4";
            barrier.transform.localScale = size;
            barrier.transform.rotation = Quaternion.Euler(0, 270, 0);
            barrier.transform.position = new Vector3(x / 2, 0, z * 0.9f);
            barrier.GetComponent<Renderer>().enabled = false;
        }
    }
}
