using System;
using System.Linq;
using UnityEngine;
using static Object_Spawner;

public class PuzzleObjects : GenericPuzzle
{
    ObjectivePart chosenObject;

    public override void PuzzleAwake()
    {
        int random = UnityEngine.Random.Range(0, Object_Spawner.instance.InstantiatedSideObjectives.Count);
        chosenObject = Object_Spawner.instance.InstantiatedSideObjectives.ElementAt(random);
        GameObject.Instantiate(chosenObject.prefab, transform.position + new Vector3(0, 1, 0), Quaternion.Euler(0, 180, 0), transform);
    }
    public void ReceiveObjectName(string name)
    {
        if (String.Equals(name, chosenObject.Name))
        {
            PuzzleEnd(true);
        }
    }
}
