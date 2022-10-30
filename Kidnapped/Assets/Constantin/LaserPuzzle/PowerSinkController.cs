using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSinkController : MonoBehaviour
{

    [Header("Effects")]
    public Light pointLight;
    public GameObject sideGlow;

    private bool isActive = false;

    public void Activate()
    {
        // activate visuals
        isActive = true;
        pointLight.enabled = true;
        sideGlow.SetActive(true);
    }

    public void Deactivate()
    {
        // deactivate visuals 
        isActive = false;
        pointLight.enabled = false;
        sideGlow.SetActive(false);
    }

    public bool IsActive()
    {
        return isActive;
    }
}
