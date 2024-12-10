using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mapbox.Examples;
using UnityEngine.UI;
using Mapbox.Unity.Map;
using System;

public class CustomTacticalGraphics : MonoBehaviour
{
    #region public methods
    //comon Objects
    public Canvas m_canvas;
    public GameObject m_player;
    public GameObject m_side_menu;
    public AbstractMap m_map;
    public Camera m_camera_2D;
    public Camera m_camera_3D;

    public TextMeshProUGUI m_display_text;
    public TMP_InputField m_edit_text;
    public GameObject m_detail_obj_0;
    public GameObject m_detail_obj_1;


    public Sprite[] m_shape;
    public Sprite[] m_quantity_top;
    public Sprite[] m_quantity_bottom;
    public Sprite[] m_detail;

    public GameObject m_quantity_obj_top;
    public GameObject m_quantity_obj_bottom;
    public GameObject m_save_button;
    public GameObject m_delete_button;

    public Sprite m_empty_sprite;
    #endregion

    #region solid colors
    //Colors
    private Color red = new Color32(255, 0, 0, 255);
    private Color yellow = new Color32(255, 255, 0, 255);
    private Color blue = new Color32(0, 0, 255, 255);
    private Color brown = new Color32(153, 102, 51, 255);
    private Color purple = new Color32(255, 0, 255, 255);
    private Color orange = new Color32(255, 160, 0, 255);
    private Color green = new Color32(0, 128, 0, 255);
    #endregion

    #region different text fields
    //4 text
    public GameObject m_4_field;
    public TextMeshProUGUI m_4_field_txt_0;
    public TMP_InputField m_4_field_input_0;
    public TextMeshProUGUI m_4_field_txt_1;
    public TMP_InputField m_4_field_input_1;
    public TextMeshProUGUI m_4_field_txt_2;
    public TMP_InputField m_4_field_input_2;
    public TextMeshProUGUI m_4_field_txt_3;
    public TMP_InputField m_4_field_input_3;
    //2 text
    public GameObject m_2_field;
    public TextMeshProUGUI m_2_field_txt_0;
    public TMP_InputField m_2_field_input_0;
    public TextMeshProUGUI m_2_field_txt_1;
    public TMP_InputField m_2_field_input_1;
    //1 center
    public GameObject m_1_center;
    public TMP_InputField m_1_center_text;
    //1 bottom
    public GameObject m_1_bottom;
    public TMP_InputField m_1_bottom_text;

    //Prefabs
    public GameObject fire_truck;
    #endregion

    #region private variables
    private GameObject[] wagons;
    private int m_shapeSelected = 0;
    private int m_factor = 2;
    private float m_scale = 0f;
    #endregion
    #region main methods
    void Start()
    {
        m_display_text.gameObject.SetActive(false);
        transform.position = new Vector3(transform.position.x, 18f, transform.position.z);
        //OnWagoncreation();
    }

    // Update is called once per frame
    void Update()
    {
        m_display_text.text = m_side_menu.GetComponentInChildren<TMP_InputField>().text;
        OnTextUpdate();
        OnDisplay();
    }
    #endregion
    #region behavior methods
    private void OnTextUpdate()
    {
        if (m_4_field.activeSelf)
        {
            m_4_field_txt_0.text = m_4_field_input_0.text;
            m_4_field_txt_1.text = m_4_field_input_1.text;
            m_4_field_txt_2.text = m_4_field_input_2.text;
            m_4_field_txt_3.text = m_4_field_input_3.text;
        }
        else if (m_2_field.activeSelf)
        {
            m_2_field_txt_0.text = m_2_field_input_0.text;
            m_2_field_txt_1.text = m_2_field_input_1.text;
        }
        else if (m_1_center.activeSelf)
        {
            m_1_center.GetComponent<TextMeshProUGUI>().text = m_1_center_text.text;
        }
        else if (m_1_bottom.activeSelf)
        {
            m_1_bottom.GetComponent<TextMeshProUGUI>().text = m_1_bottom_text.text;
        }
    }

