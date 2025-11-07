using UnityEngine;

namespace Neuromorph.Cameras
{
public class LookAtCamera : MonoBehaviour
{
    [SerializeField] LookType _lookType = LookType.LookAtCamera;

    enum LookType {LookAtCamera, CameraForward}

    Transform _camera;

    void Awake()
    {
        _camera = Camera.main?.transform;
    }

    void LateUpdate()
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
}}
