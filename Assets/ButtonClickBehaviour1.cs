using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Script is used for the behaviour of the buttons used on the Touch Interface
/// for the component interaction
/// </summary>
public class ButtonClickBehaviour : MonoBehaviour
{
    [Tooltip("Sprite Used For When The Button Is Not Pressed")]
    public Sprite unpressedSprite;
    [Tooltip("Sprite Used For When The Button Is Pressed")]
    public Sprite pressedSprite;
    [Tooltip("Sprite Used For When The Button Is Disabled")]
    public Sprite disabledSprite;
    // bool that stores if the sprite is disabled/active or not
    private bool m_isActive;

    private void Update()
    {
        m_isActive = false;
        var components = FindObjectsOfType<ComponentInteraction>();
        foreach (var component in components)
        {
            if(component.GetSelected())
            {
                m_isActive = true;
            }
        }
        if(m_isActive && GetComponent<SpriteRenderer>().sprite != disabledSprite)
        {
            GetComponent<SpriteRenderer>().sprite = disabledSprite;
        }
    }
    private void OnMouseDown()
    {
        if (m_isActive)
        {
            transform.parent.GetComponent<TouchInterfaceBehaviour>().OnChildClick(transform.name);
            GetComponent<SpriteRenderer>().sprite = pressedSprite;
        }
    }

    private void OnMouseUp()
    {
        if (m_isActive)
        {
            GetComponent<SpriteRenderer>().sprite = unpressedSprite;
        }
    }
}
