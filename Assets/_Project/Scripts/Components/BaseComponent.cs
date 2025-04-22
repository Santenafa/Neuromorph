using UnityEngine;

namespace Neuromorph.Components
{
    [RequireComponent(typeof(Puppet))]
    public abstract class BaseComponent: MonoBehaviour
    {
        protected Puppet _puppet;
        protected virtual void OnEnable() => _puppet = GetComponent<Puppet>();
    }
}