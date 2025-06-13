using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Neuromorph.Dialogues
{
    public static class InkHandlers
    {
        // -------- Tags --------
        private const string CHOICE = "choice";
        
        public static string HandleNames(string currentLine, GameObject nameBox, TMP_Text nameText)
        {
            string[] splitTag = currentLine.Split(':');
            if (splitTag.Length != 2) return currentLine;
            
            string name = splitTag[0].Trim();
            string line = splitTag[1].Trim();
            
            nameBox.SetActive(true);
            nameText.text = name;

            return line;
        }
        public static bool HandleChoices(string choiceLine, out string keyWord, out string promoLine)
        {
            string[] splitChoice = choiceLine.Split(':');
            if (splitChoice.Length == 2)
            {
                keyWord = splitChoice[0].Trim();
                promoLine = splitChoice[1].Trim();
                return true;
            }
            keyWord = "Error";
            promoLine = "Error";
            return false;
        }
        
        public static void HandleTags(List<string> currentTags)
        {
            foreach (string t in currentTags)
            {
                string[] splitTag = t.Split(':');
                if (splitTag.Length != 2) Debug.LogError($"Invalid tag: {t}");
                
                string tagKey = splitTag[0].Trim();
                string tagValue = splitTag[1].Trim();

                switch (tagKey)
                {
                    case CHOICE:
                        
                        break;
                    default: Debug.LogWarning($"Tag is not handled: {tagKey}"); break;
                }
            }
        }
    }
}