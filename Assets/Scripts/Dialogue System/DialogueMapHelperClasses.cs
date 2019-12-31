using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public enum  DTransitionEnum
    {
        None, Remove, Add, Move, AddLight, FadeLight, Error
    }

    /// <summary>
    /// Describes the kind of transition that needs to be visualized
    /// </summary>
    [System.Serializable]
    public class DTransition
    {
        public DTransitionEnum transitionType;
        
        // For Add, Move, AddLight
        public int posTo;

        // For Remove, Move, FadeLight
        public int posFrom;

        // For each transition
        public string charID;

        public DTransition(DTransitionEnum tType, int pFrom, int pTo, string cID)
        {
            transitionType = tType;
            posTo = pTo;
            posFrom = pFrom;
            charID = cID;
        }
    }

    /// <summary>
    /// Instance of a character in a given dialogue. 
    /// </summary>
    public class CharacterInDialogue
    {
        private CharacterData data;
        public Mood mood;
        public int pos;
        public Dir facing;

        /// <summary>
        /// Creates a default instance, filling mood and pos values with defaults
        /// </summary>
        /// <param name="charData">CharacterData object associated with the character</param>
        public CharacterInDialogue(CharacterData charData)
        {
            data = charData;
            mood = Mood.Happy;
            pos = -1;
            facing = Dir.R;
        }
        
        /// <summary>
        /// Returns the appropriate sprite for the current mood
        /// </summary>
        /// <returns>Sprite associated with the mood</returns>
        public Sprite GetSprite()
        {
            if (facing == Dir.L)
                return data.sprites[mood].leftSprite;
            else
                return data.sprites[mood].rightSprite;
        }
    }
}
