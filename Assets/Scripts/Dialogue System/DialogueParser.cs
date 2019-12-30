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

        public void DrawCurrentDialogue()
        {
            if (dialogueIndex < dialogueTxt.Length)
            {
                DrawDialogueScene(dialogueTxt[dialogueIndex]);
                dialogueIndex++;

                // Use empty dialogue to change expressions
                if (dialogueTxt[dialogueIndex - 1].dialogueText == "")
                    DrawCurrentDialogue();
            } else
            {
                OnDialogueEnd();
            }
        }

        public virtual void OnDialogueEnd()
        {

        }

        public void DrawDialogueScene(DialogueLine line)
        {
            foreach (Image img in lSpriteCanvas)
            {
                img.color = new Color(0, 0, 0, 0);
            }
            foreach(Image img in rSpriteCanvas)
            {
                img.color = new Color(0, 0, 0, 0);
            }

            chars[line.character.charID] = line.character;

            DrawCharacter(line.character, true);
            if (debug)
                Debug.Break();
            foreach (charInDialogue character in chars.Values)
            {
                if(character.charID != line.character.charID)
                    DrawCharacter(character, false);
            }

            dialogueBox.text = line.dialogueText; 

        }
        public bool debug = false;
        public void DrawCharacter(charInDialogue character, bool addToDict)
        {
            if (!dialogueMap.charactersDict.ContainsKey(character.charID) && !addToDict)
                return;

            Image drawTo = lSpriteCanvas[0];
            Sprite img = characters[character.charID].mood[character.mood].leftSprite;

            dialogueMap.MoveCharacter(character.charID, character.pos);

            if (character.pos == Position.L)
            {
                drawTo = lSpriteCanvas[dialogueMap.GetCharacterPosition(character.charID)];
            }
            else if (character.pos == Position.R)
            {
                drawTo = rSpriteCanvas[dialogueMap.GetCharacterPosition(character.charID)];
            }

            if (character.lookDir == Dir.L)
            {
                img = characters[character.charID].mood[character.mood].leftSprite;
            }
            else if (character.lookDir == Dir.R)
            {
                img = characters[character.charID].mood[character.mood].rightSprite;
            }

            drawTo.color = activeChar;
            drawTo.sprite = img;
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