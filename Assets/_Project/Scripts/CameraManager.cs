using Cinemachine;
using Neuromorph.Utilities;
using UnityEngine;

namespace Neuromorph
{
    public class CameraManager : Singleton<CameraManager>
    {
        public static Camera MainCamera { get; private set; }
        [SerializeField] private Camera _cameraBrain;
        [SerializeField] private CinemachineFreeLook _vCameraWorld;
        [SerializeField] private CinemachineVirtualCamera _vCameraBrain;

        private void Start()
        {
            MainCamera = Camera.main;
        }

        public void FollowPuppet(Puppet puppet)
        {
            _vCameraWorld.Follow = puppet.transform;
            _vCameraWorld.LookAt = puppet.transform;
        }
        /*private void OnEnable() => 
            EventBus.OnPlayerCreated += SetupCamera;
        private void OnDisable() =>
            EventBus.OnPlayerCreated -= SetupCamera;

        private void SetupCamera(Entity targetEntity){
            _vcam.Follow = targetEntity.transform;
            EventBus.OnPlayerCreated -= SetupCamera;
        }*/
        
        public Vector3 GetMousePositionInBrainSpace()
        {
            Vector3 mousePosition = _cameraBrain.ScreenToWorldPoint(Input.mousePosition);
            return new Vector3(mousePosition.x, mousePosition.y, 0f);
        }
        
        public static Vector3 GetMousePositionInWorldSpace()
        {
            Vector3 mousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            return new Vector3(mousePosition.x, mousePosition.y, 0f);
        }
    }
}
