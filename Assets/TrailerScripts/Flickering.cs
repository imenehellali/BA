using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flickering : MonoBehaviour
{
    public Image[] images;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Blinking());
    }

    private IEnumerator Blinking()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);
            foreach (Image m_image in images)
            {
                m_image.enabled = !m_image.enabled;
            }
        }
       
    }
}
