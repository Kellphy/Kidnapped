using UnityEngine;
using static Puzzle_Manager;

// Add this to make sure gameobjects will always have an interactable object attached 
[RequireComponent(typeof(InteractableObject))]
public class PaintingsController : MonoBehaviour
{
    public int paintingId;

    private InteractableObject interactableObject;
    void Start()
    {
        interactableObject = transform.GetComponent<InteractableObject>();
        // configure hover colors (we need a better way to do this :))
        interactableObject.hoverOutlineColor = new Color32(0xDB, 0x8B, 0x2A, 0x2A);
        interactableObject.hoverFillColor = new Color32(0xE0, 0xC2, 0x23, 0x23);

        // register user events 
        interactableObject.registerHoverClickHandler(OnHoverClickEvent);
    }


    public void OnHoverClickEvent()
    {
        if (Puzzle_Manager.instance.currentPuzzle.extraObjects == RoomObjects.Paintings)
        {
            Puzzle_Manager.instance.currentPuzzle.puzzleGO.GetComponent<PuzzlePaintings>().ReceivePaintingId(paintingId);
        }

        print($"clicked painting {paintingId}");
    }


    // Update is called once per frame
    void Update()
    {
        // no more click logic in user scripts :) 
    }

}
