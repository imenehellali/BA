using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mapbox.Examples;
public class BackButtonBehavior : MonoBehaviour
{
    #region variables
    public GameObject m_red_folder;
    public GameObject m_yellow_folder;
    public GameObject m_taktische_folder;
    public GameObject m_back_button_area;
    public GameObject m_back_button_TF;
    public Image m_tacticalMenuButton;

    public Sprite m_einsatz;
    #endregion
    #region Controle method
    public void OnClick(int code)
    {
        switch (code)
        {
            case 0:
                m_red_folder.SetActive(false);
                m_yellow_folder.SetActive(false);
                m_taktische_folder.SetActive(false);
                m_tacticalMenuButton.sprite = m_einsatz;
                m_back_button_TF.SetActive(false);
                GetComponent<InteractionManager>().m_placingIcons = false;
                break;
            case 3:
                m_back_button_area.SetActive(false);
                gameObject.GetComponent<InteractionManager>().m_incident_area_content.SetActive(false);
                break;
            default: break;
        }
    }
    #endregion
}
