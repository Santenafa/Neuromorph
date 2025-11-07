using Cinemachine;
using UnityEngine;

namespace Neuromorph.Cameras
{
[RequireComponent(typeof(Collider))]
public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _vcam;
    [SerializeField] CameraState _camState = CameraState.Inactive;

    enum CameraState{Inactive = 0, Active = 1}

    void Start()
    {
        _vcam.Priority = (int)_camState;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Puppet>()) _vcam.Priority = (int)CameraState.Active;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Puppet>()) _vcam.Priority = (int)CameraState.Inactive;
    }
}}
