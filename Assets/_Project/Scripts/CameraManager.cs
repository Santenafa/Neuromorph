using Cinemachine;
using Neuromorph.Utilities;
using UnityEngine;

namespace Neuromorph
{
    public class CameraManager : Singleton<CameraManager>
    {
        [SerializeField] private CinemachineVirtualCamera _vcam;

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
