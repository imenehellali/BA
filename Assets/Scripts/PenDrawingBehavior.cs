using Mapbox.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenDrawingBehavior : MonoBehaviour
{
    #region Variables
    public InteractionManager m_player;
    public Color m_lineColor;
    public Color m_fillColor;
    public int m_lineSize;
    

    //Start drawing permitted
    public Image m_colorSelected;
    public Image m_sizeSelected;
    public Image m_modeSelected;
    public int m_drawMode = 0;

    //Display Information
    public Sprite m_contourMode_Sprite;
    public Sprite m_fillMode_Sprite;
    public Sprite[] m_sizeSprite;
    public Sprite m_emptySprite;

    public GameObject m_PenMenu;
    //Colors Line
    private Color red = new Color32(255, 0, 0, 255);
    private Color yellow = new Color32(255, 255, 0, 255);
    private Color blue = new Color32(0, 0, 255, 255);
    private Color purple = new Color32(255, 0, 255, 255);
    private Color orange = new Color32(255, 160, 0, 255);
    private Color brawn = new Color32(153,102,51,255);
    private Color green = new Color32(0, 128, 0, 255);

    //Colors Fill
    private Color _red = new Color32(255, 0, 0, 55);
    private Color _yellow = new Color32(255, 255, 0, 55);
    private Color _blue = new Color32(0, 0, 255, 55);
    private Color _purple = new Color32(255, 0, 255, 55);
    private Color _orange = new Color32(255, 160, 0, 55);
    private Color _white = new Color32(0,0,0,55);
    private Color _black = new Color32(255, 255, 255, 55);
    private Color _green = new Color32(0, 128, 0, 55);
    private Color _brawn = new Color32(153, 102, 51, 55);

    //
    public int _canDrawNow=0;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        m_lineColor = red;
        m_lineSize = 5;
        m_PenMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Custom Pen selection 
    public void SetDrawWithFinger()
    {
        if (m_player.m_drawLine)
        {
            m_player.m_shadingContainer_temp = null;
            m_player.m_drawLine = false;
            m_PenMenu.SetActive(false);
            _canDrawNow = 0;
            m_colorSelected.sprite = m_emptySprite;
            m_modeSelected.sprite = m_emptySprite;
            m_sizeSelected.sprite = m_emptySprite;

            m_colorSelected.color = Color.gray;
            m_modeSelected.color = Color.gray;
            m_sizeSelected.color = Color.gray;
        }
        else
        {
            m_PenMenu.SetActive(true);
            m_player.m_drawLine = true;
        }

    }

    public void OnColorSelect(int code)
    {
        switch (code)
        {
            case 0:
                m_lineColor=Color.white;
                break;
            case 1:
                m_lineColor = orange;
                break;
            case 2:
                m_lineColor = purple;
                break;
            case 3:
                m_lineColor = red;
                break;
            case 4:
                m_lineColor = yellow;
                break;
            case 5:
                m_lineColor = blue;
                break;
            case 6:
                m_lineColor = Color.black;
                break;
            case 7:
                m_lineColor = brawn;
                break;
            case 8:
                m_lineColor = green;
                break;
            default:
                break;
        }
        m_colorSelected.sprite = m_fillMode_Sprite;
        m_colorSelected.color = m_lineColor;
    }

    public void OnColorSelectFill(int code)
    {
        _canDrawNow += 1;
        switch (code)
        {
            case 0:
                m_fillColor = _white;
                break;
            case 1:
                m_fillColor = _orange;
                break;
            case 2:
                m_fillColor = _purple;
                break;
            case 3:
                m_fillColor = _red;
                break;
            case 4:
                m_fillColor = _yellow;
                break;
            case 5:
                m_fillColor = _blue;
                break;
            case 6:
                m_fillColor = _black;
                break;
            case 7:
                m_fillColor = _brawn;
                break;
            case 8:
                m_fillColor = _green;
                break;
            default:
                break;
        }
    }
    public void OnDrawModeSelect(int code)
    {
        _canDrawNow += 1;
        switch (code)
        {
            case 0:
                m_drawMode = 0;
                m_modeSelected.sprite = m_contourMode_Sprite;
                m_modeSelected.color = Color.white;
                break;
            case 1:
                m_drawMode = 1;
                m_modeSelected.sprite = m_fillMode_Sprite;
                m_modeSelected.color = new Color32(255, 0, 0, 100);
                break;
            default:
                m_drawMode = 0;
                break;
        }
    }
    public void OnSizeSelect(int code)
    {
        _canDrawNow += 1;
        switch (code)
        {
            case 0:
                m_lineSize = 2;
                m_sizeSelected.sprite = m_sizeSprite[0];
                break;
            case 1:
                m_lineSize = 8;
                m_sizeSelected.sprite = m_sizeSprite[1];
                break;
            case 2:
                m_lineSize = 10;
                m_sizeSelected.sprite = m_sizeSprite[2];
                break;
            case 3:
                m_lineSize = 15;
                m_sizeSelected.sprite = m_sizeSprite[3];
                break;
            default:
                break;
        }
    }
    #endregion
}
