using TMPro;
using UnityEngine;

namespace Neuromorph.Dialogues
{
    public class Thought : MonoBehaviour
    {
        [SerializeField] private string _textValue = "Nothing";
        [SerializeField] private TMP_Text _thoughtText;
        private Collider2D _collider2D;
        private readonly Collider2D[] _endDragOverlapColliders = new Collider2D[10];
        private Vector3 _mouseOffset;

        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
            _thoughtText.text = _textValue;
        }
        
        private void OnMouseDown()
        {
            _mouseOffset = transform.position - GetMousePos();
            transform.position = GetMousePos() + _mouseOffset;
        }

        private void OnMouseDrag()
        {
            transform.position = GetMousePos() + _mouseOffset;
            
        }
        
        private void OnMouseUp()
        {
            _collider2D.enabled = false;
            int size = Physics2D.OverlapBoxNonAlloc(
                transform.position, _collider2D.bounds.size,
                0f,_endDragOverlapColliders
            );
            _collider2D.enabled = true;

            if (size <= 0) return;
            
            /*foreach (Collider2D col in _endDragOverlapColliders) {
                if (col.TryGetComponent(out Thought thought)) {
                    thought.Highlight();
                }
            }*/
        }

        private void Highlight()
        {
            
        }

        private static Vector3 GetMousePos() => CameraManager.Instance.GetMousePositionInBrainSpace();
    }
}
