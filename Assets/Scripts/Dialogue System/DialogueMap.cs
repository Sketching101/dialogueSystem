using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace Dialogue
{
    public class DialogueMap : MonoBehaviour
    {
        // Separator is the first index of the left side
        public int separator;

        [DictionaryDrawerSettings(KeyLabel = "Character ID", ValueLabel = "Character Order From Left")]
        public Dictionary<string, int> positionByChar = new Dictionary<string, int>();

        private Dictionary<int, string> charByPosition = new Dictionary<int, string>();

        public void NewCharPosition(string charID, int pos)
        {
            if (!positionByChar.ContainsKey(charID))
            {
                positionByChar.Add(charID, pos);
            }

            int oldPos = positionByChar[charID];
            positionByChar[charID] = pos;
            charByPosition.Remove(oldPos);
            if(charByPosition.ContainsKey(pos))
            {
                string oldCharID = charByPosition[pos];
                NewCharPosition(oldCharID, pos + 1);

                if (!charByPosition.ContainsKey(pos))
                    charByPosition.Add(pos, charID);
                else
                    charByPosition[pos] = charID;
            } 
        }

        public void InsertChar(string charID, int pos)
        {

        }
    }
}