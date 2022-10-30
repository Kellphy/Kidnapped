using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractableObject))]
public class LightTileController : MonoBehaviour
{

    public enum LightTileType{
        Red, 
        Green,
        Blue
    };

    [Header("Effects")]
    public GameObject greenGlow;
    public GameObject redGlow;
    public GameObject blueGlow;
    
    // default to green
    private LightTileType type = LightTileType.Green;

    // Handle front back switching 
    private bool showFront = false;
    private bool blockEventsDuringAnimation = false;
    private bool disableUserEvents = true;

    // For handling user events
    private InteractableObject interactableObject;
    void Start()
    {
        interactableObject = transform.GetComponent<InteractableObject>();
        interactableObject.hoverOutlineColor = new Color32(0xDB, 0x8B, 0x2A, 0x2A);
        interactableObject.hoverFillColor = new Color32(0xE0, 0xC2, 0x23, 0x23);

        // register user events 
        interactableObject.registerHoverClickHandler(OnHoverClickEvent);

        // set default type 
        SetType(LightTileType.Blue);
    }

    public void SetType(LightTileType type)
    {
        switch(type)
        {
            case LightTileType.Red:
                redGlow.SetActive(true);
                greenGlow.SetActive(false);
                blueGlow.SetActive(false);
                break;

            case LightTileType.Green:
                redGlow.SetActive(false);
                greenGlow.SetActive(true);
                blueGlow.SetActive(false);
                break;
            case LightTileType.Blue:
                redGlow.SetActive(false);
                greenGlow.SetActive(false);
                blueGlow.SetActive(true);
                break;
        }

        this.type = type;
    }

    public bool GetState()
    {
        return showFront;
    }

    public void DisableUserEvents(bool state)
    {
        disableUserEvents = state;
    }


    public void OnHoverClickEvent()
    {
        if (!disableUserEvents)
            ToggleTile();
    }


    public void ToggleTile()
    {
        showFront = !showFront;
        if (!blockEventsDuringAnimation)
        {
            if (showFront)
                StartCoroutine(SmoothRotation(0.0f));
            else
                StartCoroutine(SmoothRotation(180.0f));
        }
    }



    IEnumerator SmoothRotation(float targetRotationY)
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        float duration = 1.0f;
        float startTime = Time.time;

        blockEventsDuringAnimation = true;

        print($"cr {currentRotation.y} tr {targetRotationY}");

        while (true)
        {
            float dt = Time.time - startTime;

            float deltaRotationY = targetRotationY - currentRotation.y;
            float desiredRotY = currentRotation.y + deltaRotationY * dt / duration;
            transform.rotation = Quaternion.Euler(new Vector3(currentRotation.x, desiredRotY, currentRotation.z));

            if ( dt >= duration)
            {
                break;
            }

            yield return null;
        }

        // Set proper rotation
        transform.rotation = Quaternion.Euler(new Vector3(currentRotation.x, targetRotationY, currentRotation.z));

        blockEventsDuringAnimation = false;
    }


    // Update is called once per frame
    void Update()
    {

    }

}
