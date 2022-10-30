using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingCubeController : MonoBehaviour
{
    [Header("Effects")]
    public Light pointLight;
    public GameObject sideGlow;

    public void Activate()
    {
        // activate visuals
        pointLight.enabled = true;
        sideGlow.SetActive(true);
    }

    public void Deactivate()
    {
        // deactivate visuals 
        pointLight.enabled = false;
        sideGlow.SetActive(false);
    }

}
