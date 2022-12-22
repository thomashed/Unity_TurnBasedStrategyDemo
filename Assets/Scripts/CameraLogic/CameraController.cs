using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace CoolBeans.CameraLogic
{
    public class CameraController : MonoBehaviour
    {
        private float moveSpeed = 10f;
        private float rotationSpeed = 100f;
        private float zoomSpeed = 25f;

        private const float MAX_FOLLOW_OFFSET = 30f;
        private const float MIN_FOLLOW_OFFSET = 0.2f;


        [SerializeField] private CinemachineVirtualCamera vCam = null;
        private CinemachineTransposer transposer;
        private Vector3 targetFollowOffset;

        private void Start()
        {
            this.transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
        }

        private void Update()
        {
            CheckCameraPosition();
            CheckCameraRotation();
            CheckCameraZoom();
        }


        private void CheckCameraPosition()
        {
            Vector3 inputMoveDirection = new Vector3(0, 0, 0);

            if (Input.GetKey(KeyCode.W))
            {
                inputMoveDirection.z = +1f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                inputMoveDirection.z = -1f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                inputMoveDirection.x = -1f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                inputMoveDirection.x = +1f;
            }

            Vector3 moveVector = transform.forward * inputMoveDirection.z + transform.right * inputMoveDirection.x;
            transform.position += moveVector * moveSpeed * Time.deltaTime;
        }

        private void CheckCameraRotation()
        {
            Vector3 rotataionVector = new Vector3();

            if (Input.GetKey(KeyCode.Q))
            {
                rotataionVector.y = 1f;
            }
            if (Input.GetKey(KeyCode.E))
            {
                rotataionVector.y = -1f;
            }

            transform.eulerAngles += rotataionVector * rotationSpeed * Time.deltaTime;
        }

        private void CheckCameraZoom()
        {
            targetFollowOffset = transposer.m_FollowOffset;
            var mouseScroll = Input.mouseScrollDelta;
            var zoomAmount = 1f;

            if (mouseScroll.y > 0)
            {
                targetFollowOffset.y += zoomAmount;
            }

            if (mouseScroll.y < 0)
            {
                targetFollowOffset.y -= zoomAmount;
            }

            targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_OFFSET, MAX_FOLLOW_OFFSET);
            transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
        }

    }
}


