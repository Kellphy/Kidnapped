using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Puzzle_Manager : MonoBehaviour
{
    [HideInInspector]
    public Puzzle currentPuzzle;
    public enum Spawn
    {
        Left = 0,
        Right = 1,
        Middle = 2
    }
    public enum RoomObjects
    {
        None,
        Paintings,
        RandomObjects
    }
    [System.Serializable]
    public struct Puzzle
    {
        public AudioClip introClip;
        public Spawn spawnLocation;
        public GameObject puzzleGO;
        public RoomObjects extraObjects;
    };

    public List<Puzzle> puzzles;

    public List<int> randomNumbers;

    Vector3[] initialPos = new Vector3[]
    {
        new Vector3(-4.5f,0,3),
        new Vector3(4.5f,0,3),
        new Vector3(0,0,7),
    };
    Vector3[] finalPos = new Vector3[]
    {
        new Vector3(0,0,3),
        new Vector3(0,0,3),
        new Vector3(0,0,3),
    };

    public int puzzleIterator;

    public static Puzzle_Manager instance;
    void Awake()
    {
        instance = this;

        randomNumbers = GenerateRandom(puzzles.Count);

        foreach(int nr in randomNumbers)
        {
            Debug.Log(nr);
        }

        foreach (Puzzle puzzle in puzzles)
        {
            puzzle.puzzleGO.transform.position = initialPos[(int)puzzle.spawnLocation];
            puzzle.puzzleGO.SetActive(false);
        }
    }

    public void Reset()
    {
        randomNumbers = GenerateRandom(puzzles.Count);
        puzzleIterator = 0;
    }
    public async Task Puzzle_Start()
    {
        if (puzzleIterator >= puzzles.Count)
        {
            await GameEnd();
        }
        else
        {
            print($"starting puzzle with id {randomNumbers[puzzleIterator]}");
            await AudioSelector.instance.Play(puzzles[randomNumbers[puzzleIterator]].introClip);
            currentPuzzle = puzzles[randomNumbers[puzzleIterator]];
            currentPuzzle.puzzleGO = Instantiate(puzzles[randomNumbers[puzzleIterator]].puzzleGO);
            StartCoroutine(MovePuzzle(currentPuzzle.puzzleGO, true, null));
        }
    }

    private async Task GameEnd()
    {
        Debug.Log("Game Ended");
        await AudioSelector.instance.PlayEnding();
    }

    public async void Puzzle_Next(bool succeeded)
    {
        print($"ending puzzle with id {randomNumbers[puzzleIterator]}");

        Lights.instance.LightsFeedback(succeeded);
        if (succeeded)
        {
            await AudioSelector.instance.PlaySuccess();
        }
        else
        {
            await AudioSelector.instance.PlayFail();
        }

        StartCoroutine(MovePuzzle(currentPuzzle.puzzleGO, false, succeeded));
    }

    IEnumerator MovePuzzle(GameObject obj, bool startEvent,bool? succeeded, float time = 5)
    {
        Trigger_Room_Animation(currentPuzzle.spawnLocation);

        Vector3 destination;
        Vector3 startingPos = obj.transform.position;
        float elapsedTime = 0;

        //Before animation
        if (startEvent)
        {
            currentPuzzle.puzzleGO.SetActive(true);
            destination = finalPos[(int)currentPuzzle.spawnLocation];
            currentPuzzle.puzzleGO.GetComponent<IGenericPuzzle>().PuzzleAwake();
        }
        else
        {
            destination = initialPos[(int)currentPuzzle.spawnLocation];
        }

        while (elapsedTime < time)
        {
            obj.transform.position = Vector3.Lerp(startingPos, destination, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Trigger_Room_Animation(currentPuzzle.spawnLocation);

        //After animation
        if (startEvent)
        {
            currentPuzzle.puzzleGO.GetComponent<IGenericPuzzle>().PuzzleStart();
            //Nothing
        }
        else
        {
            // store current position ahead of time this way the puzzle iterator will
            // point to the right thing in puzzle start if routines are running concurrently 
            puzzleIterator++;

            Debug.Log("puzzle iterator " + puzzleIterator);

            yield return new WaitForSeconds(1);
            Destroy(currentPuzzle.puzzleGO);
            //currentPuzzle.puzzleGO.SetActive(false);
        }

        NextPuzzle(succeeded);
    }

    async void NextPuzzle(bool? succeeded)
    {   
        // there is a chance that the corutine in puzzle start may run before puzzle_end has finished executing 
        // should there be a lock that prevents this??
        if (succeeded == true || succeeded == false)
        {
            await Puzzle_Start();
        }
    }

    void Trigger_Room_Animation(Spawn spawnLocation)
    {
        switch (spawnLocation)
        {
            case Spawn.Left:
                Room_Animations.instance.Trigger_Left_Wall();
                break;
            case Spawn.Right:
                Room_Animations.instance.Trigger_Right_Wall();
                break;
            case Spawn.Middle:
                Room_Animations.instance.Trigger_Doors();
                break;
        }
    }

    int GetNum(ArrayList v)
    {
        int n = v.Count;

        System.Random rand = new System.Random();
        int index = (rand.Next() % n);

        int num = (int)v[index];

        v[index] = (int)v[n - 1];
        v.Remove(v[n - 1]);

        return num;
    }
    List<int> GenerateRandom(int n)
    {
        ArrayList v = new ArrayList(n);

        List<int> toReturn = new List<int>(n);

        for (int i = 0; i < n; i++)
            v.Add(i);

        while (v.Count > 0)
        {
            toReturn.Add(GetNum(v));
        }

        return toReturn;
    }
}
