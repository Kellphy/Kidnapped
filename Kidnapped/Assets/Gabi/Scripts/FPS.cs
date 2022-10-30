using UnityEngine;

public class FPS : MonoBehaviour
{
    public TMPro.TextMeshProUGUI fps_Text;

    float deltaTime = 0.0f;

    //int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    //float m_lastFramerate = 0.0f;
    float m_refreshTime = 0.5f;


    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;

        string text = string.Format("{1:0.} FPS ({0:0.0} ms)", msec, fps);

        if (m_timeCounter < m_refreshTime)
        {
            m_timeCounter += Time.deltaTime;
            //m_frameCounter++;
        }
        else
        {
            //This code will break if you set your m_refreshTime to 0, which makes no sense.
            //m_lastFramerate = (float)m_frameCounter / m_timeCounter;
            //m_frameCounter = 0;
            m_timeCounter = 0.0f;
            fps_Text.text = text;
            //fps_Text.text = ((int)m_lastFramerate).ToString();
        }
    }
}
