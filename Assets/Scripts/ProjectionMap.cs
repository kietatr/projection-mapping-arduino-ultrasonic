using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProjectionMap : MonoBehaviour
{
    [SerializeField] GameObject m_EditableCornerPrefab;

    MeshFilter m_MeshFilter;
    List<EditableCorner> m_EditableCorners = new();

    void Start()
    {
        m_MeshFilter = GetComponent<MeshFilter>();

        foreach (Vector3 vertex in m_MeshFilter.mesh.vertices)
        {
            EditableCorner corner = Instantiate(m_EditableCornerPrefab, transform).GetComponent<EditableCorner>();
            corner.transform.position = vertex;
            m_EditableCorners.Add(corner);
            corner.OnMove += OnCornerMove;
        }
    }

    void OnCornerMove(EditableCorner movedCorner)
    {
        int indexOfMovedCorner = m_EditableCorners.IndexOf(movedCorner);
        Vector3[] movedVertices = m_MeshFilter.mesh.vertices;
        movedVertices[indexOfMovedCorner] = movedCorner.transform.position;
        m_MeshFilter.mesh.vertices = movedVertices;
    }
}
