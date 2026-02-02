using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace Neuromorph
{
public class Puppet: MonoBehaviour
{
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] LayerMask _clickableLayer;
    [SerializeField] bool _isInstantRotation;
    [SerializeField] float _lookRotationSpeed = 50f;

    Camera _camera;
    Interactable _followTarget;
    PuppetState _state = PuppetState.Idle;

    enum PuppetState { Talking, Idle, Moving}

    void Awake()
    {
        _camera = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
    }

    void Update()
    {
        switch (_state)
        {
            case PuppetState.Moving:
                Moving();
                break;
            case PuppetState.Talking:
            case PuppetState.Idle:
            default: break;
        }
    }


    void Moving()
    {
        FaceTarget();
        
        if (_agent.remainingDistance > _agent.stoppingDistance) return;
        
        _state = PuppetState.Idle;
        
        if (!_followTarget) return;
        
        transform.DOLookAt(_followTarget.transform.position, 0.1f, AxisConstraint.Y);
        _followTarget.Interact();
        _followTarget = null;
    }

    void FaceTarget()
    {
        Vector3 direction = _agent.steeringTarget - transform.position;
        if (direction == Vector3.zero) return;
        
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        
        transform.rotation = _isInstantRotation ?
            lookRotation : Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _lookRotationSpeed);
    }

    public void ClickToMove()
    {
        if (_state == PuppetState.Talking || EventSystem.current.IsPointerOverGameObject()) return;
        
        bool isHit = Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition),
            out RaycastHit hit, 100f, _clickableLayer);

        switch (isHit)
        {
            case true when hit.transform.TryGetComponent(out Interactable target):
                WalkToTarget(target); break;
            case true:
                WalkToPoint(hit.point); break;
            default:
                _state = PuppetState.Idle; break;
        }
    }

    public void SetCanMove(bool value)
    {
        if (value) {
            _state = PuppetState.Idle;
        } else {
            _agent.ResetPath();
            _followTarget = null;
            _state = PuppetState.Talking;
        }
    }

    void WalkToTarget(Interactable target)
    {
        _state = PuppetState.Moving;
        _followTarget = target;
        _agent.SetDestination(target.InteractPoint);
    }

    void WalkToPoint(Vector3 point)
    {
        _state = PuppetState.Moving;
        _followTarget = null;
        _agent.SetDestination(point);
    }
}
}