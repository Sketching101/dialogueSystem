using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {
    [CreateAssetMenu(fileName = "NewDialogueCharacter", menuName = "DialogueCharacter")]
    public class CharacterData : ScriptableObject
    {
        public string charID;
        public MoodSprite[] mood;
        [BoxGroup("Character Information")]
        public Bio charBio;

    }

    [System.Serializable]
    public struct Bio
    {
        public string charName;
        public int age;
        public string backstoryPath;
    }

    [System.Serializable]
    public struct MoodSprite
    {
        public Mood mood;

        public Sprite lookLeftSprite;
        public Sprite lookRightSprite;
    }

    public enum Mood
    {
        Happy, Sad, Angry, 
    }
}