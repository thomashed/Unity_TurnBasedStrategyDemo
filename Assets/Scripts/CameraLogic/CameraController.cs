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
            Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();

            Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;
            transform.position += moveVector * moveSpeed * Time.deltaTime;
        }

        private void CheckCameraRotation()
        {
            Vector3 rotataionVector = new Vector3();
            rotataionVector.y = InputManager.Instance.GetCameraRotateAmount();
            transform.eulerAngles += rotataionVector * rotationSpeed * Time.deltaTime;
        }

        private void CheckCameraZoom()
        {
            float zoomIncreaseAmount = 1f;
            targetFollowOffset = transposer.m_FollowOffset;
            targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * zoomIncreaseAmount;

            targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_OFFSET, MAX_FOLLOW_OFFSET);
            transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
        }

    }
}


