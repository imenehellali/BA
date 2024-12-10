using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;
using TMPro;
using Mapbox.Utils;
using UnityEngine.UI;


public class PaintBrush : MonoBehaviour
{
    #region public variables
    public AbstractMap map;
    public GameObject m_button=null;
    public Vector2d _latLang=new Vector2d(0.0,0.0);
   
    #endregion
    #region main methods
    void Update()
    {
        if (m_button != null)
        {
            _latLang = map.WorldToGeoPosition(realworldPoint: transform.position);
            m_button.GetComponentInChildren<TextMeshProUGUI>().text = "" + _latLang.x + "\n" + _latLang.y;
            m_button.GetComponent<Image>().color = gameObject.GetComponent<MeshRenderer>().material.color;
        }
       
    }
    #endregion
    #region helper methods
   
    #endregion
}
