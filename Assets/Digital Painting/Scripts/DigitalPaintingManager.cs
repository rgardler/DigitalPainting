﻿using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.agent;
using wizardscode.digitalpainting.agent;
using Random = UnityEngine.Random;

namespace wizardscode.digitalpainting
{
    public class DigitalPaintingManager : MonoBehaviour
    {
        [Tooltip("The agents that exist in the world. These agents will act autonomously in the world, doing interesting things. The first agent in the list will be the first one in the list is the one that the camera will initially be viewing.")]
        public AgentScriptableObject[] agentObjectDefs;

        [SerializeField][Tooltip("A reference to the agent that currently has focus.")]
        private BaseAgentControllerReference _agentWithFocus = default(BaseAgentControllerReference);

        private BaseAgentControllerGameEvent _onChangeFocusAgentEvent = default(BaseAgentControllerGameEvent);

        void Awake()
        {
            SetupBarriers();
            for (int i = 0; i < agentObjectDefs.Length; i++)
            {
                BaseAgentController agent = CreateAgent("Agent: " + i + " " + agentObjectDefs[i].prefab.name, agentObjectDefs[i]);
                if (i == 0)
                {
                    _agentWithFocus.Value = agent;
                }
            }
        }

        /// <summary>
        /// Create an agent.
        /// </summary>
        /// <returns></returns>
        private BaseAgentController CreateAgent(string name, AgentScriptableObject def)
        {
            GameObject agent = GameObject.Instantiate(def.prefab).gameObject;
            agent.name = name;
            BaseAgentController controller = agent.GetComponent<BaseAgentController>();

            Renderer renderer = agent.GetComponent<Renderer>();
            if (renderer != null) {
                renderer.enabled = def.render;
            }

            float border = Terrain.activeTerrain.terrainData.size.x / 10;
            float x = Random.Range(border, Terrain.activeTerrain.terrainData.size.x - border);
            float z = Random.Range(border, Terrain.activeTerrain.terrainData.size.z - border);
            Vector3 position = new Vector3(x, 0, z);

            float y = Terrain.activeTerrain.SampleHeight(position);
            position.y = y + controller.MovementController.heightOffset;            
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
            float y = size.y;
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
