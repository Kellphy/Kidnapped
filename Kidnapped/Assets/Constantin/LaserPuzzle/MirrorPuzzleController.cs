using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class MirrorPuzzleController : MonoBehaviour
public class MirrorPuzzleController : GenericPuzzle
{
    public BouncinLaserBeamSpawner laserSpawner;
    public List<CubeMirrorController> cubeMirrors = new List<CubeMirrorController>();
    public PowerSinkController powerSink;
    public Animator animator;

    private bool puzzleFinished = false;
    private bool puzzleStarted = false;

    public override void PuzzleStart()
    {
        puzzleStarted = true;
        animator.SetBool("open", true);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (puzzleStarted)
        {
            DeactivateAll();
            laserSpawner.UpdateLaserBeams(true);
            ActivateHit(laserSpawner.GetHitCubeMirrors(), laserSpawner.GetPowerSinkHit());
        }
        else
        {
            DeactivateAll();
            laserSpawner.UpdateLaserBeams(false);
        }
        
    }


    void DeactivateAll()
    {
        foreach (var cubeMirror in cubeMirrors)
        {
            cubeMirror.SetActive(false);
        }

        powerSink.Deactivate();
    }

    void ActivateHit(List<CubeMirrorController> hitCubeMirrors, bool powerSinkHit)
    {
        foreach (var hitCubeMirror in hitCubeMirrors)
        {
            hitCubeMirror.SetActive(true);
        }

        if (hitCubeMirrors.Count == cubeMirrors.Count && powerSinkHit)
        {
            powerSink.Activate();

            if (!puzzleFinished)
            {
                puzzleFinished = true;

                foreach (var cubeMirror in cubeMirrors)
                {
                    cubeMirror.DisableInput(true);
                }

                StartCoroutine(GameFinished());
                print("Congrats you won the game!!");
            }
           
        }
    }

    IEnumerator GameFinished()
    {
        yield return new WaitForSeconds(1.5f);
        puzzleStarted = false;
        animator.SetBool("close", true);
        PuzzleEnd(true);
    }

}
