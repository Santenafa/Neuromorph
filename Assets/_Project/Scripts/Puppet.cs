using UnityEngine;
using UnityEngine.AI;

namespace Neuromorph
{
    public class Puppet: MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private LayerMask _clickableLayer;
        [SerializeField] private bool _isInstantRotation;
        [SerializeField] private float _lookRotationSpeed = 50f;
        private bool _canMove;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
        }

        private void Update()
        {
            if (_canMove) FaceTarget();
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
            if (!_canMove) return;
            
            bool isHit = Physics.Raycast(CameraManager.MainCamera.ScreenPointToRay(Input.mousePosition),
                out RaycastHit hit, 100f, _clickableLayer);
            
            if (isHit) _agent.SetDestination(hit.point);
        }

        public void SetCanMove(bool value)
        {
            if (!value) _agent.ResetPath();
            _canMove = value;
        }
    }
}