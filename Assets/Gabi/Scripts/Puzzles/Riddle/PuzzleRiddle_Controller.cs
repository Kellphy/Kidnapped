using TMPro;
using UnityEngine;


[RequireComponent(typeof(InteractableObject))]
public class PuzzleRiddle_Controller : MonoBehaviour
{
    public PuzzleRiddle puzzleRiddle;
    public int answerId;

    private InteractableObject interactableObject;
    private TextMeshProUGUI textMesh;

    // this really should be configurable somehow 
    private Color hoverColor = new Color32(0xFF, 0xFF, 0xFF, 0xFF); 
    private Color defaultColor;


    void Start()
    {
        // Get relevant refs
        interactableObject = transform.GetComponent<InteractableObject>();
        textMesh = transform.parent.GetComponent<TextMeshProUGUI>();

        // not really used for the riddle puzzle bc. it does not have a rendarable mesh :(
        // hovering and color transitions must be handled manually 
        interactableObject.hoverOutlineColor = new Color32(0xDB, 0x8B, 0x2A, 0x2A);
        interactableObject.hoverFillColor = new Color32(0xE0, 0xC2, 0x23, 0x23);

        // register user events 
        interactableObject.registerHoverClickHandler(OnHoverClickEvent);
        interactableObject.registerHoverStartHandler(OnHoverStartEvent);
        interactableObject.registerHoverEndHandler(OnHoverEndEvent);

        // store default color 
        defaultColor = textMesh.color;
    }

    public void OnHoverClickEvent()
    {
        puzzleRiddle.Answer(answerId);

        print($"clicked answer {answerId}");
    }


    public void OnHoverStartEvent()
    {
        textMesh.color = hoverColor;
    }

    public void OnHoverEndEvent()
    {
        textMesh.color = defaultColor;
    }

}
