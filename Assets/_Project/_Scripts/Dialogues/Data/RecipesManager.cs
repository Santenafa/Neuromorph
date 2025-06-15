using System.Collections.Generic;
using UnityEngine;

namespace Neuromorph.Dialogues.Data
{
	public class RecipesManager : Singleton<RecipesManager>
	{
		[SerializeField] private TextAsset _source;

		public static readonly List<string> Emotion = new() { "Love", "Hate" };

		private Dictionary<string, string> _fusionResults;

		public void Start()
		{
			Instance._fusionResults = new Dictionary<string, string>();
			
			string[] rows = _source.text.Split('\n'); //ряды
			string[] keyH = rows[0].Split(','); //ключи горизонтальные
			
			for (int i = 1; i < rows.Length; i++) //парсинг горизонтали
			{
				string[] columns = rows[i].Split(','); //колонны
				string keyV = columns[0].Replace("*", ""); //ключи вертикальные
				
				for (int j = i; j < columns.Length; j++) //парсинг вертикали
				{
					if (!columns[j].Trim().Equals("")
					      && !keyH[j].Trim().Equals(""))
					{
						AddRecipe(keyV, keyH[j].Replace("*", ""),
							columns[j].Replace("/", ",").Replace("*", ""));
					}
				}
			}
		}
		
		public static bool TryGetResults(string a, string b, out string[] results)
		{
			results = null;
			print($"TryFuse: {GetKey(a, b)}");
			if (!Instance._fusionResults.TryGetValue(GetKey(a, b), out string result)) return false;
			
			results = result.Split(',');
			return true;
		}
		
		private void AddRecipe(string a, string b, string answer)
		{
			print($"Added: {GetKey(a, b)}| Recipe: {answer}");
			_fusionResults.Add(GetKey(a, b), answer);
		}
		private static string GetKey(string a, string b)
		{
			a = a.Trim();
			b = b.Trim();
			return string.CompareOrdinal(a, b) > 0 ? (a + b) : (b + a);
		}
	}
}
