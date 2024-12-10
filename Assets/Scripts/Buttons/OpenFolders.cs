using Mapbox.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenFolders : MonoBehaviour
{
    #region variables
    public GameObject m_FW_folder;
    public GameObject m_BL_folder;
    public GameObject m_WT_folder;
    public GameObject m_VET_folder;
    public GameObject m_FÜHR_folder;
    public GameObject m_LUK_folder;
    public GameObject m_CARE_folder;
    public GameObject m_GEFAHR_folder;

    public GameObject m_taktische_folder;
    public GameObject m_backButton;
    public Image m_tacticalMenu_Button;

    public Sprite m_transparent;
    #endregion
    #region maim methods
    private void Start()
    {
        m_BL_folder.SetActive(false);
        m_FW_folder.SetActive(false);
        m_WT_folder.SetActive(false);
        m_VET_folder.SetActive(false);
        m_FÜHR_folder.SetActive(false);
        m_LUK_folder.SetActive(false);
        m_CARE_folder.SetActive(false);
        m_GEFAHR_folder.SetActive(false);
        m_taktische_folder.SetActive(false);
    }
    #endregion
    #region Controle method
    public void OnClick(int code)
    {
        switch (code)
        {
            case 0:
                m_backButton.SetActive(true);
                m_FW_folder.SetActive(true);
                m_BL_folder.SetActive(false);
                m_WT_folder.SetActive(false);
                m_VET_folder.SetActive(false);
                m_FÜHR_folder.SetActive(false);
                m_LUK_folder.SetActive(false);
                m_CARE_folder.SetActive(false);
                m_GEFAHR_folder.SetActive(false);
                break;
            case 1:
                m_backButton.SetActive(true);
                m_BL_folder.SetActive(true);
                m_FW_folder.SetActive(false);
                m_WT_folder.SetActive(false);
                m_VET_folder.SetActive(false);
                m_FÜHR_folder.SetActive(false);
                m_LUK_folder.SetActive(false);
                m_CARE_folder.SetActive(false);
                m_GEFAHR_folder.SetActive(false);
                break;
            case 2:
                m_backButton.SetActive(true);
                m_FW_folder.SetActive(false);
                m_BL_folder.SetActive(false);
                m_WT_folder.SetActive(true);
                m_VET_folder.SetActive(false);
                m_FÜHR_folder.SetActive(false);
                m_LUK_folder.SetActive(false);
                m_CARE_folder.SetActive(false);
                m_GEFAHR_folder.SetActive(false);
                break;
            case 3:
                m_backButton.SetActive(true);
                m_BL_folder.SetActive(false);
                m_FW_folder.SetActive(false);
                m_WT_folder.SetActive(false);
                m_VET_folder.SetActive(true);
                m_FÜHR_folder.SetActive(false);
                m_LUK_folder.SetActive(false);
                m_CARE_folder.SetActive(false);
                m_GEFAHR_folder.SetActive(false);
                break;
            case 4:
                m_backButton.SetActive(true);
                m_FW_folder.SetActive(false);
                m_BL_folder.SetActive(false);
                m_WT_folder.SetActive(false);
                m_VET_folder.SetActive(false);
                m_FÜHR_folder.SetActive(false);
                m_LUK_folder.SetActive(false);
                m_CARE_folder.SetActive(true);
                m_GEFAHR_folder.SetActive(false);
                break;
            case 5:
                m_backButton.SetActive(true);
                m_BL_folder.SetActive(false);
                m_FW_folder.SetActive(false);
                m_WT_folder.SetActive(false);
                m_VET_folder.SetActive(false);
                m_FÜHR_folder.SetActive(true);
                m_LUK_folder.SetActive(false);
                m_CARE_folder.SetActive(false);
                m_GEFAHR_folder.SetActive(false);
                break;
            case 6:
                m_backButton.SetActive(true);
                m_FW_folder.SetActive(false);
                m_BL_folder.SetActive(false);
                m_WT_folder.SetActive(false);
                m_VET_folder.SetActive(false);
                m_FÜHR_folder.SetActive(false);
                m_LUK_folder.SetActive(true);
                m_CARE_folder.SetActive(false);
                m_GEFAHR_folder.SetActive(false);
                break;
            case 7:
                m_backButton.SetActive(true);
                m_BL_folder.SetActive(false);
                m_FW_folder.SetActive(false);
                m_WT_folder.SetActive(false);
                m_VET_folder.SetActive(false);
                m_FÜHR_folder.SetActive(false);
                m_LUK_folder.SetActive(false);
                m_CARE_folder.SetActive(false);
                m_GEFAHR_folder.SetActive(true);
                break;
            case 10:
                GetComponent<InteractionManager>().m_placingIcons = true;
                m_backButton.SetActive(true);
                m_taktische_folder.SetActive(true);
                m_tacticalMenu_Button.sprite = m_transparent;
                m_BL_folder.SetActive(false);
                m_FW_folder.SetActive(false);
                m_WT_folder.SetActive(false);
                m_VET_folder.SetActive(false);
                m_FÜHR_folder.SetActive(false);
                m_LUK_folder.SetActive(false);
                m_CARE_folder.SetActive(false);
                m_GEFAHR_folder.SetActive(false);
                break;

            default: break;
        }
    }
    #endregion
}
