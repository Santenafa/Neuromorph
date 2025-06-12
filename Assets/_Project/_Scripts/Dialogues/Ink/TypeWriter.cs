using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

namespace Neuromorph.Dialogues
{
    public class TMP_Typewriter : MonoBehaviour
	{
		[SerializeField] private TMP_Text _textUI;

		//==============================================================================
		// 変数
		//==============================================================================
		private string _parsedText;
		private Action _onComplete;
		private Tween _tween;

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// アタッチされた時や Reset された時に呼び出されます
		/// </summary>
		private void Reset()
		{
			//_textUI = GetComponent<TMP_Text>();
		}

		/// <summary>
		/// 破棄される時に呼び出されます
		/// </summary>
		private void OnDestroy()
		{
			_tween?.Kill();
			_tween = null;
			_onComplete = null;
		}

		/// <summary>
		/// 演出を再生します
		/// </summary>
		/// <param name="text">表示するテキスト ( リッチテキスト対応 )</param>
		/// <param name="speed">表示する速さ ( speed == 1 の場合 1 文字の表示に 1 秒、speed == 2 の場合 0.5 秒かかる )</param>
		/// <param name="onComplete">演出完了時に呼び出されるコールバック</param>
		public void Play( string text, float speed, Action onComplete )
		{
			_textUI.text = text;
			_onComplete = onComplete;

			_textUI.ForceMeshUpdate();

			_parsedText = _textUI.GetParsedText();

			int length = _parsedText.Length;
			float duration = 1 / speed * length;

			OnUpdate( 0 );

			_tween?.Kill();

			_tween = DOTween
				.To( OnUpdate, 0, 1, duration )
				.SetEase( Ease.Linear )
				.OnComplete( OnComplete )
			;
		}

		/// <summary>
		/// 演出をスキップします
		/// </summary>
		/// <param name="withCallbacks">演出完了時に呼び出されるコールバックを実行する場合 true</param>
		public void Skip( bool withCallbacks = true )
		{
			_tween?.Kill();

			_tween = null;

			OnUpdate( 1 );

			if ( !withCallbacks ) return;

			_onComplete?.Invoke();

			_onComplete = null;
		}

		/// <summary>
		/// 演出を一時停止します
		/// </summary>
		public void Pause() => _tween?.Pause();
		

		/// <summary>
		/// 演出を再開します
		/// </summary>
		public void Resume() => _tween?.Play();
		

		/// <summary>
		/// 演出を更新する時に呼び出されます
		/// </summary>
		private void OnUpdate( float value )
		{
			float current = Mathf.Lerp( 0, _parsedText.Length, value );
			int count = Mathf.FloorToInt( current );

			_textUI.maxVisibleCharacters = count;
		}

		/// <summary>
		/// 演出が更新した時に呼び出されます
		/// </summary>
		private void OnComplete()
		{
			_tween = null;
			_onComplete?.Invoke();
			_onComplete = null;
		}
	}
}