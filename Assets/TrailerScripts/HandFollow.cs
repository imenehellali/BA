using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
public class HandFollow : MonoBehaviour
{
    #region variables
    public Image m_hand_point;
    public Image m_hand_zoom;

    private Touchscreen touchInput;
    #endregion
    void Start()
    {
        EnhancedTouchSupport.Enable();
        touchInput = Touchscreen.current;
        m_hand_zoom.enabled = false;
        m_hand_point.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        m_hand_zoom.enabled = false;
        m_hand_point.enabled = false;

        if (touchInput.wasUpdatedThisFrame && touchInput.touches.ToArray()[0].press.isPressed && touchInput.touches.ToArray()[1].press.isPressed)
        {
            m_hand_zoom.enabled = true;
            m_hand_point.enabled = false;
        }
        else if (touchInput.wasUpdatedThisFrame && touchInput.touches.ToArray()[0].press.isPressed && !touchInput.touches.ToArray()[1].press.isPressed)
        {
            m_hand_zoom.enabled = false;
            m_hand_point.enabled = true;
        }
        m_hand_point.gameObject.transform.position =touchInput.touches.ToArray()[0].position.ReadValue();
        m_hand_point.gameObject.transform.position = touchInput.touches.ToArray()[0].position.ReadValue();

    }
}
