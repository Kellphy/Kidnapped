using System.Collections;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(Outline))]
public class InteractableObject : MonoBehaviour
{
    [Tooltip("Can be assigned manually (only when multiple outline effects are added to same object)")]
    public Outline outlineEffect;

    [Tooltip("Color of object outline when hovering")]
    public Color hoverOutlineColor;

    [Tooltip("Color of object fill when hovering")]
    public Color hoverFillColor;

    [Tooltip("Thickness of outline in arbitray units (play around with this value)")]
    public float outlineThickness = 4.0f;

    [Tooltip("How much of fill tint should be visible")]
    public float fillOpacity = 0.5f;

    [Tooltip("Use only outline, no infill")]
    public bool outlineOnly;

    [Tooltip("Time after which a click is registered")]
    public float minHoverToClick = 0.75f;

    // handle internal state 
    private bool hovered = false;
    private bool clicked = false;
    private float hoverStartTime;


    // Handle outline & rendering 
    private Material fillMat;
    private Renderer[] renderers;

    // Handle onHover transitions 
    private float currentAlpha = 0.0f;
    Coroutine runningCorutine;

    // Handle headset events 
    public delegate void HeadsetEvent();
    protected HeadsetEvent userHoverStartHandler;
    protected HeadsetEvent userHoverEndHandler;
    protected HeadsetEvent userHoverClickHandler;
    protected HeadsetEvent userButtonClickEvent;

    public void registerHoverStartHandler(HeadsetEvent hoverStartHandler)
    {
        userHoverStartHandler += hoverStartHandler;
    }

    public void registerHoverEndHandler(HeadsetEvent hoverEndHandler)
    {
        userHoverEndHandler += hoverEndHandler;
    }

    public void registerHoverClickHandler(HeadsetEvent hoverClickHandler)
    {
        userHoverClickHandler += hoverClickHandler;
    }

    public void registerButtonClickHandler(HeadsetEvent buttonClickHandler)
    {
        userButtonClickEvent += buttonClickHandler;
    }


    private void Awake()
    {
        // Cache renderers
        renderers = GetComponentsInChildren<Renderer>();

        // Create fill shader 
        fillMat = Instantiate(Resources.Load<Material>(@"DefaultFillMaterial"));

        if (outlineEffect == null)
            outlineEffect = GetComponent<Outline>();
    }

    private void Start()
    {
        // Set configurd color params 
        outlineEffect.OutlineColor = hoverOutlineColor * currentAlpha;
        outlineEffect.OutlineWidth = outlineThickness;
        fillMat.SetColor("_Color", hoverFillColor * currentAlpha);

        // Disable rendering of outline 
        outlineEffect.enabled = false;
        ShowFill(false);

        // Default to sane state
        hovered = false;
        clicked = false;
    }


    private void ShowFill(bool show)
    {
        foreach (var renderer in renderers)
        {
            // Append outline shaders
            var materials = renderer.sharedMaterials.ToList();
            print($"num materials {materials.Count}");
            if (show && !materials.Contains(fillMat))
                materials.Add(fillMat);
            else
                materials.Remove(fillMat);
            renderer.materials = materials.ToArray();
        }
    }

    virtual public void OnHoverStart()
    {
        // Store event start time 
        hoverStartTime = Time.time;
        hovered = true;

        // Handle animations 
        if (runningCorutine != null)
            StopCoroutine(runningCorutine);
        runningCorutine = StartCoroutine(HoverTransition(true));

        // Trigger user configured event 
        if (userHoverStartHandler != null)
            userHoverStartHandler();
    }

    virtual public void OnHoverEnd()
    {
        // Rest state
        hovered = false;
        clicked = false;

        // Handle animations 
        if (runningCorutine != null)
            StopCoroutine(runningCorutine);
        runningCorutine = StartCoroutine(HoverTransition(false));

        // Trigger user configured event
        if (userHoverEndHandler != null)
            userHoverEndHandler();
    }

    public void OnClick()
    {
        // Clear state 
        hovered = false;
        clicked = false;

        // Trigger user configured event
        if (userButtonClickEvent != null)
            userButtonClickEvent();
    }

    private IEnumerator HoverTransition(bool show)
    {
        float transitionSpeed = 10.0f;
        float targetAlpha;

        if (show == true)
        {
            if (!outlineOnly)
                ShowFill(true);


            outlineEffect.enabled = true;
            targetAlpha = 1.0f;
        }
        else
        {
            targetAlpha = 0.0f;
        }


        while (Mathf.Abs(targetAlpha - currentAlpha) > 0.001f)
        {
            if (targetAlpha < currentAlpha)
            {
                currentAlpha -= transitionSpeed * Time.deltaTime;
                currentAlpha = Mathf.Max(currentAlpha, targetAlpha);
            }
            else
            {
                currentAlpha += transitionSpeed * Time.deltaTime;
                currentAlpha = Mathf.Min(currentAlpha, targetAlpha);
            }

            // Update alpha channel
            Color outlineColor = hoverOutlineColor;
            Color fillColor = hoverFillColor;

            outlineColor.a *= currentAlpha;
            fillColor.a *= currentAlpha;

            outlineEffect.OutlineColor = outlineColor;
            fillMat.SetColor("_Color", fillColor);

            yield return null;
        }

        // If onHoverStop transition disable effect 
        if (show == false)
        {
            if (!outlineOnly)
                ShowFill(false);
            outlineEffect.enabled = false;
        }

    }


    private void Update()
    {
        if (hovered && !clicked)
        {
            if (Time.time - hoverStartTime > minHoverToClick)
            {
                // Trigger user defined event 
                if (userHoverClickHandler != null)
                    userHoverClickHandler();

                // prevent re-entry
                clicked = true;
            }
        }
    }

    public bool IsHovered()
    {
        return hovered;
    }
}
