#if !UNITY_EDITOR
using Google.XR.Cardboard;
#endif
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance { get { return _instance; } }

    [Tooltip("Maximum depth of laser raycast")]
    public float maxRaycastDistance;

    [Tooltip("Specify which layers can be used for interactions")]
    public LayerMask interactionLayerMask;

    [Tooltip("Camera rotation speed when using mouse controller (in debug mode)")]
    public float rotationSensitivity = 1.2f;

    [Tooltip("Maximum camera pitch, around local y axis,  rotation (in degrees)")]
    public float maxCameraPitch = 98.0f;

    [Tooltip("Laser beam prefab")]
    public LaserController laserBeam;

    // Store ref to main scene camera 
    new Camera camera;

    // This is necessary to generate onHoverStart and onHoverEnd events 
    GameObject lastHoveredObject;

    // Used only in debug mode
    private Vector3 cameraRotation;




    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning("Multiple intances of InputManger present in scene!");
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void OnDestroy()
    {
        if (this == _instance)
        {
            _instance = null;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        if (camera == null)
        {
            camera = Camera.main;
        }

        InitializeCardboardAPI();


#if UNITY_EDITOR
        // Lock cursor position in debug mode 
        Cursor.lockState = CursorLockMode.Locked;
#endif
    }

    private void InitializeCardboardAPI()
    {
#if !UNITY_EDITOR
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.brightness = 1.0f;

        // Checks if the device parameters are stored and scans them if not.
        if (!Api.HasDeviceParams())
        {
            Api.ScanDeviceParams();
        }
#endif 
    }


    // Update is called once per frame
    void Update()
    {
        HandleCardboardOnscreenInputs();

        DetectHoveredObject();

        HandleClickEvent();

#if UNITY_EDITOR
        HandleMouseRotation();
#endif
    }

    // Google cardboard API overlays a few buttons onto the game screen (gear & exit)
    // Those events are handles here
    void HandleCardboardOnscreenInputs()
    {
#if !UNITY_EDITOR
        if (Api.IsGearButtonPressed)
        {
            Api.ScanDeviceParams();
        }

        if (Api.IsCloseButtonPressed)
        {
            Application.Quit();
        }

        if (Api.IsTriggerHeldPressed)
        {
            Api.Recenter();
        }

        if (Api.HasNewDeviceParams())
        {
            Api.ReloadDeviceParams();
        }

        Api.UpdateScreenParams();
#endif
    }

    public bool IsButtonPressed()
    {
#if !UNITY_EDITOR
        return (Api.IsTriggerPressed || Input.GetMouseButtonDown(0));
#else
        // Use left click in editor 
        return Input.GetMouseButtonDown(0);
#endif
    }


    private void DetectHoveredObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(laserBeam.transform.position, laserBeam.transform.forward, out hit, maxRaycastDistance, interactionLayerMask))
        {


            if (lastHoveredObject != hit.transform.gameObject)
            {
                // send exit event to previous object
                lastHoveredObject?.SendMessage("OnHoverEnd",SendMessageOptions.DontRequireReceiver);

                // this is a nasty hack but i'll take it 
                if (hit.transform.gameObject.GetComponent<InteractableObject>() != null)
                {
                    lastHoveredObject = hit.transform.gameObject;
                    lastHoveredObject.SendMessage("OnHoverStart");
                }
                else
                {
                    lastHoveredObject = null;
                }

            }

            laserBeam.SetLaserHitPoint(hit.point, hit.normal, true);
        }
        else
        {
            lastHoveredObject?.SendMessage("OnHoverEnd", SendMessageOptions.DontRequireReceiver);
            lastHoveredObject = null;

            Vector3 fakeHitLocation = laserBeam.transform.forward * maxRaycastDistance + laserBeam.transform.position;
            laserBeam.SetLaserHitPoint(fakeHitLocation, -laserBeam.transform.forward, false);
        }
    }


    private void HandleClickEvent()
    {
        // Google Cardboard offers at least one usable button, handle interactions (may not be used)
        if (IsButtonPressed() && lastHoveredObject != null)
        {
            lastHoveredObject.SendMessage("OnClick");
        }
    }


    private void HandleMouseRotation()
    {

        cameraRotation.x += Input.GetAxis("Mouse X") * rotationSensitivity;
        cameraRotation.y += Input.GetAxis("Mouse Y") * rotationSensitivity;
        cameraRotation.y = Mathf.Clamp(cameraRotation.y, -maxCameraPitch, maxCameraPitch);


        var xQuat = Quaternion.AngleAxis(cameraRotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(cameraRotation.y, Vector3.left);

        //Quaternions seem to rotate more consistently than EulerAngles.
        camera.transform.localRotation = xQuat * yQuat;
    }


}
