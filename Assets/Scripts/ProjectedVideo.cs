using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEditor;

public class ProjectedVideo : MonoBehaviour
{
    float m_MaxDistance = 200f;
    ArduinoSerialConnector m_ArduinoSerialConnector;
    MeshRenderer m_MeshRenderer;
    ProjectionMapsController m_ProjectionMapsController;
    VideoPlayer m_VideoPlayer;

    void Awake()
    {
        m_ArduinoSerialConnector = FindAnyObjectByType<ArduinoSerialConnector>();
        m_MeshRenderer = GetComponent<MeshRenderer>();
        m_ProjectionMapsController = GetComponentInParent<ProjectionMapsController>();

        m_VideoPlayer = GetComponent<VideoPlayer>();
        m_VideoPlayer.Stop();
        m_VideoPlayer.prepareCompleted += OnVideoPrepared;
        m_VideoPlayer.sendFrameReadyEvents = true;
        m_VideoPlayer.frameReady += OnVideoFrameReady;
        m_VideoPlayer.Prepare();
    }

    void Update()
    {
        if (m_ArduinoSerialConnector != null && !m_ProjectionMapsController.PauseArduinoInputData)
        {
            string arduinoData = m_ArduinoSerialConnector.ReadData();
            if (arduinoData != null)
            {
                float data = float.Parse(arduinoData);
                SetAlpha(data / m_MaxDistance);
            }
        }
    }

    void OnDisable()
    {
        SetAlpha(0f);
    }

    void SetAlpha(float alpha)
    {
        if (m_VideoPlayer.isPrepared)
        {
            int frameToShow = Mathf.RoundToInt(MyMathUtils.Remap(alpha, 0f, 1f, 0f, m_VideoPlayer.frameCount - 1));

            m_VideoPlayer.Play();
            m_VideoPlayer.frame = frameToShow;
            m_VideoPlayer.Pause();
        }
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        vp.Pause();
    }

    void OnVideoFrameReady(VideoPlayer source, long frameIndex)
    {
        Debug.Log(frameIndex);
        m_MeshRenderer.sharedMaterial.SetTexture("_BaseMap", source.texture);
    }
}
