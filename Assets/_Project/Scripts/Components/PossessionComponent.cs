using UnityEngine;

namespace Neuromorph.Components
{
    public class PossessionComponent: BaseComponent
    {
        [SerializeField] private float _possessDistance = 3f;
        private CharacterController _col;
        private void Awake()
        {
            _col = GetComponent<CharacterController>();
        }
        
        public bool TryPossess(out Puppet puppetToPossess)
        {
            bool hit = Physics.Raycast(_col.bounds.center, transform.forward,
                out RaycastHit hitInfo, _possessDistance);
            
            if (hit && hitInfo.collider.TryGetComponent(out Puppet puppet)) {
                puppetToPossess = puppet;
                return true;
            }
            puppetToPossess = null;
            return false;
        }

        public void UnPossess(Puppet puppetToUnPossess)
        {
            puppetToUnPossess.Movement.InputDir = Vector2.zero;
        }
    }
}