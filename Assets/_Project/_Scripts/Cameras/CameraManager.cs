using System.Collections.Generic;
using Cinemachine;
using Neuromorph.Utilities;
using UnityEngine;

namespace Neuromorph
{
    public class CameraManager : Singleton<CameraManager>
    {
        public static Camera MainCamera { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            MainCamera = Camera.main;
        }

        private void Start()
        {
            //ChangeEdgeCollider();
        }
        
        public static Vector3 GetMousePositionInWorld()
        {
            return MainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        
        private void ChangeEdgeCollider()
        {
            /*float height = _cameraBrain.orthographicSize;
            float width = _cameraBrain.orthographicSize * _cameraBrain.aspect;
            
            List<Vector2> screenEdges = new() {
                new Vector2(-width, -height), new Vector2(-width, height),
                new Vector2(width, height), new Vector2(width, -height),
                new Vector2(-width, -height)
            };
            _edgeCollider.SetPoints(screenEdges);*/
        }
        
        public static bool IsInsideWorldUI(){
            Vector3 mousePos = MainCamera.ScreenToViewportPoint(Input.mousePosition);
            return mousePos.x is >= 0f and <= 1f;
        }
    }
}
