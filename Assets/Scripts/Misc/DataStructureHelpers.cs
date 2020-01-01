using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;


public static class DataStructHelpers
{
    /// <summary>
    /// Shifts part of the array to the right
    /// </summary>
    /// <typeparam name="T">Type of the elements in the array</typeparam>
    /// <param name="firstShift">First element to be shifted</param>
    /// <param name="lastShift">Last element to be shifted, and overflow</param>
    /// <param name="overflow">Overflow element</param>
    /// <returns>-1 on failure, 1 on success</returns>
    public static int ShiftRight<T>(T[] array, int firstShift, int lastShift, out T overflow)
    {
        overflow = default(T);

        if (array == null || firstShift < 0 || firstShift >= array.Length || lastShift < 0 || lastShift >= array.Length)
            return -1;

        for (int i = lastShift; i > firstShift; i--)
        {
            array[i] = array[i - 1];
            array[i - 1] = default(T);
        }

        return 0;
    }


    /// <summary>
    /// Shifts part of the array to the left
    /// </summary>
    /// <typeparam name="T">Type of the elements in the array</typeparam>
    /// <param name="array">Array to alter</param>
    /// <param name="firstShift">First element to be shifted</param>
    /// <param name="lastShift">Last element to be shifted, and overflow</param>
    /// <param name="lastShift">Overflow element</param>
    /// <returns>-1 on failure, 1 on success</returns>
    public static int ShiftLeft<T>(T[] array, int firstShift, int lastShift, out T overflow)
    {
        overflow = default(T);

        if (array == null || firstShift < 0 || firstShift >= array.Length || lastShift < 0 || lastShift >= array.Length)
            return -1;


        overflow = array[firstShift];

        for (int i = firstShift; i < lastShift; i++)
        {
            array[i] = array[i + 1];
            array[i + 1] = default(T);
        }

        return 0;
    }

    /// <summary>
    /// Moves all elements in a sparse array to get close to a single point
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">Array to alter</param>
    /// <param name="singularity">Array element to coalesce to</param>
    /// <returns>-1 on failure, emptyCount on success</returns>
    public static int Coalesce<T>(T[] array, int singularity, T nullVal)
    {
        if (singularity > array.Length)
            return -1;

        int emptyCountTotal = 0;

        int emptyCount = 0;
        for(int i = singularity; i < array.Length; i++)
        {
            if(EqualityComparer<T>.Default.Equals(array[i], nullVal))
            {
                emptyCount++;
            } else
            {
                T temp = array[i];
                array[i] = nullVal;
                array[i - emptyCount] = temp;
            }
        }
        emptyCountTotal += emptyCount;
        emptyCount = 0;
        for(int i = singularity - 1; i >= 0; i--)
        {
            if (EqualityComparer<T>.Default.Equals(array[i], nullVal))
            {
                emptyCount++;
            }
            else
            {
                T temp = array[i];
                array[i] = nullVal;
                array[i + emptyCount] = temp; 
            }
        }

        emptyCountTotal += emptyCount;

        return emptyCountTotal;
    }
}
