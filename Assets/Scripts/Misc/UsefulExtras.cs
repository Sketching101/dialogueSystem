using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Reference wrapper class for structs
/// </summary>
/// <typeparam name="T">Generic struct type</typeparam>
public class structRefWrapper<T> where T : struct
{
    public T value;
}