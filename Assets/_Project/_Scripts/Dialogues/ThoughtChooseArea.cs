using UnityEngine;

namespace Neuromorph.Dialogues
{
[RequireComponent(typeof(Collider2D))]
public class ThoughtChooseArea: MonoBehaviour
{
    Collider2D _collider;
    protected void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    public void SetMouth(bool isChoosing)
    {
        _collider.enabled = isChoosing;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Thought thought))
            BrainManager.Instance.AddInMouth(thought);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Thought thought))
            BrainManager.Instance?.RemoveFromMouth(thought);
    }
}}