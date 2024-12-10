using Mapbox.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkedAreaButtonBrhavior : MonoBehaviour
{
    #region variables
    public GameObject m_area;
    public GameObject m_contour;
    public GameObject m_content;
    public GameObject m_player;
    public Camera m_camera_2D;
    public GameObject m_back_button;
    public GameObject m_shading;

    private string m_text = "";
    private bool m_txt_isOpen = false;
    #endregion
    #region main methods
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.m_txt_isOpen)
        {
            m_text = m_player.GetComponent<InteractionManager>().m_south.text;

            m_player.GetComponent<InteractionManager>().m_west.text = this.m_text;
            m_player.GetComponent<InteractionManager>().m_ost.text = this.m_text;
            m_player.GetComponent<InteractionManager>().m_north.text = this.m_text;
        }
    }
    #endregion
    #region helper methods
    public void OnButtonClicked()
    {
        m_player.transform.position = m_area.transform.position;

        m_camera_2D.orthographicSize = 100f;
        m_content.SetActive(false);
        m_back_button.SetActive(false);
    }
    public void OnDeleteComponent()
    {
        bool rem=m_player.GetComponent<InteractionManager>().m_incident_areas_List.Remove(m_area);
        Debug.Log("removed area from list:   " + rem);
        rem= m_player.GetComponent<InteractionManager>().m_incident_line_List.Remove(m_contour);
        Debug.Log("removed contour from list:   " + rem);

        Destroy(m_area.gameObject);
        Destroy(m_contour.gameObject);
        Destroy(gameObject);
    }

    public void OnDisplayInformation()
    {
        if (!m_player.GetComponent<InteractionManager>().m_west.gameObject.activeSelf)
        {
            this.m_txt_isOpen = true;
            m_player.GetComponent<InteractionManager>().m_west.text = m_shading.GetComponent < TextNotesForArea >().myText;
            m_player.GetComponent<InteractionManager>().m_ost.text = m_shading.GetComponent < TextNotesForArea > ().myText;
            m_player.GetComponent<InteractionManager>().m_north.text = m_shading.GetComponent<TextNotesForArea>().myText;
            m_player.GetComponent<InteractionManager>().m_south.text = m_shading.GetComponent<TextNotesForArea>().myText;

            m_player.GetComponent<InteractionManager>().m_west.gameObject.SetActive(true);
            m_player.GetComponent<InteractionManager>().m_ost.gameObject.SetActive(true);
            m_player.GetComponent<InteractionManager>().m_north.gameObject.SetActive(true);
            m_player.GetComponent<InteractionManager>().m_south.gameObject.SetActive(true);
        }
        else
        {
            m_shading.GetComponent<TextNotesForArea>().myText = m_text;
            this.m_txt_isOpen = false;
            m_player.GetComponent<InteractionManager>().m_west.gameObject.SetActive(false);
            m_player.GetComponent<InteractionManager>().m_ost.gameObject.SetActive(false);
            m_player.GetComponent<InteractionManager>().m_north.gameObject.SetActive(false);
            m_player.GetComponent<InteractionManager>().m_south.gameObject.SetActive(false);
        }
        
    }

    public void OnModifyShading()
    {
        Debug.Log("To be implemented either only shade or only controue or both");
    }
    #endregion
}