    private void OnDisplay()
    {
        if (m_camera_3D.gameObject.activeSelf)
        {
            m_side_menu.SetActive(false);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            m_detail_obj_0.GetComponent<SpriteRenderer>().enabled = false;
            m_detail_obj_1.GetComponent<SpriteRenderer>().enabled = false;
            m_quantity_obj_top.GetComponent<SpriteRenderer>().enabled = false;
            m_quantity_obj_bottom.GetComponent<SpriteRenderer>().enabled = false;

            //foreach (GameObject wagon in wagons)
            //{
            //    wagon.SetActive(true);
            //}
        }
        else if (m_camera_2D.gameObject.activeSelf && !m_side_menu.activeSelf)
        {
            //foreach (GameObject wagon in wagons)
            //{
            //    wagon.SetActive(false);
            //}
            if (m_camera_2D.orthographicSize < 900)
            {
                m_scale = m_camera_2D.orthographicSize * 0.1f;
            }
            gameObject.transform.localScale = new Vector3(m_scale, m_scale, 10f);
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            m_detail_obj_0.GetComponent<SpriteRenderer>().enabled = true;
            m_detail_obj_1.GetComponent<SpriteRenderer>().enabled = true;
            m_quantity_obj_top.GetComponent<SpriteRenderer>().enabled = true;
            m_quantity_obj_bottom.GetComponent<SpriteRenderer>().enabled = true;

        }
        else if (m_camera_2D.gameObject.activeSelf && m_side_menu.activeSelf)
        {
            //foreach (GameObject wagon in wagons)
            //{
            //    wagon.SetActive(false);
            //}
            m_scale = m_camera_2D.orthographicSize * 0.08f;
            gameObject.transform.localScale = new Vector3(m_scale, m_scale, 10f);
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            m_detail_obj_0.GetComponent<SpriteRenderer>().enabled = true;
            m_detail_obj_1.GetComponent<SpriteRenderer>().enabled = true;
            m_quantity_obj_top.GetComponent<SpriteRenderer>().enabled = true;
            m_quantity_obj_bottom.GetComponent<SpriteRenderer>().enabled = true;


        }
    }
    //private void OnWagoncreation()
    //{
    //    wagons = new GameObject[3];
    //    for (int i = 0; i < 3; i++)
    //    {
    //        wagons[i] = Instantiate(fire_truck, transform.position, Quaternion.identity);
    //        wagons[i].GetComponent<Wagon>().touch_point = transform.position;
    //        wagons[i].transform.localScale = new Vector3(.01f, .01f, .01f);
    //        wagons[i].GetComponent<Wagon>().latLang = m_map.WorldToGeoPosition(wagons[i].transform.position);
    //        wagons[i].GetComponent<Wagon>().m_camera_2D = m_camera_2D;
    //        wagons[i].GetComponent<Wagon>().m_camera_3D = m_camera_3D;
    //        wagons[i].GetComponentInChildren<Canvas>().worldCamera = m_camera_2D;
    //        wagons[i].GetComponent<Wagon>().m_palyer = m_player;
    //        wagons[i].transform.SetParent(transform, true);
    //    }
    //    foreach (GameObject wagon in wagons)
    //    {
    //        wagon.SetActive(true);
    //    }
    //}
    #endregion
    #region button / creation methods
    public void OnDetailSelect(int detail_code)
    {
        if (m_shapeSelected % m_factor == 0)
        {
            m_detail_obj_0.GetComponent<SpriteRenderer>().sprite = m_detail[detail_code];
        }
        else
        {
            m_detail_obj_1.GetComponent<SpriteRenderer>().sprite = m_detail[detail_code];
        }
        m_shapeSelected++;
    }

    public void OnColorSelect(int color_code)
    {
        switch (color_code)
        {
            case 0:
                gameObject.GetComponent<SpriteRenderer>().color = red;
                break;
            case 1:
                gameObject.GetComponent<SpriteRenderer>().color = yellow;
                break;
            case 2:
                gameObject.GetComponent<SpriteRenderer>().color = blue;
                break;
            case 3:
                gameObject.GetComponent<SpriteRenderer>().color = brown;
                break;
            case 4:
                gameObject.GetComponent<SpriteRenderer>().color = purple;
                break;
            case 5:
                gameObject.GetComponent<SpriteRenderer>().color = orange;
                break;
            case 6:
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case 7:
                gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                break;
            case 8:
                gameObject.GetComponent<SpriteRenderer>().color = green;
                break;
            default:
                break;
        }
    }
    public void OnShapeSelect(int shape_code)
    {
        switch (shape_code)
        {
            case 0:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_shape[0];
                break;
            case 1:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_shape[1];
                break;
            case 2:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_shape[2];
                break;
            case 3:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_shape[3];
                break;
            case 4:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_shape[4];
                break;
            case 5:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_shape[5];
                break;
            case 6:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_shape[6];
                break;
            case 7:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_shape[7];
                break;
            default: break;
        }
    }
    public void OnQuatityTopSelect(int quantity_code)
    {
        switch (quantity_code)
        {
            case 0:
                m_quantity_obj_top.GetComponent<SpriteRenderer>().sprite = m_quantity_top[0];
                break;
            case 1:
                m_quantity_obj_top.GetComponent<SpriteRenderer>().sprite = m_quantity_top[1];
                break;
            case 2:
                m_quantity_obj_top.GetComponent<SpriteRenderer>().sprite = m_quantity_top[2];
                break;
            case 3:
                m_quantity_obj_top.GetComponent<SpriteRenderer>().sprite = m_quantity_top[3];
                break;
            case 4:
                m_quantity_obj_top.GetComponent<SpriteRenderer>().sprite = m_quantity_top[4];
                break;
            case 5:
                m_quantity_obj_top.GetComponent<SpriteRenderer>().sprite = m_quantity_top[5];
                break;
            case 6:
                m_quantity_obj_top.GetComponent<SpriteRenderer>().sprite = m_quantity_top[6];
                break;
            case 7:
                m_quantity_obj_top.GetComponent<SpriteRenderer>().sprite = m_quantity_top[7];
                break;
            case 8:
                m_quantity_obj_top.GetComponent<SpriteRenderer>().sprite = m_quantity_top[8];
                break;
            case 9:
                m_quantity_obj_top.GetComponent<SpriteRenderer>().sprite = m_empty_sprite;
                break;
            default: break;
        }
    }

