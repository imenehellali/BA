using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mapbox.Unity.Map;
using TopDownCameraController;
using Mapbox.Examples;
public class PointerPlayerFollow : MonoBehaviour
{
    #region variables
    public Image m_pointer;
    public AbstractMap map;
    public Camera m_camera_3D;
    public Camera m_camera_2D;
    #endregion
    #region main functions
    void Start()
    {

        if (m_camera_2D.gameObject.activeSelf)
        {
            m_pointer.gameObject.SetActive(false);
        }
        else if (m_camera_3D.gameObject.activeSelf)
        {
            m_pointer.gameObject.SetActive(true);
            OnPlayerMoved3D();
        }
    }

    void Update()
    {
        if (m_camera_2D.gameObject.activeSelf)
        {
        }
        else if (m_camera_3D.gameObject.activeSelf)
        {
            OnPlayerMoved3D();
        }
    }
    #endregion
    #region helper functions
    private void OnPlayerMoved3D()
    {
        m_pointer.GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(45f, 0, -Vector3.SignedAngle(transform.forward, map.transform.forward, Vector3.up)));
    }
    private void OnPlayerMoved2D()
    {
        m_pointer.GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 0, Vector3.SignedAngle(transform.forward, map.transform.forward, Vector3.up)));
    }
    #endregion
}
