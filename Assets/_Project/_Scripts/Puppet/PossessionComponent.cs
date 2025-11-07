using UnityEngine;

namespace Neuromorph
{
public class PossessionComponent: MonoBehaviour
{
    [SerializeField] float _possessDistance = 3f;
    CharacterController _col;

    void Awake()
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

    public static void UnPossess(Puppet puppetToUnPossess)
    {
        //puppetToUnPossess.SetCanMove(false);
    }
}}