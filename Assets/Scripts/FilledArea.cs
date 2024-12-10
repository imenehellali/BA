using TMPro;
using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using Mapbox.Examples;
using System;

namespace Area
{

    public class FilledArea : MonoBehaviour
    {
        #region public variables
        public AbstractMap m_map;
        public Camera m_camera_2D;
        public LineRenderer m_line_renderer;
        public Canvas m_canvas;
        public TMP_InputField txt_input;
        public TextMeshProUGUI[] txts;
        #endregion
        #region private variables
        #endregion
        #region main methods
        void Update()
        {
            gameObject.SetActive(false);
        }
        #endregion
        #region helper methods
        //private void OnDisplayInformation()
        //{
        //    Vector3 _pos = m_line_renderer.bounds.center;
        //    Vector2d _latLang = m_map.WorldToGeoPosition(_pos);
        //    foreach (TextMeshProUGUI txt in txts)
        //    {
        //        txt.text =""+_latLang+"\n"+txt_input.text;
        //    }
        //    if (m_camera_2D.gameObject.activeSelf)
        //    {
        //        if (m_camera_2D.orthographicSize > 300)
        //        {
        //            gameObject.GetComponent<MeshRenderer>().enabled = true;
        //            if (m_camera_2D.orthographicSize > 500)
        //            {
        //                m_canvas.gameObject.SetActive(true);
        //            }
        //        }
        //        else
        //        {
        //            gameObject.GetComponent<MeshRenderer>().enabled = false;
        //            m_canvas.gameObject.SetActive(false);
        //        }
        //    }
        //    else
        //    {
        //        m_canvas.gameObject.SetActive(false);
        //        gameObject.GetComponent<MeshRenderer>().enabled = false;
        //    }
        //}
        #endregion
    }
}
