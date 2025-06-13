using Neuromorph.Dialogues;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Neuromorph.UI
{
    [RequireComponent(typeof(Button))]
    public class MenuThought: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        private string _thoughtName;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnButtonClick);
        }

        public void Init(string thoughtName)
        {
            _thoughtName = thoughtName;
            _text.text = _thoughtName;
        }

        private void OnButtonClick()
        {
            BrainManager.Instance.SpawnThoughts(_thoughtName);
        }
    }
}