using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Lights : MonoBehaviour
{
    public static Lights instance;
    public float lightFeedbackTime = 2;
    public Light[] lights;
    //Default 3.25
    public float Range;

    public Color failColor;
    public Color successColor;

    [Tooltip("Minimum random light intensity")]
    public float minIntensity = 0.5f;
    [Tooltip("Maximum random light intensity")]
    public float maxIntensity = 0.5f;
    [Tooltip("How much to smooth out the randomness; lower values = sparks, higher = lantern")]
    [Range(1, 50)]
    public int smoothing = 50;

    // Continuous average calculation via FIFO queue
    // Saves us iterating every time we update, we just change by the delta
    Queue<float> smoothQueue;

    public void Reset()
    {
        smoothQueue.Clear();
        lastSum = 0;
    }

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        smoothQueue = new Queue<float>(smoothing);
    }


    float lastSum = 0;

    private void Update()
    {
        foreach (Light light in lights)
        {
            light.range = Range;

            // pop off an item if too big
            while (smoothQueue.Count >= smoothing)
            {
                lastSum -= smoothQueue.Dequeue();
            }

            // Generate random new item, calculate new average
            float newVal = Random.Range(minIntensity, maxIntensity);
            smoothQueue.Enqueue(newVal);
            lastSum += newVal;

            // Calculate new smoothed average
            light.intensity = lastSum / (float)smoothQueue.Count;
        }
    }

    public void LightsFeedback(bool success)
    {
        if (success)
        {
            StartCoroutine(ChangeColor(successColor));
        }
        else
        {
            StartCoroutine(ChangeColor(failColor));
        }
    }

    IEnumerator ChangeColor(Color color)
    {
        Color initColor = lights[0].color;
        float initMinIntensity = minIntensity;
        float initMaxIntensity = maxIntensity;

        minIntensity = 0.25f;
        maxIntensity = 0.75f;

        foreach (var light in lights)
        {
            light.color = color;
        }
        yield return new WaitForSeconds(lightFeedbackTime);

        foreach (var light in lights)
        {
            light.color = initColor;
        }

        minIntensity = initMinIntensity;
        maxIntensity = initMaxIntensity;
    }
}
