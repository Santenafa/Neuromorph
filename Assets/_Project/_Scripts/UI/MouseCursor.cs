using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Neuromorph.UI
{
    [RequireComponent(typeof(Image))]
    public class MouseCursor : MonoBehaviour
    {
        [SerializeField] Sprite _mainCursorSprite;
        [SerializeField] Sprite _wormCursorSprite;
        Image _mouseImage;
        bool _isInsideBrain;

        void Start()
        {
            _mouseImage = GetComponent<Image>();
            Cursor.visible = false;
        }

        void Update()
        {
            transform.position = Input.mousePosition;
            bool isInsideBrain = EventSystem.current.IsPointerOverGameObject();

            if (_isInsideBrain == isInsideBrain) return;
            
            _isInsideBrain = isInsideBrain;
            _mouseImage.sprite = _isInsideBrain ? _wormCursorSprite : _mainCursorSprite;
        }
    }
}
