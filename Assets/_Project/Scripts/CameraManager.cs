using Cinemachine;
using Neuromorph.Utilities;
using UnityEngine;

namespace Neuromorph
{
    public class CameraManager : Singleton<CameraManager>
    {
        [SerializeField] private CinemachineFreeLook _vCameraWorld;
        [SerializeField] private CinemachineVirtualCamera _vCameraBrain;

        public void FollowPuppet(Puppet puppet) {
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
    }
}
