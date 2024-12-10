using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThreeDCameraController
{
    public class ThreeDCameraController : MonoBehaviour
    {
        #region variables
        //target transform-> player
        //player moves -> controlled by touch input
        public Transform m_target;
        public float m_height = 20f;
        [SerializeField]
        private float m_distance = 50f;
        [SerializeField]
        private float m_angle = 45f;
        [SerializeField]
        private float m_smooth_speed = 2f;
        [SerializeField]
        private Vector3 refVelocity;
        #endregion

        #region main methods
        // Start is called before the first frame update
        void Start()
        {
            //CameraSetting();
            OnCameraFollow();
        }

        // Update is called once per frame
        void Update()
        {
            //CameraSetting();
            OnCameraFollow();
        }
        #endregion
        #region helper methods
        private void OnCameraFollow()
        {
            if (!m_target)
            {
                Debug.Log("Camera can not follow: target tranform is empty");
            }
            else
            {
                Vector3 world_pos = ((Vector3.forward + Vector3.right).normalized * m_distance) + (Vector3.up * m_height);
                //Build rotated vector
                Vector3 rotated_vector = Quaternion.AngleAxis(m_angle, Vector3.up) * world_pos;
                //Move position
                Vector3 flat_target_pos = m_target.position;
                flat_target_pos.y = 0f;
                Vector3 final_pos = flat_target_pos + rotated_vector;
                transform.position = Vector3.SmoothDamp(transform.position, final_pos, ref refVelocity, m_smooth_speed);

                float rotation_fac = m_target.localRotation.eulerAngles.y;
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(20f,rotation_fac, 0f), m_smooth_speed * Time.deltaTime);

            }
        }
        protected virtual void CameraSetting()
        {
            if (!m_target)
            {
                Debug.Log("Camera can not follow: target tranform is empty");
            }
            else
            {
                //Build world position vector
                Vector3 world_pos = ((Vector3.forward + Vector3.right).normalized * m_distance) + (Vector3.up * m_height);
                //Build rotated vector
                Vector3 rotated_vector = Quaternion.AngleAxis(m_angle, Vector3.up) * world_pos;
                //Move position
                Vector3 flat_target_pos = m_target.position;
                flat_target_pos.y = 0f;
                Vector3 final_pos = flat_target_pos + rotated_vector;
                transform.position = Vector3.SmoothDamp(transform.position, final_pos, ref refVelocity, m_smooth_speed);
                transform.LookAt(flat_target_pos);
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            if (!m_target)
            {
                Debug.Log("Camera can't follow: assign target tranform");
                return;
            }
            else
            {
                Gizmos.DrawLine(transform.position, m_target.position);
                Gizmos.DrawSphere(m_target.position, 1.5f);
                Gizmos.DrawSphere(transform.position, 1.5f);
            }
        }
        #endregion
    }
}
