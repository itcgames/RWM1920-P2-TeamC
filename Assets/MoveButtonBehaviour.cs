using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

public class MoveButtonBehaviour : MonoBehaviour
{
    [Tooltip("Sprite Used For When The Button Is Not Pressed")]
    public Sprite unpressedSprite;
    [Tooltip("Sprite Used For When The Button Is Pressed")]
    public Sprite pressedSprite;
    [Tooltip("Sprite Used For When The Button Is Disabled")]
    public Sprite disabledSprite;
    [Tooltip("Alternate Sprite Used For When The Button Is Not Pressed")]
    public Sprite unpressedAlternateSprite;
    [Tooltip("Alternate Sprite Used For When The Button Is Pressed")]
    public Sprite pressedAlternateSprite;
    [Tooltip("Alternate Sprite Used For When The Button Is Disabled")]
    public Sprite disabledAlternateSprite;
    // bool that stores if the sprite is disabled/active or not
    private bool m_isActive;
    // bool that is ued to switch between main and alterante sprite
    private bool m_isMainSprite;

    [Tooltip("Color For The Move Sprite")]
    public Color mainSpriteColour;
    [Tooltip("Color For The Alternate Functionality Sprite")]
    public Color alternateSpriteColour;

    private void Start()
    {
        m_isMainSprite = true;
    }

    void Update()
    {
        m_isActive = false;
        var components = FindObjectsOfType<ComponentInteraction>();
        foreach (var component in components)
        {
            if (component.GetSelected())
            {
                m_isActive = true;
            }
        }
        if (!m_isActive && GetComponent<SpriteRenderer>().sprite != disabledSprite)
        {
            GetComponent<SpriteRenderer>().sprite = m_isMainSprite ? disabledSprite : disabledAlternateSprite;
        }
        else if (m_isActive && GetComponent<SpriteRenderer>().sprite != unpressedSprite)
        {
            GetComponent<SpriteRenderer>().sprite = m_isMainSprite ? unpressedSprite : unpressedAlternateSprite;
        }
        GetComponent<SpriteRenderer>().color = m_isMainSprite ? mainSpriteColour : alternateSpriteColour;
    }

    private void OnMouseDown()
    {
        if (m_isActive)
        {
            transform.parent.GetComponent<TouchInterfaceBehaviour>().OnChildClick(transform.name);
            GetComponent<SpriteRenderer>().sprite = m_isMainSprite ? pressedSprite : pressedAlternateSprite;
        }
    }

    private void OnMouseUp()
    {
        if (m_isActive)
        {
            GetComponent<SpriteRenderer>().sprite = m_isMainSprite ? unpressedSprite : unpressedAlternateSprite;
        }
    }

    public void ToggleIsSpriteType()
    {
        m_isMainSprite = !m_isMainSprite;
    }
}
