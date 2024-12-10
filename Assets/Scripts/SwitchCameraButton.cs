using UnityEngine;
using UnityEngine.UI;
using TopDownCameraController;
public class SwitchCameraButton : MonoBehaviour
{
    #region variables
    public GameObject m_3D_Camera;
    public GameObject m_2D_Camera;
    public Sprite enable_3D;
    public Sprite disable_3D;
    #endregion
    public void SwitchCamera()
    {
        //Switch to TopDown view
        if (m_3D_Camera.activeSelf)
        {
            m_3D_Camera.SetActive(false);
            m_2D_Camera.SetActive(true);
            gameObject.GetComponent<Button>().image.sprite = disable_3D;
        }
        //Switch to 3D view
        else if (m_2D_Camera.activeSelf)
        {
            m_2D_Camera.SetActive(false);
            m_3D_Camera.SetActive(true);
            gameObject.GetComponent<Button>().image.sprite = enable_3D;
        }
    }
}
