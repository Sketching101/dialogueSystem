using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public class CharacterCanvasLookup : SerializedMonoBehaviour
    {
        public static CharacterCanvasLookup Instance { get; private set; }

        [DictionaryDrawerSettings(KeyLabel = "Character ID", ValueLabel = "Character Canvas")]
        public Dictionary<string, CharacterCanvas> charCanvasLookup = new Dictionary<string, CharacterCanvas>();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
    }
}