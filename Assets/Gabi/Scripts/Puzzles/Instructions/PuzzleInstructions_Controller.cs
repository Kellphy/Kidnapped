using UnityEngine;

[RequireComponent(typeof(InteractableObject))]
public class PuzzleInstructions_Controller : MonoBehaviour
{

    public PuzzleInstructions puzzleInstructions;
    public bool fail;


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
        if (fail)
        {
            puzzleInstructions.FailedInstruction();
        }
        else
        {
            puzzleInstructions.NextInstruction();
        }
    }
}
