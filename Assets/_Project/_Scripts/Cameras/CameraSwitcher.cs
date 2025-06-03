using Cinemachine;
using UnityEngine;

namespace Neuromorph.Cameras
{
    [RequireComponent(typeof(Collider))]
    public class CameraSwitcher : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _vcam;
        [SerializeField] private CameraState _camState = CameraState.Inactive;
        private enum CameraState{Inactive = 0, Active = 1}
        private void Start()
        {
            _vcam.Priority = (int)_camState;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (GameManager.IsPlayer(other)) _vcam.Priority = (int)CameraState.Active;
        }
        private void OnTriggerExit(Collider other)
        {
            if (GameManager.IsPlayer(other)) _vcam.Priority = (int)CameraState.Inactive;
        }
    }
}
