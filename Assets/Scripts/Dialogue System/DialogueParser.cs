using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace Dialogue
{
    public class DialogueParser : SerializedMonoBehaviour
    {
        public TextAsset parseDialogue;
        public TextMeshProUGUI dialogueBox;

        public string dialogueSetup;
        public string[] dialogueTxt;

        [DictionaryDrawerSettings(KeyLabel = "Character ID", ValueLabel = "Character Asset")]
        public Dictionary<string, CharacterData> characters = new Dictionary<string, CharacterData>();

        [Button]
        public void InitializeDialogue()
        {
            string[] fullDialogue = parseDialogue.text.Split('#');

            dialogueSetup = fullDialogue[1];
            dialogueTxt = new string[fullDialogue.Length - 2];

            for(int i = 2; i < fullDialogue.Length; i++)
            {
                dialogueTxt[i - 2] = fullDialogue[i];
            }
//
        }
    }
}
