using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A cursor representing with which object the user will interact
/// </summary>
public class VRCursor : MonoBehaviour {

    private Transform cursorTransform;

    private Renderer cursorRenderer;

    void Awake()
    {
        cursorTransform = GetComponent<Transform>();
        cursorRenderer = GetComponent<Renderer>();
    }

    public void MoveCursor(Vector3 newPosition, Vector3 forward)
    {
        cursorTransform.position = newPosition;
        cursorTransform.rotation = Quaternion.LookRotation(forward);
    }

    public void HideCursor()
    {
        cursorRenderer.enabled = false;
    }

    public void ShowCursor()
    {
        cursorRenderer.enabled = true;
    }

    public void SetShowCursor(bool value)
    {
        cursorRenderer.enabled = value;
    }
}
