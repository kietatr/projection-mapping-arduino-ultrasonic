using UnityEngine;

public class ProjectionMap : MonoBehaviour
{
    ArduinoSerialConnector m_ArduinoSerialConnector;
    MeshRenderer m_MeshRenderer;

    void Start()
    {
        m_ArduinoSerialConnector = FindAnyObjectByType<ArduinoSerialConnector>();
        m_MeshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        string arduinoData = m_ArduinoSerialConnector.ReadData();
        int data = int.Parse(arduinoData);

        SetAlpha(data / 100f);
    }

    void SetAlpha(float alpha)
    {
        Color currentColor = m_MeshRenderer.sharedMaterial.color;
        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
        m_MeshRenderer.sharedMaterial.color = newColor;
    }

    void OnDisable()
    {
        SetAlpha(1f);
    }
}
