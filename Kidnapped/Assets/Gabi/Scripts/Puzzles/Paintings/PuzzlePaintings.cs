using System;
using UnityEngine;

public class PuzzlePaintings : GenericPuzzle
{
    [Serializable]
    public struct Paintings
    {
        public GameObject painting;
        public int id;
    }
    public Paintings[] paintings;
    public Transform spawnLoc;

    Paintings chosenPainting;

    public override void PuzzleAwake()
    {
        int random = UnityEngine.Random.Range(0, paintings.Length);
        chosenPainting = paintings[random];
        //GameObject.Instantiate(chosenPainting.painting, transform.position + new Vector3(0, 1, 0), Quaternion.Euler(0, 180, 0), transform);
        GameObject.Instantiate(chosenPainting.painting, spawnLoc.position, spawnLoc.rotation, spawnLoc);
        print("Chosen painting " + random);
    }
    public void ReceivePaintingId(int id)
    {
        if (id == chosenPainting.id)
        {
            PuzzleEnd(true);
        }
    }
}
