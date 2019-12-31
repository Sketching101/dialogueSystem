using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Dialogue
{
    public class DialogueDataStructs : SerializedMonoBehaviour
    {
        public static DialogueDataStructs Instance { get; private set; } // static singleton

        // Dictionary of all Character Data, should not be altered in scripts
        [DictionaryDrawerSettings(KeyLabel = "Character ID", ValueLabel = "Character Asset")]
        public Dictionary<string, CharacterData> characters = new Dictionary<string, CharacterData>();

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this; 
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}