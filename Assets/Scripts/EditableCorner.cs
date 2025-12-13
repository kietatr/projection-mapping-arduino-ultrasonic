using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class EditableCorner : MonoBehaviour
{
    [SerializeField] Color m_FocusColor = Color.white;
    [SerializeField] Color m_ActiveColor = new(0.2f, 0.2f, 0.2f);
    [SerializeField] LayerMask m_RaycastPlane;

    public event Action<EditableCorner> OnFocus;
    public event Action<EditableCorner> OnUnfocus;
    public event Action<EditableCorner> OnMove;

    Color m_OriginalColor;
    MeshRenderer m_MeshRenderer;
    bool m_IsFocused;
    bool m_IsActive;

    void Awake()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
        m_OriginalColor = m_MeshRenderer.material.color;
    }

    void Update()
    {
        CheckFocus();
        CheckActive();
        CheckMoveWithMouse();
    }

    void CheckFocus()
    {
        // Avoid dragging over other corners when a corner is active
        if (Mouse.current.leftButton.isPressed)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject == gameObject)
            {
                m_MeshRenderer.material.color = m_FocusColor;
                m_IsFocused = true;
                OnFocus?.Invoke(this);
            }
            else
            {
                m_MeshRenderer.material.color = m_OriginalColor;
                m_IsFocused = false;
                OnUnfocus?.Invoke(this);
            }
        }
        else
        {
            m_MeshRenderer.material.color = m_OriginalColor;
            m_IsFocused = false;
            OnUnfocus?.Invoke(this);
        }
    }

    void CheckActive()
    {
        if (m_IsFocused && Mouse.current.leftButton.isPressed)
        {
            m_IsActive = true;
        }
        else if (!m_IsFocused && Mouse.current.leftButton.isPressed)
        {
            if (m_IsActive)
            {
                m_IsActive = true;
            }
        }
        else if (!Mouse.current.leftButton.isPressed)
        {
            m_IsActive = false;
        }

        if (m_IsActive)
        {
            m_MeshRenderer.material.color = m_ActiveColor;
        }
    }

    void CheckMoveWithMouse()
    {
        if (m_IsActive)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 10, m_RaycastPlane))
            {
                transform.position = new Vector3(hit.point.x, hit.point.y, 0f);
                OnMove?.Invoke(this);
            }
        }
    }
}
