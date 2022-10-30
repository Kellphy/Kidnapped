using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPuzzleController : GenericPuzzle
{
    [Tooltip("Tile prefab")]
    public LightTileController tilePrefab;

    [Tooltip("Grid location")]
    public Transform tileGridLoc;

    [Tooltip("Puzzle check trigger")]
    public InteractableObject checkTrigger;

    [Tooltip("Puzzle difficulty")]
    public int numActiveTiles = 5;

    // dimensions of grid
    int gridX = 7;
    int gridY = 8;
    int numTiles;
    float pitchX = 0.172f;
    float pitchY = 0.1715f;

    private List<LightTileController> tileGrid = new List<LightTileController>();
    private List<bool> activeTiles = new List<bool>();

    private bool acceptCheckEvents = false;

    // Start is called before the first frame update

    public override void PuzzleAwake()
    {
        // generate a new random game board 
        numTiles = gridX * gridY;
        SpawnTiles();
        SelectRandomBoard(numActiveTiles);
        
        
        // configure board check trigger
        checkTrigger.hoverOutlineColor = new Color32(0xDB, 0x8B, 0x2A, 0x2A);
        checkTrigger.hoverFillColor = new Color32(0xE0, 0xC2, 0x23, 0x23);
        checkTrigger.registerHoverClickHandler(CheckBoard);
    }

    public override void PuzzleStart()
    {
        // display board 
        StartCoroutine(ShowBoard());
    }

    IEnumerator ShowBoard()
    {
        for (int i = 0; i < numTiles; i++)
        {
            // prevent user clicks
            tileGrid[i].DisableUserEvents(true);

            // show relevant tiles
            if (activeTiles[i])
                tileGrid[i].ToggleTile();
        }

        yield return new WaitForSeconds(2.0f);

        for (int i = 0; i < numTiles; i++)
        {
            tileGrid[i].DisableUserEvents(false);
            
            // turn tiles
            if (activeTiles[i])
                tileGrid[i].ToggleTile();
        }

        acceptCheckEvents = true;
    }


    void SpawnTiles()
    {
        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridY; j++)
            {
                Vector3 offset = new Vector3(i * pitchX, -j * pitchY, 0);
                Vector3 pos = tileGridLoc.transform.position + offset;
                LightTileController tileInstance = Instantiate(tilePrefab, pos, Quaternion.Euler(0.0f, 180.0f, 0.0f), tileGridLoc);
                tileGrid.Add(tileInstance);

            }
        }
    }

    void SelectRandomBoard(int numActive)
    {
        activeTiles.Clear();
        List<int> activeIndices = new List<int>();

        // Select num active indices
        for (int i = 0; i < numTiles; i++)
        {
            activeIndices.Add(i);
            activeTiles.Add(false);
        }

        Shuffle(activeIndices);

        for (int i = 0; i < numActive; i++)
        {
            activeTiles[activeIndices[i]] = true;
        }
    }

    void Shuffle(List<int> elems)
    {
        for (int i = 0; i < elems.Count; i++)
        {
            int temp = elems[i];
            int randomIndex = Random.Range(i, elems.Count);
            elems[i] = elems[randomIndex];
            elems[randomIndex] = temp;
        }
    }


    List<bool> GetBoardState()
    {
        List<bool> ret = new List<bool>();

        for (int i = 0; i < numTiles; i++)
        {
            tileGrid[i].DisableUserEvents(true);
            ret.Add(tileGrid[i].GetState());
        }

        return ret;
    }
    void CheckBoard()
    {
        List<bool> boardState = GetBoardState();
        acceptCheckEvents = false;
        StartCoroutine(ShowErrors(boardState));
    }

    IEnumerator ShowErrors(List<bool> boardState)
    {
        // turn everything back 
        for (int i = 0; i < numTiles; i++)
        {
            if (boardState[i] == true)
            {
                tileGrid[i].ToggleTile();
            }
        }

        yield return new WaitForSeconds(1.5f);

        // show errors 
        int numErrors = 0;
        for (int i = 0; i < numTiles; i++)
        {
            if (boardState[i] != activeTiles[i])
            {
                tileGrid[i].SetType(LightTileController.LightTileType.Red);
                if (activeTiles[i] == true)
                    numErrors++;
            }
            else
            {
                tileGrid[i].SetType(LightTileController.LightTileType.Green);
            }

            if (activeTiles[i] == true)
                tileGrid[i].ToggleTile();
        }

        yield return new WaitForSeconds(3.0f);

        if (numErrors == 0)
        {
            PuzzleEnd(true);
            print("GG BRO!");
        }
        else
        {
            PuzzleEnd(false);
            print("lost");
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
