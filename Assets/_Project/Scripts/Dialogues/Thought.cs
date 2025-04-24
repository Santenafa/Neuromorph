using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Neuromorph.Dialogues
{
    public class Thought : MonoBehaviour
    {
        public ThoughtSO ThoughtData { get; private set; }
        [SerializeField] private Color _draggingColor;
        [SerializeField] private Color _chosenColor;
        [SerializeField] private TMP_Text _thoughtText;
        [SerializeField] private Image _thoughtImage;
        [SerializeField] private Outline _outline;
        
        private ThoughtState _state = ThoughtState.Idle;
        private readonly Collider2D[] _endDragOverlapColliders = new Collider2D[10];
        private Rigidbody2D _rBody;
        private CapsuleCollider2D _collider;
        private Vector3 _mouseOffset;

        private void Awake()
        {
            _rBody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<CapsuleCollider2D>();
        }

        private void OnMouseDown()
        {
            _mouseOffset = transform.position - GetMousePos();
            _rBody.MovePosition(GetMousePos() + _mouseOffset);
        }

        private void OnMouseOver()
        {
            if (_state == ThoughtState.Idle) SetOutline(true, _draggingColor);
        }
        private void OnMouseExit()
        { 
            if (_state == ThoughtState.Idle) SetOutline(false, _draggingColor);
        }

        private void OnMouseDrag()
        {
            _rBody.MovePosition(GetMousePos() + _mouseOffset);
        }
        
        private void OnMouseUp()
        {
            /*_collider2D.enabled = false;
            int size = Physics2D.OverlapBoxNonAlloc(
                transform.position, _collider.bounds.size,
                0f,_endDragOverlapColliders
            );
            _collider2D.enabled = true;

            if (size <= 0) return;
            
            foreach (Collider2D col in _endDragOverlapColliders) {
                if (col.TryGetComponent(out Thought thought)) {
                    thought.Highlight();
                }
            }*/
        }

        public void Init(ThoughtSO data, Bounds spawnBounds)
        {
            ThoughtData = data;
            _thoughtText.text = ThoughtData.NameValue;

            LayoutRebuilder.ForceRebuildLayoutImmediate(_thoughtImage.rectTransform);
            _collider.size = _thoughtImage.rectTransform.rect.size;
            SetState(ThoughtState.Idle);
            
            Bounds thoughtBounds = _collider.bounds;
            
            float x = Random.Range(spawnBounds.min.x, spawnBounds.max.x - thoughtBounds.size.x); 
            float y = Random.Range(spawnBounds.min.y + thoughtBounds.size.y, spawnBounds.max.y);
            
            transform.position = new Vector3(x, y, 0f);
        }

        private static Vector3 GetMousePos() => CameraManager.Instance.GetMousePositionInBrainSpace();

        private void SetOutline(bool isEnabled, Color color)
        {
            _outline.enabled = isEnabled;
            _outline.effectColor = color;
        }

        public void SetState(ThoughtState state)
        {
            _state = state;
            
            switch (state)
            {
                case ThoughtState.Idle:
                    SetOutline(false, _draggingColor);
                    break;
                case ThoughtState.Chosen:
                    SetOutline(true, _chosenColor);
                    break;
                default: break;
            }
        }
    }
    public enum ThoughtState { Idle, Chosen }
}
