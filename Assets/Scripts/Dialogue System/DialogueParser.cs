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
        public TextAsset parseDialogue;
        public TextMeshProUGUI dialogueBox;

        public Image lSpriteCanvas, rSpriteCanvas;

        public string dialogueSetup;
        public DialogueLine[] dialogueTxt;
        public int dialogueIndex = 0;

        [DictionaryDrawerSettings(KeyLabel = "Character ID", ValueLabel = "Character Asset")]
        public Dictionary<string, CharacterData> characters = new Dictionary<string, CharacterData>();

        private void Awake()
        {
            dialogueIndex = 0;
            InitializeDialogue();
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

        public void DrawCurrentDialogue()
        {
            if (dialogueIndex < dialogueTxt.Length)
            {
                DrawDialogueLine(dialogueTxt[dialogueIndex]);
                dialogueIndex++;
            } else
            {
                OnDialogueEnd();
            }
        }

        public virtual void OnDialogueEnd()
        {

        }

        public void DrawDialogueLine(DialogueLine line)
        {
            Image drawTo = lSpriteCanvas;
            Sprite img = characters[line.charID].mood[line.mood].leftSprite;
            if (line.pos == Position.L)
            {
                drawTo = lSpriteCanvas;
                img = characters[line.charID].mood[line.mood].leftSprite;
            }
            else if (line.pos == Position.R)
            {
                drawTo = rSpriteCanvas;
                img = characters[line.charID].mood[line.mood].rightSprite;
            }
            drawTo.color = Color.white;
            drawTo.sprite = img;

            dialogueBox.text = line.dialogueText;
        }
    }

    [System.Serializable]
    public struct DialogueLine
    {
        public string charID;
        public Mood mood;
        public Position pos;
        public Dir lookDir;
        public string dialogueText;

        public DialogueLine(string charID_in, Mood mood_in, Position pos_in, Dir lookDir_in, string dialogueText_in)
        {
            charID = charID_in;
            mood = mood_in;
            pos = pos_in;
            lookDir = lookDir_in;
            dialogueText = dialogueText_in;
        }
    }
}