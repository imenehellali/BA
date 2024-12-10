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

public class MarkBuildingBehavior : MonoBehaviour, IDragHandler, IEndDragHandler
{
    #region variables
    public Material m_solidBlue;
    public Material m_solidBurgundy;
    public InteractionManager m_player;

    private Plane _yPlane;
    private Touchscreen touchInput;
    private Vector3 init_pos=Vector3.zero;
    #endregion
    #region Main methods
    void Start()
    {
        EnhancedTouchSupport.Enable();
        _yPlane.SetNormalAndPosition(transform.up, transform.position);
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
    private void MarkDeMarkBuilding()
    {
        Vector2 screen_pos = touchInput.touches.ToArray()[0].position.ReadValue();

        var ray = Camera.main.ScreenPointToRay(screen_pos);
        RaycastHit enterNow;
        if (Physics.Raycast(ray, out enterNow))
        {
            //enterNow.transform.gameObject.name.Contains("3D")
            if (enterNow.transform.gameObject.GetComponent<MeshCollider>()!=null)
            {
                if (enterNow.transform.gameObject.GetComponent<MeshRenderer>().materials != null)
                {
                    if(enterNow.transform.gameObject.GetComponent<MeshRenderer>().materials[0].color == m_solidBurgundy.color)
                    {
                        enterNow.transform.gameObject.GetComponent<MeshRenderer>().materials[0].color = m_solidBlue.color;
                    }
                    else if(enterNow.transform.gameObject.GetComponent<MeshRenderer>().materials[0].color == m_solidBlue.color)
                    {
                        enterNow.transform.gameObject.GetComponent<MeshRenderer>().materials[0].color = m_solidBurgundy.color;
                    }
                   
                }
                else
                {
                    Debug.Log("Material is null");
                }

            }
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        MarkDeMarkBuilding();
        transform.position = init_pos;
        m_player.m_markingBuilding = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_player.m_markingBuilding = true;
        transform.position = touchInput.touches.ToArray()[0].position.ReadValue();
    }

    #endregion
}
