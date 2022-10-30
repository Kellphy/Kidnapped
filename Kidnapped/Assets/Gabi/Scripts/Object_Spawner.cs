using System.Collections.Generic;
using UnityEngine;

public class Object_Spawner : MonoBehaviour
{
    [System.Serializable]
    public struct ObjectivePart
    {
        public string Name;
        public GameObject prefab;
    }
    public ObjectivePart[] Objects;

    [System.Serializable]
    public struct SpawnPoint
    {
        public Vector3 Position;
        public Vector3 Rotation;
    }
    public SpawnPoint[] SpawnPoints;

    bool[] objectActive;
    [HideInInspector]
    public List<ObjectivePart> InstantiatedSideObjectives = new List<ObjectivePart>();

    public static Object_Spawner instance;

    void Awake()
    {
        instance = this;
        objectActive = new bool[Objects.Length];
    }
    void Start()
    {
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            bool spawning = false;
            while (!spawning)
            {
                int rnd = Random.Range(0, Objects.Length);
                if (objectActive[rnd] == false)
                {
                    GameObject tempOO = Instantiate(Objects[rnd].prefab, SpawnPoints[i].Position, Quaternion.Euler(SpawnPoints[i].Rotation), transform);
                    InstantiatedSideObjectives.Add(Objects[rnd]);
                    objectActive[rnd] = true;
                    spawning = true;
                }
            }
        }
    }
}