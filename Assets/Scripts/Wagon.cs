using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using TouchControl = UnityEngine.InputSystem.Controls.TouchControl;
using TouchPhaseControl = UnityEngine.InputSystem.Controls.TouchPhaseControl;
using Mapbox.Examples;
public class Wagon : MonoBehaviour
{
    #region public variables
    public Vector2d latLang;
    //public string strasse="";
    public Camera m_camera_2D;
    public Camera m_camera_3D;
    public GameObject m_palyer;
    public Vector3 touch_point;
    public Canvas canvas;
    #endregion
    #region private variables
    private TextMeshProUGUI text;
    #endregion
    #region main methods
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        canvas = GetComponentInChildren<Canvas>();
        canvas.gameObject.SetActive(false);
        transform.position = new Vector3(touch_point.x, 0f, touch_point.z);
        transform.localScale = new Vector3(.01f, .01f, .01f);
        EnhancedTouchSupport.Enable();
    }

    void Update()
    {
        var touchInput = Touchscreen.current;
        if (touchInput == null)
        {
            return;
        }
        var deltaOne = Vector3.zero;
        if (touchInput.wasUpdatedThisFrame && m_camera_3D.gameObject.activeSelf)
        {
            OnWagonInfoDisplay(touchInput);
        }
        OnWagonMove();
        OnWagonInofPositioning();
    }
    #endregion
    #region helper methods
    private void OnWagonMove()
    {
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        transform.localScale = new Vector3(.01f, .01f, .01f);
    }
    private void OnWagonInofPositioning()
    {
        if (m_camera_2D.gameObject.activeSelf)
        {
            text.text = "lat: " + latLang.x + "\nlang: " + latLang.y;
            canvas.transform.up = transform.forward;
            canvas.gameObject.SetActive(false);
            
        }
        else if (m_camera_3D.gameObject.activeSelf)
        {
            text.text = "lat: " + latLang.x + "\nlang: " + latLang.y;
            canvas.transform.forward = m_palyer.transform.forward;
        }
    }
    private void OnWagonInfoDisplay(Touchscreen touch)
    {
        var ray = Camera.main.ScreenPointToRay(touch.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null && rb.gameObject.CompareTag("Wagon"))
            {
                if (!rb.gameObject.GetComponent<Wagon>().canvas.gameObject.activeSelf)
                {
                    rb.gameObject.GetComponent<Wagon>().canvas.gameObject.SetActive(true);
                }
                else if (rb.gameObject.GetComponent<Wagon>().canvas.gameObject.activeSelf)
                {
                    rb.gameObject.GetComponent<Wagon>().canvas.gameObject.SetActive(false);
                }
            }
        }
    }
        
    #endregion
}
