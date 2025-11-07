using UnityEngine;

namespace Neuromorph
{
    public class CameraManager : MonoBehaviour
    {
        static Camera MainCamera;

        protected void Awake()
        {
            MainCamera = Camera.main;
        }
        
        public static Vector3 GetMousePositionInWorld() =>
            MainCamera.ScreenToWorldPoint(Input.mousePosition);
        
        public static bool IsInsideWorldUI()
        {
            Vector3 mousePos = MainCamera.ScreenToViewportPoint(Input.mousePosition);
            return mousePos.x is >= 0f and <= 1f;
        }
    }
}
