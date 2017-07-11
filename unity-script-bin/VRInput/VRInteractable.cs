using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// The object that the VRInput can interact with
/// </summary>
[RequireComponent(typeof(BoxCollider), typeof(Image))]
public class VRInteractable : MonoBehaviour {

    #region Events

    [SerializeField]
    private UnityEvent onHover;

    [SerializeField]
    private UnityEvent onUnhover;

    [SerializeField]
    private UnityEvent onPress;

    [SerializeField]
    private UnityEvent onClick;

    #endregion

    private BoxCollider boxCollider;
    private Image interactableImage;

    [SerializeField]
    private Sprite normalImage;

    [SerializeField]
    private Sprite hoverImage;

    [SerializeField]
    private Sprite pressedImage;

    [SerializeField]
    private Sprite inactiveSprite;

    [SerializeField]
    private Color normalColor;

    [SerializeField]
    private Color hoverColor;

    [SerializeField]
    private Color pressedColor;

    [SerializeField]
    private Color inactiveColor;

    void Awake()
    {
        if(onHover == null)
        {
            onHover = new UnityEvent();
        }
        if(onUnhover == null)
        {
            onUnhover = new UnityEvent();
        }
        if(onPress == null)
        {
            onPress = new UnityEvent();
        }
        if(onClick == null)
        {
            onClick = new UnityEvent();
        }
        boxCollider = GetComponent<BoxCollider>();
        interactableImage = GetComponent<Image>();
    }

    void Start()
    {
        boxCollider.size = new Vector3(interactableImage.rectTransform.rect.width, interactableImage.rectTransform.rect.height, 0.0F);
    }

    public void Hover()
    {
        if(onHover != null)
        {
            onHover.Invoke();
        }
        ChangeImageAndColor(hoverImage, hoverColor);
    }

    public void Unhover()
    {
        if(onUnhover != null)
        {
            onUnhover.Invoke();
        }
        ChangeImageAndColor(normalImage, normalColor);
    }

    public void Press()
    {
        Debug.Log("Pressed");
        if (onPress != null)
        {
            onPress.Invoke();
        }
        ChangeImageAndColor(pressedImage, pressedColor);
    }

    public void Unpress()
    {
        Debug.Log("Unpressed");
        ChangeImageAndColor(normalImage, normalColor);
    }

    public void Click()
    {
        Debug.Log("Clickity");
        if (onClick != null)
        {
            onClick.Invoke();
        }
    }

    public void AddOnHoverListener(UnityAction action)
    {
        onHover.AddListener(action);
    }

    public void RemoveOnHoverListener(UnityAction action)
    {
        onHover.RemoveListener(action);
    }

    public void AddOnUnhoverListener(UnityAction action)
    {
        onUnhover.AddListener(action);
    }

    public void RemoveOnUnhoverListener(UnityAction action)
    {
        onUnhover.RemoveListener(action);
    }

    public void AddOnClickListener(UnityAction action)
    {
        onClick.AddListener(action);
    }

    public void RemoveOnClickListener(UnityAction action)
    {
        onClick.RemoveListener(action);
    }

    private void ChangeImageAndColor(Sprite img, Color clr)
    {
        if (interactableImage != null)
        {
            if (img != null)
            {
                interactableImage.sprite = img;
            }
            else
            {
                interactableImage.sprite = normalImage;
            }
            interactableImage.color = clr;
        }
    }
}
