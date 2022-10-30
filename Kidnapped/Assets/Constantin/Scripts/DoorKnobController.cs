using UnityEngine;

public class DoorKnobController : MonoBehaviour
{
    public enum Action
    {
        Door,
        Wall_Right,
        Wall_Left
    }
    public Action selectedAction;

    private InteractableObject interactableObject;

    public void Start()
    {
        interactableObject = transform.GetComponent<InteractableObject>();
        // register user events 
        interactableObject.registerHoverClickHandler(OnHoverClickEvent);
    }

    public void OnHoverClickEvent()
    {
        switch (selectedAction)
        {
            case Action.Door:
                Room_Animations.instance.Trigger_Doors();
                break;
            case Action.Wall_Left:
                Room_Animations.instance.Trigger_Left_Wall();
                break;
            case Action.Wall_Right:
                Room_Animations.instance.Trigger_Right_Wall();
                break;
        }
    }

}
