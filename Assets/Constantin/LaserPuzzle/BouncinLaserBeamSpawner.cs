using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncinLaserBeamSpawner : MonoBehaviour
{
    [Tooltip("Which prefab to use for laser beam")]
    public LaserBeam laserPrab;

    [Tooltip("Maximum number of laser bounces")]
    public int maxLaserBounces = 20;

    [Tooltip("Maxmum beam length")]
    public float maxRayCastDist = 10.0f;

    [Tooltip("Width of laser beam")]
    public float laserBeamWidth = 0.1f;

    [Tooltip("Start of laser beam")]
    public Transform spawnLoc;

    private List<LaserBeam> laserBeams = new List<LaserBeam>();

    private List<CubeMirrorController> hitObjects = new List<CubeMirrorController>();
    private bool powerSinkHit = false;

    // Start is called before the first frame update
    void Start()
    {
        // forward instantiate as many laser prefabs as needed
        for (int i = 0; i < maxLaserBounces; i++)
        {
            laserBeams.Add(Instantiate(laserPrab, transform));
            laserBeams[i].gameObject.SetActive(false);
        }
    }

    void CastRay(Vector3 initialPos, Vector3 initialDir)
    {
        hitObjects.Clear();
        powerSinkHit = false;

        // Start casting rays 
        Vector3 pos = initialPos;
        Vector3 dir = initialDir;

        bool done = false;
        int bounceNum = 0;

        while (!done)
        {
            // create a ray and raycast
            Ray ray = new Ray(pos, dir);
            RaycastHit hitPos;

            if (Physics.Raycast(ray, out hitPos, maxRayCastDist, 1))
            {
                laserBeams[bounceNum].gameObject.SetActive(true);
                laserBeams[bounceNum].UpdateBeam(pos, hitPos.point, getFlickeredBeamWidth());

                // spawn new ray from hit direction
                CubeMirrorController ctrl = hitPos.collider.GetComponentInParent<CubeMirrorController>();
                if (ctrl != null)
                {
                    hitObjects.Add(ctrl);

                    // spawn new beam
                    pos = hitPos.point;
                    dir = Vector3.Reflect(dir, hitPos.normal);
                    bounceNum++;
                }
                else
                {
                    // check if power sink
                    if (hitPos.collider.GetComponent<PowerSinkController>() != null)
                        powerSinkHit = true;
                    done = true;
                }
            }
            else
            {
                // failed to hit (we are done)
                laserBeams[bounceNum].gameObject.SetActive(true);
                laserBeams[bounceNum].UpdateBeam(pos, ray.GetPoint(maxRayCastDist), getFlickeredBeamWidth());
                done = true;
            }

            if (bounceNum >= maxLaserBounces)
                done = true;
        }
    }

    void ClearPrevBounces()
    {
        for (int i = 0; i < maxLaserBounces; i++)
        {
            laserBeams[i].gameObject.SetActive(false);
        }
    }

    float getFlickeredBeamWidth()
    {
        float intensity = 0.1f;
        float variation = Mathf.Sin(20f * Time.time) + 0.5f* Mathf.Sin(30f * Time.time); 
        float width = laserBeamWidth * ( 1 + variation * intensity) ;
        return width;
    }

    // update is triggered externally
    public void UpdateLaserBeams(bool show)
    {
        ClearPrevBounces();
        if (show)
            CastRay(spawnLoc.transform.position, spawnLoc.transform.forward);
    }


    public List<CubeMirrorController> GetHitCubeMirrors()
    {
        return hitObjects;
    }

    public bool GetPowerSinkHit()
    {
        return powerSinkHit;
    }


}
