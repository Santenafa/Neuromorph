using Neuromorph.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Neuromorph.Dialogues
{
    public class Thought : MonoBehaviour, IPointerDownHandler
    {
        public ThoughtSO ThoughtData { get; private set; }
        [Header("----- Colors -----")]
        [SerializeField] private Color _draggingColor;
        [SerializeField] private Color _chosenColor;
        
        [Header("----- UI -----")]
        [SerializeField] private TMP_Text _thoughtText;
        [SerializeField] private Image _thoughtImage;
        [SerializeField] private Outline _outline;
        [SerializeField] private BoxCollider2D[] _collidersToResize;
        
        [Header("----- Audio -----")]
        [SerializeField] private AudioClip _placeSFX;
        [SerializeField] private AudioClip _deleteSFX;
        [SerializeField] private AudioSource _audioSource;
        
        private ThoughtState _state = ThoughtState.Idle;
        private Rigidbody2D _rBody;
        private Vector3 _mouseOffset;
        
        private void Awake()
        {
            _rBody = GetComponent<Rigidbody2D>();
            _audioSource = GetComponent<AudioSource>();
        }
        
        public void Init(ThoughtSO data, Bounds spawnBounds)
        {
            ThoughtData = data;
            _thoughtText.text = ThoughtData.NameValue;

            LayoutRebuilder.ForceRebuildLayoutImmediate(_thoughtImage.rectTransform);
            Vector2 size = _thoughtImage.rectTransform.rect.size;
            
            BoxCollider2D[] colliders = _collidersToResize;
            foreach (BoxCollider2D col in colliders)
                col.size = size;
            
            SetState(ThoughtState.Idle);
            
            Bounds thoughtBounds = colliders[0].bounds;
            
            float x = Random.Range(spawnBounds.min.x, spawnBounds.max.x - thoughtBounds.size.x); 
            float y = Random.Range(spawnBounds.min.y + thoughtBounds.size.y, spawnBounds.max.y);
            
            transform.position = new Vector3(x, y, 0f);
            WordsManager.Instance.TryAddMenuThought(data);
        }
        

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                Remove();
            }
            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                _mouseOffset = transform.position - GetMousePos();
            }
        }
        private void OnMouseDown()
        {
            
        }

        private void OnMouseDrag()
        {
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
        
        private static Vector3 GetMousePos() => CameraManager.Instance.GetMousePositionInBrainSpace();
    }
    public enum ThoughtState { Idle, Chosen }
}
