using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class LaserController : MonoBehaviour
{

    [Tooltip("Laser dot which will be displayed at laser hit location")]
    public GameObject hitObjPrefab;

    private LineRenderer lineRenderer;
    private GameObject hitObj;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        hitObj = Instantiate(hitObjPrefab, transform.position, Quaternion.identity);
        hitObj.transform.parent = gameObject.transform;
    }


    public void SetLaserHitPoint(Vector3 pos, Vector3 normal, bool hit)
    {
        lineRenderer.SetPosition(0, new Vector3(0.0f, 0.0f, 0.0f));
        lineRenderer.SetPosition(1, transform.InverseTransformPoint(pos));

        if (hit)
        {
            hitObj.SetActive(true);
            Vector3 rayDir = Vector3.Normalize(pos - transform.position);

            // adjust position
            Vector3 blendedNoramal = -0.5f * rayDir + 0.5f * normal;
            hitObj.transform.position = pos + blendedNoramal * 0.01f;

            // adjust orientation 
            hitObj.transform.rotation = Quaternion.LookRotation(blendedNoramal);
        }
        else
        {
            hitObj.SetActive(false);
        }

    }

}
