using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectionMapsController : MonoBehaviour
{
    ProjectionMap m_FocusedProjectionMap;
    ProjectionMap m_PreviousFocusedProjectionMap;
    ProjectionMap m_ActiveProjectionMap;
    List<ProjectionMap> m_ProjectionMaps = new();
    GameObject m_HitObject;


    void Start()
    {
        m_ProjectionMaps = gameObject.GetComponentsInChildren<ProjectionMap>().ToList<ProjectionMap>();
    }

    void Update()
    {
        CheckFocus();
        CheckActive();
    }

    void CheckFocus()
    {
        // Avoid dragging over other maps when a map is active
        if (Mouse.current.leftButton.isPressed)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            m_HitObject = hit.transform.gameObject;
            if (m_HitObject.TryGetComponent<ProjectionMap>(out ProjectionMap hitProjectionMap))
            {
                hitProjectionMap.SetFocusedColor();
                m_FocusedProjectionMap = hitProjectionMap;
                m_PreviousFocusedProjectionMap = hitProjectionMap;
            }
            else
            {
                if (m_PreviousFocusedProjectionMap != null)
                {
                    m_PreviousFocusedProjectionMap.SetOriginalColor();
                }
                m_FocusedProjectionMap = null;
            }
        }
        else
        {
            m_FocusedProjectionMap = null;
        }
    }

    void CheckActive()
    {
        if (m_FocusedProjectionMap != null && Mouse.current.leftButton.isPressed)
        {
            m_ActiveProjectionMap = m_FocusedProjectionMap;
            m_ActiveProjectionMap.ShowCorners(true);
        }
        else if (m_HitObject != null && (m_HitObject.layer == LayerMask.NameToLayer("Raycast Plane")) && Mouse.current.leftButton.isPressed)
        {
            if (m_ActiveProjectionMap != null)
            {
                m_ActiveProjectionMap.ShowCorners(false);
                m_ActiveProjectionMap = null;
            }
        }
    }
}
