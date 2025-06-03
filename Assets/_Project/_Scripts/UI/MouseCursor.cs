using UnityEngine;
using UnityEngine.UI;

namespace Neuromorph.UI
{
    public class MouseCursor : MonoBehaviour
    {
        [SerializeField] private Sprite _mainCursorSprite;
        [SerializeField] private Sprite _wormCursorSprite;
        private Image _mouseImage;
        private bool _isInsideBrain;

        private void Start()
        {
            _mouseImage = GetComponent<Image>();
            Cursor.visible = false;
        }

        private void Update()
        {
            transform.position = Input.mousePosition;// + _cursorHotspot;
            bool isInsideBrain = CameraManager.IsInsideWorldUI();

            if (_isInsideBrain == isInsideBrain) return;
            
            _isInsideBrain = isInsideBrain;
            _mouseImage.sprite = _isInsideBrain ? _wormCursorSprite : _mainCursorSprite;
        }
    }
}
