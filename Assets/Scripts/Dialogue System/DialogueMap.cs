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
        [SerializeField]
        private string[] charsByPos = new string[10];

        [HideInInspector]
        public int lCount, rCount;

        public static int separator = 5;

        /// <summary>
        /// Clears and re-initializes data structures
        /// </summary>
        [Button]
        public void Clear()
        {
            characters = new Dictionary<string, CharacterInDialogue>();
            charsByPos = new string[separator * 2];
        }

        /// <summary>
        /// Updates the CharacterInDialogue mood of a given character
        /// </summary>
        /// <param name="charID">ID of the character</param>
        /// <param name="mood">Next mood</param>
        private void UpdateCharacterMood(string charID, Mood mood)
        {
            if(mood != Mood.INVALID)    
                characters[charID].mood = mood;
        }

        /// <summary>
        /// Inserts new character into the scene
        /// </summary>
        /// <param name="charID">ID of new character</param>
        /// <param name="pos">Position to add new character</param>
        /// <returns>Transition to be visualized</returns>
        [Button]
        public DTransition AddCharacter(string charID, int pos, Mood mood = Mood.INVALID)
        {
            if (characters.ContainsKey(charID))
                return new DTransition(DTransitionEnum.Error, 0, 0, charID, null);
            
            // 1. Read the CharacterData from the DialogueDataStructs Instance
            CharacterData data = DialogueDataStructs.Instance.characters[charID];

            // 2. Make instance of CharacterInDialogue and add it to the dictionary
            CharacterInDialogue cInDialogue = new CharacterInDialogue(data);
            cInDialogue.pos = pos;
            characters.Add(charID, cInDialogue);

            // 3. Handle array overflow by removing that character from the scene
            string overflowID = "";
            if (pos < separator)
                DataStructHelpers.ShiftLeft(charsByPos, 0, pos, out overflowID);
            else
                DataStructHelpers.ShiftRight(charsByPos, pos, 2 * separator - 1, out overflowID);

            charsByPos[pos] = charID;

            UpdateCharacterPositions();

            UpdateCharacterMood(charID, mood);

            DTransition transition = new DTransition(DTransitionEnum.Add, -1, pos, charID, cInDialogue.GetSprite());

            return transition;
        }

        /// <summary>
        /// Removes character from scene
        /// </summary>
        /// <param name="charID">ID of the character that needs removal</param>
        /// <returns>Transition to be visualized</returns>
        [Button]
        public DTransition RemoveCharacter(string charID, Mood mood = Mood.INVALID)
        {
            int charPos = characters[charID].pos;

            // 1. Set the instance of the CharacterInDialogue to null
            charsByPos[characters[charID].pos] = ""; 
            characters.Remove(charID);

            UpdateCharacterPositions();

            DTransition transition = new DTransition(DTransitionEnum.Remove, charPos, -1, charID, null);

            return transition;

        }

        /// <summary>
        /// Moves character with given ID to a given position
        /// </summary>
        /// <param name="charID">Character ID of character to be moved</param>
        /// <param name="pos">Position character is being moved to</param>
        /// <returns>Transition to be visualized</returns>
        [Button]
        public DTransition MoveCharacter(string charID, int pos, Mood mood = Mood.INVALID)
        {
            int charPos = characters[charID].pos;

            charsByPos[charPos] = "";

            string overflowID = "";
            if (pos < separator)
                DataStructHelpers.ShiftLeft(charsByPos, 0, pos, out overflowID);
            else
                DataStructHelpers.ShiftRight(charsByPos, pos, 2 * separator - 1, out overflowID);

            charsByPos[pos] = charID;

            UpdateCharacterPositions();

            UpdateCharacterMood(charID, mood);

            DTransition transition = new DTransition(DTransitionEnum.Move, charPos, pos, charID, characters[charID].GetSprite());

            return transition;
        }

        /// <summary>
        /// Function that brings a character to the foreground
        /// </summary>
        /// <param name="charID">ID of character to be affected</param>
        /// <returns>Transition to be visualized</returns>
        [Button]
        public DTransition AddLightCharacter(string charID, Mood mood = Mood.INVALID)
        {
            int charPos = characters[charID].pos;

            UpdateCharacterMood(charID, mood);

            DTransition transition = new DTransition(DTransitionEnum.AddLight, 0, charPos, charID, characters[charID].GetSprite());

            return transition;
        }

        /// <summary>
        /// Function that brings a character to the foreground
        /// </summary>
        /// <param name="charID">ID of character to be affected</param>
        /// <returns>Transition to be visualized</returns>
        [Button]
        public DTransition FadeLightCharacter(string charID, Mood mood = Mood.INVALID)
        {
            int charPos = characters[charID].pos;

            UpdateCharacterMood(charID, mood);

            DTransition transition = new DTransition(DTransitionEnum.FadeLight, charPos, 0, charID, characters[charID].GetSprite());

            return transition;
        }

        private void UpdateCharacterPositions()
        {
            DataStructHelpers.Coalesce(charsByPos, separator, "");
            int lTemp = 0, rTemp = 0;

            for (int i = 0; i < charsByPos.Length; i++)
            {
                if (characters.ContainsKey(charsByPos[i]))
                {
                    characters[charsByPos[i]].pos = i;

                    if (i < charsByPos.Length / 2)
                        lTemp++;
                    else
                        rTemp++;
                }
                else if (charsByPos[i] != "")
                    charsByPos[i] = "";
            }

            lCount = lTemp;
            rCount = rTemp;
        }
    }
}