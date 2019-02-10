using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.environment;

namespace wizardscode.devtest.environment
{
    public class ThingSpawner : MonoBehaviour
    {
        public GameObject thingPrefab;
        public int numberToSpawn = 5;
        public float spawnRadius = 5;

        void Awake()
        {
            for (int i = 0; i < numberToSpawn; i++)
            {
                GameObject obj = Instantiate(thingPrefab);
                obj.name = "Spawned Object " + i;
                obj.transform.parent = transform;
                obj.transform.position = transform.position + (Random.insideUnitSphere * spawnRadius);                
            }
        }
    }
}