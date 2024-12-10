using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mapbox.Examples;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Examples;


public class IconCorrectView : MonoBehaviour
{
    #region public variables
    public Transform m_target;
    public Camera m_camera_2D;
    public Camera m_camera_3D;
    public AbstractMap map;
    public Vector3 touch_point;
    public Vector3 Pos { get; set; } = Vector3.zero;
    public int size_of_wagon;
    public Button validate_button;
    public Button delete_button;
    public Vector2d dynamic_latLang;
    public GameObject fire_truck;
    #endregion
    #region private variables
    private GameObject[] wagons;
    private float m_scale = 0f;
    #endregion
    #region main methods
    void Start()
    {
        validate_button.gameObject.SetActive(false);
        delete_button.gameObject.SetActive(false);
        OnFollowPlayerFieldOfView();
        wagons = new GameObject[size_of_wagon];
        //for (int i = 0; i < size_of_wagon; i++)
        //{
        //    wagons[i] = Instantiate(fire_truck, touch_point, Quaternion.identity);
        //    wagons[i].GetComponent<Wagon>().touch_point = touch_point;
        //    wagons[i].transform.localScale = new Vector3(.01f, .01f, .01f);
        //    wagons[i].GetComponent<Wagon>().latLang = map.WorldToGeoPosition(wagons[i].transform.position);
        //    wagons[i].GetComponent<Wagon>().m_camera_2D = m_camera_2D;
        //    wagons[i].GetComponent<Wagon>().m_camera_3D = m_camera_3D;
        //    wagons[i].GetComponentInChildren<Canvas>().worldCamera = m_camera_2D;
        //    wagons[i].GetComponent<Wagon>().m_palyer = m_target.gameObject;
        //    wagons[i].transform.SetParent(transform, true);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        OnFollowPlayerFieldOfView();
        OnCorrectVisualization();
        //OnDynamicGeoPosition();
    }
    #endregion
    #region helper methods
    private void OnFollowPlayerFieldOfView()
    {
        if (m_camera_2D.gameObject.activeSelf)
        {
            transform.rotation = Quaternion.Euler(new Vector3(90f, transform.rotation.eulerAngles.y, 0f));
            transform.up = m_target.forward;
            transform.position = new Vector3(transform.position.x, 8f, transform.position.z);

        }
    }

    public void onClickButton()
    {
        m_target.gameObject.GetComponent<InteractionManager>().OnEditClose();
        validate_button.gameObject.SetActive(false);
        delete_button.gameObject.SetActive(false);
    }
    public void OnDeleteObject()
    {
        gameObject.Destroy();
    }
    public void OnCorrectVisualization()
    {
        if (m_camera_2D.gameObject.activeSelf)
        {
            if (m_camera_2D.orthographicSize < 500f)
            {
                m_scale = m_camera_2D.orthographicSize * 0.15f;
            }
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            transform.localScale = new Vector3(m_scale, m_scale, 10f);
            //foreach (GameObject wagon in wagons)
            //{
            //    wagon.SetActive(false);
            //}
        }

    }
    private void OnDynamicGeoPosition()
    {
        Vector3 pos = transform.position;
        if (wagons != null)
        {
            foreach (GameObject wagon in wagons)
            {
                pos.x += wagon.transform.position.x;
                pos.y += wagon.transform.position.y;
                pos.z += wagon.transform.position.z;
            }

            //Debug.Log("wagon pos:  " + Vector3.Scale(pos,new Vector3(1/wagons.Length, 1/wagons.Length, 1/wagons.Length)));
        }

    }
    #endregion
}
