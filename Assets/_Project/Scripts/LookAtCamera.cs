using UnityEngine;

namespace Neuromorph
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] private LookType _lookType = LookType.LookAtCamera;
        private enum LookType {LookAtCamera, CameraForward}
        private Transform _camera;

        private void Start()
        {
            _camera = CameraManager.MainCamera.transform;
        }

        private void LateUpdate()
        {
            switch (_lookType)
            {
                case LookType.LookAtCamera:
                    transform.LookAt(_camera, Vector3.up);
                    break;
                case LookType.CameraForward:
                    transform.forward
                        = _camera.forward;
                    break;
                default: break;
            }
        }
    }
}
