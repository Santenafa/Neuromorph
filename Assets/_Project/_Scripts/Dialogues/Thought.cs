using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Neuromorph.Dialogues
{
    public class Thought : MonoBehaviour,
        IPointerUpHandler, IPointerDownHandler,
        IDragHandler, IDropHandler, IEndDragHandler,
        IPointerEnterHandler, IPointerExitHandler
    {
        public string Name { get; private set; }
        [Header("----- Filter -----")]
        [SerializeField] private ContactFilter2D _contactFilter;
        
        [Header("----- Colors -----")]
        [SerializeField] private Color _draggingColor;
        [SerializeField] private Color _chosenColor;
        
        [Header("----- UI -----")]
        [SerializeField] private TMP_Text _thoughtText;
        [SerializeField] private Image _thoughtImage;
        [SerializeField] private Outline _outline;
        [SerializeField] private BoxCollider2D _colliderToResize;
        
        [Header("----- Audio -----")]
        [SerializeField] private AudioClip _placeSFX;
        [SerializeField] private AudioClip _deleteSFX;
        [SerializeField] private AudioSource _audioSource;
        
        private ThoughtState _state = ThoughtState.Idle;
        private Rigidbody2D _rBody;
        private Collider2D _collider;
        private Vector2 _mouseOffset;
        private RectTransform _rectTransform;
        private Canvas _canvas;
        private Vector2 _targetPos = Vector2.zero;
        private bool _canFuse;
        
        private void Awake()
        {
            _rBody = GetComponent<Rigidbody2D>();
            _audioSource = GetComponent<AudioSource>();
            _rectTransform = GetComponent<RectTransform>();
            _collider = GetComponent<Collider2D>();
        }
        private void FixedUpdate()
        {
            if (_targetPos != Vector2.zero)
                _rBody.MovePosition(_targetPos);
        }
        public void Init(string thoughtName, Bounds spawnBounds)
        {
            _thoughtText.text = thoughtName;
            Name = thoughtName;
            SetPositionInBrain(spawnBounds);
        }

        private void SetPositionInBrain(Bounds spawnBounds)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_thoughtImage.rectTransform);
            Vector2 size = _thoughtImage.rectTransform.rect.size;
            
            _colliderToResize.size = size;
            
            SetState(ThoughtState.Idle);
            
            Bounds thoughtBounds = _colliderToResize.bounds;

            float x = Random.Range(spawnBounds.min.x + thoughtBounds.extents.x, spawnBounds.max.x - thoughtBounds.extents.x);
            float y = Random.Range(spawnBounds.min.y + thoughtBounds.extents.y, spawnBounds.max.y - thoughtBounds.extents.y);
            
            transform.position = new Vector3(x, y, 0f);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;

            
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _mouseOffset = (Vector2)transform.position - GetMousePos();
                _targetPos = GetMousePos() + _mouseOffset;
                gameObject.transform.SetAsLastSibling();
            }
            else Remove();
        }
        public void OnDrag(PointerEventData eventData)
        {
            _targetPos = GetMousePos() + _mouseOffset;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            _targetPos = Vector2.zero;
        }

        public void OnDrop(PointerEventData eventData)
        {
            var results = new Collider2D[5];
            int number = Physics2D.OverlapCollider(_collider, _contactFilter, results);

            if (number <= 0) return;
            
            foreach (Collider2D col in results)
            {
                if (col != null
                    && col.TryGetComponent(out Thought thought)
                    && BrainManager
                        .TryFuse(Name, thought.Name))
                {
                    thought.Remove();
                    Remove();
                }
            }
            //TODO: play place sound
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_state == ThoughtState.Idle) SetOutline(true, _draggingColor);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (_state == ThoughtState.Idle) SetOutline(false, _draggingColor);
        }
        
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

        private void Remove()
        {
            Destroy(gameObject);
        }
        
        private static Vector2 GetMousePos() => Input.mousePosition;
    }
    public enum ThoughtState { Idle, Chosen }
}
