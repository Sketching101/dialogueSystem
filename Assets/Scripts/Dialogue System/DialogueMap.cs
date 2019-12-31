using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System;
using UnityEngine.XR.WSA.Input;
using Sirenix.Serialization.Utilities.Editor;

namespace Dialogue
{
    public class DialogueMap : SerializedMonoBehaviour
    {
        // Data structures to hold 
        [DictionaryDrawerSettings(KeyLabel = "Character ID", ValueLabel = "Character Instance")]
        [SerializeField]
        private Dictionary<string, CharacterInDialogue> characters = new Dictionary<string, CharacterInDialogue>();
        // Contains a list of charIDs
        private string[] charsByPos = new string[10];

        [HideInInspector]
        public int lCount, rCount;

        public static int separator = 5;

        /// <summary>
        /// Clears and re-initializes data structures
        /// </summary>
        public void Clear()
        {
            characters = new Dictionary<string, CharacterInDialogue>();
            charsByPos = new string[separator * 2];
        }

        /// <summary>
        /// Inserts new character into the scene
        /// </summary>
        /// <param name="charID">charID of new character</param>
        /// <param name="pos">Position to add new character</param>
        /// <returns>Transition to be visualized</returns>
        private DTransition AddCharacter(string charID, int pos)
        {
            if (characters.ContainsKey(charID))
                return new DTransition(DTransitionEnum.Error, 0, 0, charID);

            DTransition transition = new DTransition(DTransitionEnum.Add, 0, pos, charID);
            
            // TODO: Make appropriate changes to data structures:
            // 1. Read the CharacterData from the DialogueDataStructs Instance
            CharacterData data = DialogueDataStructs.Instance.characters[charID];
            // 2. Make instance of CharacterInDialogue and add it to the dictionary
            CharacterInDialogue cInDialogue = new CharacterInDialogue(data);
            cInDialogue.pos = pos;
            characters.Add(charID, cInDialogue);
            // 3. Handle array overflow by removing that character from the scene
            
            return transition;
        }

        /// <summary>
        /// Removes character from scene
        /// </summary>
        /// <param name="charID">ID of the character that needs removal</param>
        /// <returns>Transition to be visualized</returns>
        public DTransition RemoveCharacter(string charID)
        {
            int charPos = 0;
            // TODO: Get character position
            DTransition transition = new DTransition(DTransitionEnum.Remove, charPos, 0, charID);

            // TODO: Make appropriate changes to data structures:
            // 1. Set the instance of the CharacterInDialogue to null

            return transition;

        }

        /// <summary>
        /// Moves character with given ID to a given position
        /// </summary>
        /// <param name="charID">Character ID of character to be moved</param>
        /// <param name="pos">Position character is being moved to</param>
        /// <returns>Transition to be visualized</returns>
        public DTransition MoveCharacter(string charID, int pos)
        {
            int charPos = 0;
            // TODO: Get character position
            DTransition transition = new DTransition(DTransitionEnum.Move, charPos, pos, charID);

            return transition;
        }

        /// <summary>
        /// Function that brings a character to the foreground
        /// </summary>
        /// <param name="charID">ID of character to be affected</param>
        /// <returns>Transition to be visualized</returns>
        public DTransition AddLightCharacter(string charID)
        {
            int charPos = 0;
            // TODO: Get character position
            DTransition transition = new DTransition(DTransitionEnum.AddLight, 0, charPos, charID);

            return transition;
        }

        /// <summary>
        /// Function that brings a character to the foreground
        /// </summary>
        /// <param name="charID">ID of character to be affected</param>
        /// <returns>Transition to be visualized</returns>
        public DTransition FadeLightCharacter(string charID)
        {
            int charPos = 0;
            // TODO: Get character position
            DTransition transition = new DTransition(DTransitionEnum.FadeLight, charPos, 0, charID);

            return transition;
        }


    }
}