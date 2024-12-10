using Mapbox.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

public class DRagIconBehavior : MonoBehaviour, IDragHandler, IEndDragHandler
{
    #region variables
    public InteractionManager m_player;
    public int m_iconSpawnCode;
    public GameObject m_fake;

    private Plane _yPlane;
    private Touchscreen touchInput;
    #endregion
    #region Main methods
    void Start()
    {
        EnhancedTouchSupport.Enable();
        _yPlane.SetNormalAndPosition(transform.up, transform.position);
        touchInput = Touchscreen.current;
    }

    // Update is called once per frame
    void Update()
    {
        touchInput = Touchscreen.current;
        if (touchInput == null)
        {
            return;
        }
    }
    #endregion
    #region helper methods
    public void OnEndDrag(PointerEventData eventData)
    {
        m_fake.SetActive(false);
        m_player.touch_point.transform.position = m_player.OnPlanePositionFromScreenPoint(touchInput.touches.ToArray()[0].position.ReadValue());
        m_player.SpawnTaKTischePrefOnButtonPress(m_iconSpawnCode);
        m_player.m_placingIcons = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_fake.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
        m_fake.SetActive(true);
        m_fake.transform.position = touchInput.touches.ToArray()[0].position.ReadValue();
        m_player.m_placingIcons = true;
    }

    #endregion
}
