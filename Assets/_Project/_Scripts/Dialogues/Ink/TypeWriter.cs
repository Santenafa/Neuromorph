using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

namespace Neuromorph.Dialogues
{
    public class TMP_Typewriter
	{
		public bool IsTyping { get; private set; }
		private readonly TMP_Text _textUI;
		private string _parsedText;
		private Action _onComplete;
		private Tween _tween;

		public TMP_Typewriter(TMP_Text textUI) =>
			_textUI = textUI;

		private void OnDestroy()
		{
			_tween?.Kill();
			_tween = null;
			_onComplete = null;
		}

		/// <summary> Start new typewriter effect </summary>
		/// <param name="text"> Text to display (rich text supported) </param>
		/// <param name="speed">Display speed (if speed == 1, it takes 1 second to display one character,
		/// if speed == 2, it takes 0.5 seconds)</param>
		/// <param name="onComplete">A callback that is called when typewriting is completed.</param>
		/// /// <param name="ease"> Ease method for typing animation</param>
		public void Play(string text, float speed, Action onComplete = null, Ease ease = Ease.Linear)
		{
			IsTyping = true;
			_textUI.text = text;
			_onComplete = onComplete;

			//force regeneration of the text object before its normal process time
			_textUI.ForceMeshUpdate();
			
			//get the text after it has been parsed and rich text tags removed.
			_parsedText = _textUI.GetParsedText(); 

			int length = _parsedText.Length;
			float duration = 1 / speed * length;

			OnUpdate(0);

			_tween?.Kill();

			_tween = DOTween
				.To(OnUpdate, 0, 1, duration)
				.SetEase(ease)
				.OnComplete(OnComplete)
			;
		}

		/// <summary> Skip typing animation </summary>
		/// <param name="withCallbacks"> is calling Callback when typing is skipped</param>
		public void Skip(bool withCallbacks = true)
		{
			_tween?.Kill();
			_tween = null;
			IsTyping = false;

			OnUpdate(1);

			if (!withCallbacks) return;

			_onComplete?.Invoke();
			_onComplete = null;
		}

		public void Pause() => _tween?.Pause();
		public void Resume() => _tween?.Play();
		

		/// <summary> Updating max visible characters </summary>
		/// <param name="value"> (0-1) How much of the line is visible </param>
		private void OnUpdate(float value)
		{
			float current = Mathf.Lerp(0, _parsedText.Length, value);
			int count = Mathf.FloorToInt(current);

			_textUI.maxVisibleCharacters = count;
		}

		private void OnComplete()
		{
			IsTyping = false;
			_tween = null;
			_onComplete?.Invoke();
			_onComplete = null;
		}
	}
}