using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the touchpad input, cursor movement, hovering and unhovering of interactables, etc.
/// </summary>
public class VRInput : MonoBehaviour {

    private Transform objectTransform;
    private VRInteractable activeInteractable = null;

    private VRInteractable clickedInteractable = null;

    private VRCursor cursor;

    private bool cursorEnabled = false;
    private bool inputEnabled = false;

    [SerializeField]
    private float inputDistance = 5f;

    public float InputDistance
    {
        get
        {
            return inputDistance;
        }
        set
        {
            inputDistance = value;
        }
    }

    private LayerMask inputMask;
    public LayerMask InputMask
    {
        get { return inputMask; }
        set { inputMask = value; }
    }

    private bool isClicked = false;

    private Ray inputRay;
    private RaycastHit inputHit;

    void Awake()
    {
        objectTransform = GetComponent<Transform>();
        inputRay = new Ray();
        inputHit = new RaycastHit();
    }
	
	// Update is called once per frame
	void Update () {
		if(inputEnabled)
        {
            Vector3 cursorPosition = objectTransform.position + objectTransform.forward * inputDistance;
            inputRay.origin = objectTransform.position;
            inputRay.direction = objectTransform.forward;
            if(Physics.Raycast(inputRay, out inputHit, inputDistance, inputMask))
            {
                VRInteractable interactable = inputHit.collider.GetComponent<VRInteractable>();
                if (interactable != null)
                {
                    cursorPosition = objectTransform.position + objectTransform.forward * inputHit.distance;
                    if(interactable != activeInteractable)
                    {
                        // lights out for active, lights on for the new fella' and switch places
                        if(activeInteractable != null)
                        {
                            activeInteractable.Unhover();
                        }
                        activeInteractable = interactable;
                        activeInteractable.Hover();
                    }
                }
            }
            else
            {
                if (activeInteractable != null)
                {
                    // lights out for the last interactable
                    activeInteractable.Unhover();
                    activeInteractable = null;
                }
            }
            if(Input.GetAxis("Fire1") > 0f && !isClicked)
            {
                clickedInteractable = activeInteractable;
                if(clickedInteractable != null)
                {
                    clickedInteractable.Press();
                }
                isClicked = true;
            }
            else if(Input.GetAxis("Fire1") == 0f && isClicked)
            {
                if(clickedInteractable != null)
                {
                    clickedInteractable.Unpress();
                    if (clickedInteractable == activeInteractable)
                    {
                        clickedInteractable.Click();
                        activeInteractable.Hover();
                    }
                    clickedInteractable = null;
                }
                isClicked = false;
            }
            cursor.MoveCursor(cursorPosition, objectTransform.forward);
        }
	}

    public void SetCursor(VRCursor c)
    {
        cursor = c;
    }

    public void SetInputEnabled(bool value)
    {
        inputEnabled = value;
    }

    public void SetCursorEnabled(bool value)
    {
        cursorEnabled = value;
        if(cursor != null)
        {
            cursor.SetShowCursor(value);
        }
    }
}
