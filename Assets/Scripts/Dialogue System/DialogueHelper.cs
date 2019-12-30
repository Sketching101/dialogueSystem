using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public static class DialogueHelper
    {
        /// <summary>
        /// Outputs enum value by text
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="str">String that is supposed to be the enum name</param>
        /// <param name="result">The enum output</param>
        /// <returns>Whether or not the enum was found</returns>
        public static bool GetEnum<T>(string str, out T result) where T : struct
        {
            //Debug.Log(str);
            return System.Enum.TryParse<T>(str, true, out result);
        }
    }
}