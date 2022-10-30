using UnityEngine;
using static Puzzle_Manager;

[RequireComponent(typeof(InteractableObject))]
public class ObjectsController : MonoBehaviour
{
    public string objectName;


    private InteractableObject interactableObject;
    void Start()
    {
        interactableObject = transform.GetComponent<InteractableObject>();
        // configure hover colors (we need a better way to do this :))
        interactableObject.hoverOutlineColor = new Color32(0xDB, 0x8B, 0x2A, 0x2A);
        interactableObject.hoverFillColor = new Color32(0xE0, 0xC2, 0x23, 0x23);

        // register user events (note you can register multiple functions on same event)
        interactableObject.registerHoverClickHandler(OnHoverClickEvent);
    }

    public void OnHoverClickEvent()
    {
        if (Puzzle_Manager.instance.currentPuzzle.extraObjects == RoomObjects.RandomObjects)
        {
            Puzzle_Manager.instance.currentPuzzle.puzzleGO.GetComponent<PuzzleObjects>().ReceiveObjectName(objectName);
        }

        print($"clicked object {objectName}");
    }

}
