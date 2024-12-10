using Mapbox.Examples;
using Mapbox.Unity.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class EraseAreaMarking : MonoBehaviour,IDragHandler,IEndDragHandler
{
    #region variables
    public InteractionManager m_player;

    
    private Touchscreen touchInput;
    private Vector3 init_pos = Vector3.zero;
    #endregion
    #region Main methods
    void Start()
    {
        EnhancedTouchSupport.Enable();
        touchInput = Touchscreen.current;
        init_pos = transform.position;
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
    private void OnDeleteAreaMarking()
    {
        Vector2 screen_pos = touchInput.touches.ToArray()[0].position.ReadValue();
        var ray = Camera.main.ScreenPointToRay(screen_pos);
        RaycastHit enterNow;
        if (Physics.Raycast(ray, out enterNow))
        {
            if (enterNow.transform.gameObject.GetComponent<MeshCollider>() != null)
            {
                Debug.Log("tag:  " + enterNow.transform.tag);
                //if ( rb.gameObject.CompareTag("Area") && rb.gameObject.GetComponent<PaintBrush>() != null)
                //{
                //    if (rb.gameObject.GetComponent<PaintBrush>().m_button != null)
                //    {
                //        Destroy(rb.gameObject.GetComponent<PaintBrush>().m_button.gameObject);
                //    }
                //    if (rb.gameObject.GetComponent<PaintBrush>().m_contou != null)
                //    {
                //        m_player.m_incident_line_List.Remove(rb.gameObject.GetComponent<PaintBrush>().m_contou);
                //        Destroy(rb.gameObject.GetComponent<PaintBrush>().m_contou.gameObject);
                //    }
                //    m_player.m_incident_areas_List.Remove(rb.gameObject);
                //}
                //Destroy(rb.gameObject);
            }
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        OnDeleteAreaMarking();
        transform.position = init_pos;
        m_player.m_erase = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_player.m_erase = true;
        transform.position = touchInput.touches.ToArray()[0].position.ReadValue();
    }

    #endregion
}