    public void OnQuatityBottomSelect(int quantity_code)
    {
        switch (quantity_code)
        {
            case 0:
                m_quantity_obj_bottom.GetComponent<SpriteRenderer>().sprite = m_quantity_bottom[0];
                break;
            case 1:
                m_quantity_obj_bottom.GetComponent<SpriteRenderer>().sprite = m_quantity_bottom[1];
                break;
            case 2:
                m_quantity_obj_bottom.GetComponent<SpriteRenderer>().sprite = m_quantity_bottom[2];
                break;
            case 3:
                m_quantity_obj_bottom.GetComponent<SpriteRenderer>().sprite = m_empty_sprite;
                break;
            default: break;
        }
    }

    public void OnTextFieldSelect(int text_code)
    {
        switch (text_code)
        {
            case 0:
                m_2_field.SetActive(false);
                m_1_bottom.SetActive(false);
                m_1_center.SetActive(false);
                m_2_field_input_0.gameObject.SetActive(false);
                m_2_field_input_1.gameObject.SetActive(false);
                m_1_center_text.gameObject.SetActive(false);
                m_1_bottom_text.gameObject.SetActive(false);

                m_4_field.SetActive(true);
                m_4_field_input_0.gameObject.SetActive(true);
                m_4_field_input_1.gameObject.SetActive(true);
                m_4_field_input_2.gameObject.SetActive(true);
                m_4_field_input_3.gameObject.SetActive(true);
                break;
            case 1:
                m_1_bottom.SetActive(false);
                m_1_center.SetActive(false);
                m_4_field.SetActive(false);
                m_1_center_text.gameObject.SetActive(false);
                m_1_bottom_text.gameObject.SetActive(false);
                m_4_field_input_0.gameObject.SetActive(false);
                m_4_field_input_1.gameObject.SetActive(false);
                m_4_field_input_2.gameObject.SetActive(false);
                m_4_field_input_3.gameObject.SetActive(false);

                m_2_field.SetActive(true);
                m_2_field_input_0.gameObject.SetActive(true);
                m_2_field_input_1.gameObject.SetActive(true);
                break;
            case 2:
                m_4_field.SetActive(false);
                m_2_field.SetActive(false);
                m_1_bottom.SetActive(false);
                m_1_bottom_text.gameObject.SetActive(false);
                m_4_field_input_0.gameObject.SetActive(false);
                m_4_field_input_1.gameObject.SetActive(false);
                m_4_field_input_2.gameObject.SetActive(false);
                m_4_field_input_3.gameObject.SetActive(false);
                m_2_field_input_0.gameObject.SetActive(false);
                m_2_field_input_1.gameObject.SetActive(false);

                m_1_center.SetActive(true);
                m_1_center_text.gameObject.SetActive(true);
                break;
            case 3:
                m_4_field.SetActive(false);
                m_2_field.SetActive(false);
                m_1_center.SetActive(false);
                m_4_field_input_0.gameObject.SetActive(false);
                m_4_field_input_1.gameObject.SetActive(false);
                m_4_field_input_2.gameObject.SetActive(false);
                m_4_field_input_3.gameObject.SetActive(false);
                m_2_field_input_0.gameObject.SetActive(false);
                m_2_field_input_1.gameObject.SetActive(false);
                m_1_center_text.gameObject.SetActive(false);

                m_1_bottom.SetActive(true);
                m_1_bottom_text.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
    public void OnSaveChanges()
    {
        m_player.GetComponent<InteractionManager>().m_custom_TG = null;
        m_side_menu.SetActive(false);
        m_save_button.SetActive(false);
        m_delete_button.SetActive(false);
        m_display_text.gameObject.SetActive(true);
        m_2_field_input_0.gameObject.SetActive(false);
        m_2_field_input_1.gameObject.SetActive(false);
        m_4_field_input_0.gameObject.SetActive(false);
        m_4_field_input_1.gameObject.SetActive(false);
        m_4_field_input_2.gameObject.SetActive(false);
        m_4_field_input_3.gameObject.SetActive(false);
        m_1_center_text.gameObject.SetActive(false);
        m_1_bottom_text.gameObject.SetActive(false);
    }
    public void OnDeleteObject()
    {
        m_player.GetComponent<InteractionManager>().m_custom_TG = null;
        m_side_menu.SetActive(false);
        Destroy(this.gameObject);
    }
    #endregion
    #region helpers
    #endregion
}
