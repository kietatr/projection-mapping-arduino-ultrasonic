using UnityEngine;
using System.Collections.Generic;

public class ProjectedImages : MonoBehaviour
{
    float m_MaxDistance = 200f;
    ArduinoSerialConnector m_ArduinoSerialConnector;
    MeshRenderer m_MeshRenderer;

    void Awake()
    {
        m_ArduinoSerialConnector = FindAnyObjectByType<ArduinoSerialConnector>();
        m_MeshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (m_ArduinoSerialConnector != null)
        {
            string arduinoData = m_ArduinoSerialConnector.ReadData();
            if (arduinoData != null)
            {
                int data = int.Parse(arduinoData);
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
