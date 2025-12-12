using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProjectionMap : MonoBehaviour
{
    [SerializeField] GameObject m_EditableCornerPrefab;

    ArduinoSerialConnector m_ArduinoSerialConnector;
    MeshRenderer m_MeshRenderer;
    MeshFilter m_MeshFilter;
    List<EditableCorner> m_EditableCorners = new();

    void Start()
    {
        m_ArduinoSerialConnector = FindAnyObjectByType<ArduinoSerialConnector>();
        m_MeshRenderer = GetComponent<MeshRenderer>();
        m_MeshFilter = GetComponent<MeshFilter>();

        foreach (Vector3 vertex in m_MeshFilter.mesh.vertices)
        {
            EditableCorner corner = Instantiate(m_EditableCornerPrefab, transform).GetComponent<EditableCorner>();
            corner.transform.position = vertex;
            m_EditableCorners.Add(corner);
            corner.OnMove += OnCornerMove;
        }
    }

    void Update()
    {
        string arduinoData = m_ArduinoSerialConnector.ReadData();
        if (arduinoData != null)
        {
            int data = int.Parse(arduinoData);
            SetAlpha(data / 100f);
        }
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

    void OnCornerMove(EditableCorner corner)
    {
        int indexOfCorner = m_EditableCorners.IndexOf(corner);
        Vector3[] movedVertices = m_MeshFilter.mesh.vertices;
        movedVertices[indexOfCorner] = corner.transform.position;
        m_MeshFilter.mesh.vertices = movedVertices;
    }
}
