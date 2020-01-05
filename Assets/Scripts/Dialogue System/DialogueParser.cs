using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using JetBrains.Annotations;
using UnityEditor.Animations;
using TreeEditor;

namespace Dialogue
{

    public class DialogueParser : SerializedMonoBehaviour
    {
        [Header("Text Items")]
        public TextAsset parseDialogue;
        public TextMeshProUGUI dialogueBox;

        [Header("Sprite Items")]
        public Dictionary<string, Image> charSpriteCanvas = new Dictionary<string, Image>();
        public Color activeChar;
        public Color passiveChar;

        [Header("Dialogue Items")]
        public DialogueMap dialogueMap;
        public string dialogueSetup;
        public DialogueLine[] dialogueTxt;
        public int dialogueIndex = 0;

        [DictionaryDrawerSettings(KeyLabel = "Character ID", ValueLabel = "Character Asset")]
        public Dictionary<string, CharacterData> characters = new Dictionary<string, CharacterData>();

        private Dictionary<string, CharInDialogue> chars = new Dictionary<string, CharInDialogue>();

        [SerializeField]
        private List<DTransition> transitionList = new List<DTransition>();

        private IEnumerator writeLineCoroutine;

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
            Dir lookDir = Dir.INVALID;
            DTransitionEnum transition = DTransitionEnum.None;
            string text = "";

            int len = line.Length;

            string curParam = "";
            string[] paramArr = new string[1];

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
                            if (!DialogueHelper.GetEnum(curParam, out mood))
                                isValid = false;
                            break;
                        case 2:
                            if (!DialogueHelper.GetEnum(curParam, out lookDir))
                                isValid = false;
                            break;
                        case 3:
                            if (!DialogueHelper.GetEnum(curParam, out transition))
                                transition = DTransitionEnum.None;
                            break;
                        case 4:
                            paramArr = curParam.Split(',');
                            break;
                        case 5:
                            text = curParam;
                            break;
                    }
                }
                else
                {
                    curParam += line[i];
                }
            }

            dLine = new DialogueLine(charID, mood, lookDir, text, transition, paramArr);

            return isValid;
        }

        public virtual void OnDialogueStart()
        {
            OnDialogueLineNext();
        }

        public virtual void OnDialogueEnd()
        {
            Debug.Log("End!");
        }

        public void OnDialogueLineStart(DialogueLine dLine)
        {
            ParseDialogueLineForTransition(dLine);

            for (int i = transitionList.Count - 1; i >= 0; i--)
            {
                DialogueTransitionHandler.HandleTransition(transitionList[i]);
                transitionList.RemoveAt(i);
            }

            WriteLine(dLine.dialogueText, 4);
        }

        public void ParseDialogueLineForTransition(DialogueLine dLine)
        {
            int pos = 0;

            DTransition transition = new DTransition(DTransitionEnum.None, 0, 0, dLine.character.charID, null);

            switch (dLine.transition)
            {
                case DTransitionEnum.Add:
                    if (int.TryParse(dLine.args[0], out pos))
                    {
                        transition = dialogueMap.AddCharacter(dLine.character.charID, pos, dLine.character.mood);
                    }
                    break;
                case DTransitionEnum.Move:
                    if (int.TryParse(dLine.args[0], out pos))
                    {
                        transition = dialogueMap.MoveCharacter(dLine.character.charID, pos, dLine.character.mood);
                    }
                    break;
                case DTransitionEnum.Remove:
                    transition = dialogueMap.RemoveCharacter(dLine.character.charID, dLine.character.mood);
                    break;
            }

            transitionList.Add(transition);
        }

        public virtual void OnDialogueLineEnd(DialogueLine dLine)
        {
       //     transitionList.Clear();
            dialogueIndex++;
        }

        public virtual void OnDialogueLineNext()
        {
            if (dialogueIndex < dialogueTxt.Length)
            {
                OnDialogueLineStart(dialogueTxt[dialogueIndex]);
                OnDialogueLineEnd(dialogueTxt[dialogueIndex]);
            } else
            {
                OnDialogueEnd();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="waitBetweenPrints"></param>
        private void WriteLine(string txt, float waitBetweenPrints = 1)
        {
            if(writeLineCoroutine != null)
                StopCoroutine(writeLineCoroutine);

            writeLineCoroutine = WriteLineCoroutine(txt, waitBetweenPrints, dialogueBox);
            StartCoroutine(writeLineCoroutine);
        }

        private IEnumerator WriteLineCoroutine(string txt, float waitBetweenPrints, TextMeshProUGUI textBox)
        {
            textBox.text = "";
            for(int i = 0; i < txt.Length; i++)
            {
                textBox.text = txt.Substring(0, i);
                for(int j = 0; j < waitBetweenPrints; j++)
                {
                    yield return null;
                }
                yield return null;
            }

            yield return null;
        }
    }

    [System.Serializable]
    public struct DialogueLine
    {
        public CharInDialogue character;
        public DTransitionEnum transition;
        public string[] args;
        public string dialogueText;

        public DialogueLine(string charID_in, Mood mood_in, Dir lookDir_in, string dialogueText_in, DTransitionEnum transition_in, string[] args_in)
        {
            transition = transition_in;
            character = new CharInDialogue(charID_in, mood_in, lookDir_in);
            dialogueText = dialogueText_in;
            args = args_in;
        }
    }

    public struct CharInDialogue
    {
        public string charID;
        public Mood mood;
        public Dir lookDir;

        public CharInDialogue(string charID_in, Mood mood_in, Dir lookDir_in)
        {
            charID = charID_in;
            mood = mood_in;
            lookDir = lookDir_in;
        }
    }
}