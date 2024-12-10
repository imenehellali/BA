using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextNotesForArea : MonoBehaviour
{
    public Camera m_2DCamera;
    public Canvas m_canvas;
    public TextMeshProUGUI m_text;
    public string myText = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (m_canvas != null && m_2DCamera != null)
        {
            if (m_2DCamera.orthographicSize < 100)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                m_canvas.gameObject.SetActive(false);

            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().enabled = true;
                m_canvas.gameObject.SetActive(true);
                m_text.text = myText;
            }
        }
    }
}
