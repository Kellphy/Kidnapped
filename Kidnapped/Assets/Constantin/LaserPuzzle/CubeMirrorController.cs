using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMirrorController : MonoBehaviour
{
    [Header("User interation triggers")]
    public InteractableObject rotateLeftButton;
    public InteractableObject rotateRightButton;

    [Header("Cube mirror properties")]
    public GlowingCubeController cubeMirror;
    public float rotationSpeed = 30.0f;

    [Header("Internal")]
    public int cubeId;
    bool isActive;

    private bool inputDisabled = false;

    // Start is called before the first frame update
    void Start()
    {        
        SetHoverProperties(rotateRightButton);
        SetHoverProperties(rotateLeftButton);
        SetActive(false);
    }

    public void SetHoverProperties(InteractableObject interactableObject)
    {
        interactableObject.hoverOutlineColor = new Color32(0xDB, 0x8B, 0x2A, 0x2A);
        interactableObject.hoverFillColor = new Color32(0xE0, 0xC2, 0x23, 0x23);
    }

    public void DisableInput(bool state)
    {
        inputDisabled = state;
    }


    // Update is called once per frame
    void Update()
    {
        if (!inputDisabled)
        {
            float rotationAngle = rotationSpeed * Time.deltaTime;
            if (rotateLeftButton.IsHovered())
            {
                cubeMirror.transform.Rotate(Vector3.up, rotationAngle);
            }
            else if (rotateRightButton.IsHovered())
            {
                cubeMirror.transform.Rotate(Vector3.up, -rotationAngle);
            }
        }
        
    }

    public void SetActive(bool state)
    {
        isActive = state;
        if (isActive)
        {
            cubeMirror.Activate();
        }
        else
        {
            cubeMirror.Deactivate();
        }
    }

    public bool IsActive()
    {
        return isActive;
    }
}
