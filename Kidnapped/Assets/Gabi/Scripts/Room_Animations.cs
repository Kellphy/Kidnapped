using UnityEngine;

public class Room_Animations : MonoBehaviour
{
    public Animator doorsAnim;
    public Animator WallAnim_Right;
    public Animator WallAnim_Left;

    bool doorState;
    bool wallState_Right;
    bool wallState_Left;

    public static Room_Animations instance;

    private void Awake()
    {
        instance = this;
    }

    private void Animation_Action(Animator anim, bool action)
    {
        if (action)
        {
            anim.SetTrigger("open");
        }
        else
        {
            anim.SetTrigger("close");
        }
    }

    public void Trigger_Doors()
    {
        doorState = !doorState;
        Animation_Action(doorsAnim, doorState);
    }
    public void Trigger_Left_Wall()
    {
        wallState_Left = !wallState_Left;
        Animation_Action(WallAnim_Left, wallState_Left);
    }

    public void Trigger_Right_Wall()
    {
        wallState_Right = !wallState_Right;
        Animation_Action(WallAnim_Right, wallState_Right);
    }
}
