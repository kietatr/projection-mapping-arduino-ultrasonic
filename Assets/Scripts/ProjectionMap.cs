using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProjectionMap : MonoBehaviour
{
    [SerializeField] GameObject m_EditableCornerPrefab;

    public List<EditableCorner> EditableCorners {get; private set;}

    MeshFilter m_MeshFilter;
    MeshRenderer m_MeshRenderer;
    MeshCollider m_MeshCollider;
    Color m_FocusedColor = new Color(0.5f, 0.5f, 0.5f);
    Color m_OriginalColor = Color.white;

    void Start()
    {
        EditableCorners = new();
        m_MeshFilter = GetComponent<MeshFilter>();
        m_MeshRenderer = GetComponent<MeshRenderer>();
        m_MeshCollider = GetComponent<MeshCollider>();

        foreach (Vector3 vertex in m_MeshFilter.mesh.vertices)
        {
            EditableCorner corner = Instantiate(m_EditableCornerPrefab, transform).GetComponent<EditableCorner>();
            corner.transform.localPosition = vertex;
            EditableCorners.Add(corner);
            corner.OnMove += OnCornerMove;
        }

        ShowCorners(false);
    }

    void OnCornerMove(EditableCorner movedCorner)
    {
        int indexOfMovedCorner = EditableCorners.IndexOf(movedCorner);
        Vector3[] movedVertices = m_MeshFilter.mesh.vertices;
        movedVertices[indexOfMovedCorner] = movedCorner.transform.localPosition;
        m_MeshFilter.mesh.vertices = movedVertices;
        m_MeshCollider.sharedMesh = m_MeshFilter.mesh;
    }

    public void ShowCorners(bool show)
    {
        foreach (EditableCorner corner in EditableCorners)
        {
            corner.gameObject.SetActive(show);
        }
    }

    public void SetColor(Color color)
    {
        m_MeshRenderer.sharedMaterial.SetColor("_Color", color);
    }

    public void SetFocusedColor()
    {
        SetColor(m_FocusedColor);
    }

    public void SetOriginalColor()
    {
        SetColor(m_OriginalColor);
    }
}
