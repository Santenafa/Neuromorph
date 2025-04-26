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
        private ThoughtSO _thoughtData;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnButtonClick);
        }

        public void Init(ThoughtSO thoughtData)
        {
            _thoughtData = thoughtData;
            _text.text = _thoughtData.NameValue;
        }

        private void OnButtonClick()
        {
            BrainManager.Instance.SpawnThought(_thoughtData);
        }
    }
}