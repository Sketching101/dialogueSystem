using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using JetBrains.Annotations;

namespace Dialogue
{

    public class DialogueParser : SerializedMonoBehaviour
    {
        [Header("Text Items")]
        public TextAsset parseDialogue;
        public TextMeshProUGUI dialogueBox;

        [Header("Sprite Items")]
        public Image[] lSpriteCanvas;
        public Image[] rSpriteCanvas;
        public Color activeChar;
        public Color passiveChar;

        [Header("Dialogue Items")]
        public DialogueMap dialogueMap;
        public string dialogueSetup;
        public DialogueLine[] dialogueTxt;
        public int dialogueIndex = 0;

        [DictionaryDrawerSettings(KeyLabel = "Character ID", ValueLabel = "Character Asset")]
        public Dictionary<string, CharacterData> characters = new Dictionary<string, CharacterData>();

        private Dictionary<string, charInDialogue> chars = new Dictionary<string, charInDialogue>();

        private void Awake()
        {
            dialogueIndex = 0;
            InitializeDialogue();
            if(dialogueMap == null)
            {
                dialogueMap = GetComponent<DialogueMap>();
            }
        }

        private void Update()
        {
            
        }


        [Button]
        public void InitializeDialogue()
        {
            string[] fullDialogue = parseDialogue.text.Split('#');

            dialogueSetup = fullDialogue[1];
            dialogueTxt = new DialogueLine[fullDialogue.Length - 2];


            for(int i = 2; i < fullDialogue.Length; i++)
            {
                DialogueLine dLine;
                TextToDialogueLine(fullDialogue[i], out dLine);
                dialogueTxt[i - 2] = dLine;
                if (!chars.ContainsKey(dLine.character.charID))
                    chars.Add(dLine.character.charID, dLine.character);
            }

        }

        /// <summary>
        /// Returns true if valid dialogue line
        /// </summary>
        public bool TextToDialogueLine(string line, out DialogueLine dLine)
        {
            string charID = "";
            Mood mood = Mood.INVALID;
            Position pos = Position.INVALID;
            Dir lookDir = Dir.INVALID;
            string text = "";

            int len = line.Length;

            string curParam = ""; 
            int paramIdx = -1;

            bool isValid = true;

            for (int i = 0; i < len; i++)
            {
                if (line[i] == '[')
                {
                    paramIdx++;
                    curParam = "";
                }
                else if (line[i] == ']')
                {
                    switch (paramIdx)
                    {
                        case 0:
                            charID = curParam;
                            break;
                        case 1:
                            if (!DialogueHelper.GetEnum<Mood>(curParam, out mood))
                                isValid = false;
                            break;
                        case 2:
                            if (!DialogueHelper.GetEnum<Position>(curParam, out pos))
                                isValid = false;
                            break;
                        case 3:
                            if (!DialogueHelper.GetEnum<Dir>(curParam, out lookDir))
                                isValid = false;
                            break;
                        case 4:
                            text = curParam;
                            break;
                    }
                }
                else
                {
                    curParam += line[i];
                }
            }

            dLine = new DialogueLine(charID, mood, pos, lookDir, text);

            return isValid;
        }


        public virtual void OnDialogueStart()
        {

        }

        public virtual void OnDialogueEnd()
        {

        }

        public virtual void OnDialogueLineStart()
        {

        }

        public virtual void OnDialogueLineEnd()
        {

        }

        public virtual void OnDialogueLineNext()
        {

        }

    }

    [System.Serializable]
    public struct DialogueLine
    {
        public charInDialogue character;
        public string dialogueText;

        public DialogueLine(string charID_in, Mood mood_in, Position pos_in, Dir lookDir_in, string dialogueText_in)
        {
            character = new charInDialogue(charID_in, mood_in, pos_in, lookDir_in);
            dialogueText = dialogueText_in;
        }
    }

    public struct charInDialogue
    {
        public string charID;
        public Mood mood;
        public Position pos;
        public Dir lookDir;

        public charInDialogue(string charID_in, Mood mood_in, Position pos_in, Dir lookDir_in)
        {
            charID = charID_in;
            mood = mood_in;
            pos = pos_in;
            lookDir = lookDir_in;
        }


    }
}