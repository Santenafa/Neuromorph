using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace Neuromorph
{
    public class Puppet: MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private LayerMask _clickableLayer;
        [SerializeField] private bool _isInstantRotation;
        [SerializeField] private float _lookRotationSpeed = 50f;
        
        private Interactable _followTarget;
        private PuppetState _state = PuppetState.Idle;
        private enum PuppetState { Talking, Idle, Moving}

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
        }

        private void Update()
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

        
        private void Moving()
        {
            FaceTarget();
            
            if (_agent.remainingDistance > _agent.stoppingDistance) return;
            
            _state = PuppetState.Idle;
            
            if (!_followTarget) return;
            
            transform.DOLookAt(_followTarget.transform.position, 0.1f, AxisConstraint.Y);
            _followTarget.Interact();
            _followTarget = null;
        }

        private void FaceTarget()
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
            
            bool isHit = Physics.Raycast(CameraManager.MainCamera.ScreenPointToRay(Input.mousePosition),
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
            if (value)
                _state = PuppetState.Idle;
            else {
                _agent.ResetPath();
                _followTarget = null;
                _state = PuppetState.Talking;
            }
        }

        private void WalkToTarget(Interactable target)
        {
            _state = PuppetState.Moving;
            _followTarget = target;
            _agent.SetDestination(target.InteractPoint);
        }

        private void WalkToPoint(Vector3 point)
        {
            _state = PuppetState.Moving;
            _followTarget = null;
            _agent.SetDestination(point);
        }
    }
}