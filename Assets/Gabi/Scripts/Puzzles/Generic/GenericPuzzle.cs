using System.Collections;
using UnityEngine;

public class GenericPuzzle : MonoBehaviour, IGenericPuzzle
{
    public GameObject cover;
    public virtual void PuzzleAwake() { }
    public virtual void PuzzleStart()
    {
        StartCoroutine(RemoveCover());
    }

    public void PuzzleEnd(bool succeeded)
    {
        Puzzle_Manager.instance.Puzzle_Next(succeeded);
    }
    IEnumerator RemoveCover()
    {
        if (cover != null)
        {
            while (cover.transform.position.y > -1)
            {
                cover.transform.position += new Vector3(0, -1f * Time.deltaTime, 0);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
