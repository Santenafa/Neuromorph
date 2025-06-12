using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Neuromorph.Dialogues
{
    public static class InkHandlers
    {
        // -------- Constants --------
        private const string SPEAKER_TAG = "speaker";
        
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
                    case SPEAKER_TAG:
                        
                        break;
                    default: Debug.LogWarning($"Tag is not handled: {tagKey}"); break;
                }
            }
        }
    }
}