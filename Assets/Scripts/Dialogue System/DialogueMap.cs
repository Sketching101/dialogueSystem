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
        // Separator is the first index of the right side
        public int separator = 5;

        public Dictionary<string, int> charactersDict = new Dictionary<string, int>();
        
        // Highest index is the character in the foreground
        public string[] leftCharacters = new string[5];
        public string[] rightCharacters = new string[5];

        private string error = "error";

        public int lCount = 0;
        public int rCount = 0;

        [Button]
        public void ResetSideSize()
        {
            leftCharacters = new string[separator];
            rightCharacters = new string[separator];
        }

        public void MoveCharacter(string charID, Position pos)
        {
            if(InsertCharacter(charID, pos))
                return;

            Debug.Log("Moving");
            Remove(charactersDict[charID]);
            InsertCharacter(charID, pos);
        }

        [Button]
        public bool InsertCharacter(string charID, Position side)
        {
            if (charactersDict == null)
                charactersDict = new Dictionary<string, int>();
            if (charactersDict.ContainsKey(charID))
                return false;

            int pos;

            if(side == Position.R)
            {
                pos = separator * 2 - 1;
                rCount++;
            } else
            {
                pos = separator - 1;
                lCount++;
            }

            string overflow = Insert(charID, pos);
            
            if(overflow == "")
            {
                if (side == Position.R)
                {
                    rCount--;
                }
                else
                {
                    lCount--;
                }
            }

            if (lCount >= separator)
                lCount = separator - 1;
            if (rCount >= separator)
                rCount = separator - 1;

            return true;

        }

        /// <summary>
        /// Removes character from scene
        /// </summary>
        /// <param name="pos">Index of item to remove</param>
        /// <returns>Removed string</returns>
        private string Remove(int pos)
        {
            if (!charactersDict.ContainsKey(GetCharacter(pos)))
                return error;

            int low;
            if (pos >= separator)
            {
                low = separator;
                rCount--;
                if (rCount < 0)
                    rCount = 0;
            } else
            {
                low = 0;
                lCount--;
                if (lCount < 0)
                    lCount = 0;
            }
            int emptyCount = 0;

            ref string retVal = ref GetCharacter(pos);
            charactersDict.Remove(retVal);
            retVal = "";

            for(int i = pos; i >= low; i--)
            {
                string charID = GetCharacter(i);

                if (charID == "")
                    emptyCount++;
                else
                {
                    ref string toChange = ref GetCharacter(i + 1);
                    if (toChange != error)
                    {
                        toChange = charID;
                        charactersDict[charID] = i;
                    } else
                    {
                        Debug.LogError("Out of bounds in RemoveShift!");
                    }
                }
            }

            return retVal;
        }


        /// <summary>
        /// Inserts new character into the scene
        /// </summary>
        /// <param name="charIDin">charID of new character</param>
        /// <param name="pos">Position to add new character</param>
        /// <returns>charID of overflow, "" if no overflow, and error if character exists in scene</returns>
        private string Insert(string charIDin, int pos)
        {
            if (charactersDict.ContainsKey(charIDin) || GetCharacter(pos) == error)
                return error;


            int low = 0;
            if (pos >= separator)
                low = separator;

            string overflow = GetCharacter(low);

            for (int i = low; i < pos; i++)
            {
                ref string charID = ref GetCharacter(i);

                //charactersDict[charID] = i - 1;
                charID = GetCharacter(i + 1);
                charactersDict[charID] = i;
            }

            if (charactersDict.ContainsKey(overflow))
                charactersDict.Remove(overflow);

            charactersDict.Add(charIDin, pos);
            ref string charIDold = ref GetCharacter(pos);
            charIDold = charIDin;

            return overflow;
        }

        public ref string GetCharacter(int pos)
        {
            if (pos < separator && pos >= 0)
                return ref leftCharacters[pos];
            else if (pos >= separator && pos < 2 * separator)
                return ref rightCharacters[pos - separator];
            else
                return ref error;
        }

        public void UpdateDictionary(int insertIdx)
        {
            
        }

        public int GetCharacterPosition(string charID)
        {
            if (!charactersDict.ContainsKey(charID))
                return -1;

            int ret = charactersDict[charID];

            return ret % separator;
        }
    }

}