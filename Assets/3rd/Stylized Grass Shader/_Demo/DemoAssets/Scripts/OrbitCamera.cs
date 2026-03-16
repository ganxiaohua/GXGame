using System;
using UnityEngine;
using System.Collections;

namespace StylizedGrassDemo
{
    public class OrbitCamera : MonoBehaviour
    {
        [Space]
        public Transform pivot;

        [Space]
        public bool enableMouse = true;
        public float idleRotationSpeed = 0.05f;
        public float lookSmoothSpeed = 5;
        public float moveSmoothSpeed = 5;
        public float scrollSmoothSpeed = 5;

        private Transform cam;
        private float cameraRotSide;
        private float cameraRotUp;
        private float cameraRotSideCur;
        private float cameraRotUpCur;
        private float distance;

        private Vector3 targetPoint;
        
        void Start()
        {
            cam = Camera.main.transform;

            cameraRotSide = transform.eulerAngles.y;
            cameraRotSideCur = transform.eulerAngles.y;
            cameraRotUp = transform.eulerAngles.x;
            cameraRotUpCur = transform.eulerAngles.x;
            distance = -cam.localPosition.z;
        }

        private void LateUpdate()
        {
            if (Input.GetMouseButton(0) && enableMouse)
            {
                cameraRotSide += Input.GetAxis("Mouse X") * 5;
                cameraRotUp -= Input.GetAxis("Mouse Y") * 5;
                
                Cursor.visible = false;
            }
            else
            {
                cameraRotSide += idleRotationSpeed;

                Cursor.visible = true;
            }

            if (Input.GetMouseButton(1) && enableMouse)
            {
                distance *= (1 - 0.1f * Input.GetAxis("Mouse Y"));
            }
            
            if(enableMouse) distance *= (1 - 1 * Input.GetAxis("Mouse ScrollWheel"));
            
        }
        
        private void Update()
        {
 
        }

        private void Apply()
        {
            cameraRotSideCur = Mathf.LerpAngle(cameraRotSideCur, cameraRotSide, 0.02f * lookSmoothSpeed);
            cameraRotUpCur = Mathf.Lerp(cameraRotUpCur, cameraRotUp, 0.02f * lookSmoothSpeed);
            
            transform.position = Vector3.Lerp(transform.position, pivot.position, 0.02f * moveSmoothSpeed);
            transform.rotation = Quaternion.Euler(cameraRotUpCur, cameraRotSideCur, 0);

            float dist = Mathf.Lerp(-cam.transform.localPosition.z, distance, 0.02f * scrollSmoothSpeed);
            cam.localPosition = -Vector3.forward * dist;
        }
        
        void FixedUpdate()
        {
            Apply();

        }
    }
}
