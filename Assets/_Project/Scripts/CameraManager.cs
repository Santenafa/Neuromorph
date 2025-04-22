using System.Collections.Generic;
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
        [SerializeField] private EdgeCollider2D _edgeCollider;

        protected override void Awake()
        {
            base.Awake();
            MainCamera = Camera.main;
        }

        private void Start()
        {
            ChangeEdgeCollider();
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
        
        private void ChangeEdgeCollider()
        {
            float height = _cameraBrain.orthographicSize;
            float width = _cameraBrain.orthographicSize * _cameraBrain.aspect;
            
            List<Vector2> screenEdges = new() {
                new Vector2(-width, -height), new Vector2(-width, height),
                new Vector2(width, height), new Vector2(width, -height),
                new Vector2(-width, -height)
            };
            _edgeCollider.SetPoints(screenEdges);
        }
        
        public bool IsInsideBrainUI(){
            Vector3 mousePos = _cameraBrain.ScreenToViewportPoint(Input.mousePosition);
            return mousePos.x <= 1f;
        }
    }
}
