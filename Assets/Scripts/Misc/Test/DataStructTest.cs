using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DataStructTest : MonoBehaviour
{
    public int[] arr = new int[10];

    [Button]
    public void CoalesceTest(int separator)
    {
        Debug.Log(DataStructHelpers.Coalesce(arr, separator, 0));
    }


    [Button]
    public void ShiftTest(int start, int end, bool rightShift)
    {
        int overflow;
        if (rightShift)
            DataStructHelpers.ShiftRight(arr, start, end, out overflow);
        else
            DataStructHelpers.ShiftLeft(arr, start, end, out overflow);

        Debug.Log(overflow);
    }
}
