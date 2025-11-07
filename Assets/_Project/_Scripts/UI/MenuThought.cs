using Neuromorph.Dialogues;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Neuromorph
{
[RequireComponent(typeof(Button))]
public class MenuThought: MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    string _thoughtName;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    public void Init(string thoughtName)
    {
        _thoughtName = thoughtName;
        _text.text = _thoughtName;
    }

    void OnButtonClick()
    {
        BrainManager.Instance.SpawnThoughts(_thoughtName);
    }
}}