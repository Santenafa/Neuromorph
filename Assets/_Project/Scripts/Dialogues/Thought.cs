using TMPro;
using UnityEngine;

namespace Neuromorph.Dialogues
{
    public class Thought : MonoBehaviour
    {
        public ThoughtSO ThoughtData => _data;
        public Collider2D Collider => _collider2D;
        [SerializeField] private ThoughtSO _data;
        [SerializeField] private TMP_Text _thoughtText;
        [SerializeField] private  Collider2D _collider2D;
        private readonly Collider2D[] _endDragOverlapColliders = new Collider2D[10];
        private Rigidbody2D _rBody;
        private Vector3 _mouseOffset;

        private void Awake()
        {
            _rBody = GetComponent<Rigidbody2D>();
        }

        private void OnMouseDown()
        {
            _mouseOffset = transform.position - GetMousePos();
            //transform.position = GetMousePos() + _mouseOffset;
            _rBody.MovePosition(GetMousePos() + _mouseOffset);
        }

        private void OnMouseDrag()
        {
            //transform.position = GetMousePos() + _mouseOffset;
            _rBody.MovePosition(GetMousePos() + _mouseOffset);
        }
        
        private void OnMouseUp()
        {
            //_collider2D.enabled = false;
            int size = Physics2D.OverlapBoxNonAlloc(
                transform.position, _collider2D.bounds.size,
                0f,_endDragOverlapColliders
            );
            //_collider2D.enabled = true;

            if (size <= 0) return;
            
            /*foreach (Collider2D col in _endDragOverlapColliders) {
                if (col.TryGetComponent(out Thought thought)) {
                    thought.Highlight();
                }
            }*/
        }

        public void Init(ThoughtSO data, Vector2 position)
        {
            _data = data;
            _thoughtText.text = _data.NameValue;
            transform.position = position;
        }

        private void Highlight()
        {
            
        }

        private static Vector3 GetMousePos() => CameraManager.Instance.GetMousePositionInBrainSpace();
    }
}
