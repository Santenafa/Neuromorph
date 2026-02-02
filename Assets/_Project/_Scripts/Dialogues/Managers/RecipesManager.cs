using System.Collections.Generic;
using UnityEngine;

namespace Neuromorph.Dialogues.Data
{
public class RecipesManager : MonoBehaviour
{
	[SerializeField] TextAsset _source;

	public static readonly List<string> Emotion = new() { "Love", "Hate" };

	Dictionary<string, string> _fusionResults;

	public void Start()
	{
		_fusionResults = new Dictionary<string, string>();
		
		string[] rows = _source.text.Split('\n'); //rows
		string[] keyH = rows[0].Split(','); //horizontal keys
		
		for (int i = 1; i < rows.Length; i++) //horizontal parsing
		{
			string[] columns = rows[i].Split(','); //columns
			string keyV = columns[0].Replace("*", ""); //vertical keys
			
			for (int j = i; j < columns.Length; j++) //vertical parsing
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
	
	public bool TryGetResults(string a, string b, out string[] results)
	{
		results = null;
		print($"TryFuse: {GetKey(a, b)}");
		if (!_fusionResults.TryGetValue(GetKey(a, b), out string result)) return false;
		
		results = result.Split(',');
		return true;
	}

	void AddRecipe(string a, string b, string answer)
	{
		print($"Added: {GetKey(a, b)}| Recipe: {answer}");
		_fusionResults.Add(GetKey(a, b), answer);
	}

	static string GetKey(string a, string b)
	{
		a = a.Trim();
		b = b.Trim();
		return string.CompareOrdinal(a, b) > 0 ? (a + b) : (b + a);
	}
}
}
