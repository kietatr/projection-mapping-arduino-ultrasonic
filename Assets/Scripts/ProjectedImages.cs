using UnityEngine;
using System.Collections.Generic;

public class ProjectedImages : MonoBehaviour
{
    float m_MaxDistance = 200f;
    ArduinoSerialConnector m_ArduinoSerialConnector;
    MeshRenderer m_MeshRenderer;
    ProjectionMapsController m_ProjectionMapsController;

    void Awake()
    {
        m_ArduinoSerialConnector = FindAnyObjectByType<ArduinoSerialConnector>();
        m_MeshRenderer = GetComponent<MeshRenderer>();
        m_ProjectionMapsController = GetComponentInParent<ProjectionMapsController>();
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
        m_MeshRenderer.sharedMaterial.SetFloat("_Alpha", alpha);
    }
}
