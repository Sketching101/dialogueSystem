using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {
    [CreateAssetMenu(fileName = "NewDialogueCharacter", menuName = "DialogueCharacter")]
    public class CharacterData : SerializedScriptableObject
    {
        public string charID;
        [DictionaryDrawerSettings(KeyLabel = "Mood", ValueLabel = "Mood Sprite")]
        public Dictionary<Mood, MoodSprite> mood = new Dictionary<Mood, MoodSprite>();
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

        public Sprite leftSprite;
        public Sprite rightSprite;
    }

    public enum Mood
    {
        Happy, Sad, Angry, INVALID
    }
    public enum Position
    {
        L, R, Center, INVALID
    }

    public enum Dir
    {
        L, R, INVALID
    }
}