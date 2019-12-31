using System.Collections;
using System.Collections.Generic;

public static class DataStructureHelpers
{
    /// <summary>
    /// Shifts part of the array to the right
    /// </summary>
    /// <typeparam name="T">Type of the elements in the array</typeparam>
    /// <param name="firstShift">First element to be shifted</param>
    /// <param name="lastShift">Last element to be shifted, and overflow</param>
    /// <returns>Overflow element</returns>
    public static T ShiftRight<T>(T[] array, int firstShift, int lastShift)
    {
        T ret = array[lastShift];

        for (int i = lastShift; i > firstShift; i--)
        {
            array[i] = array[i - 1];
            array[i - 1] = default(T);
        }

        return ret;
    }


    /// <summary>
    /// Shifts part of the array to the left
    /// </summary>
    /// <typeparam name="T">Type of the elements in the array</typeparam>
    /// <param name="array">Array to alter</param>
    /// <param name="firstShift">First element to be shifted</param>
    /// <param name="lastShift">Last element to be shifted, and overflow</param>
    /// <returns>Overflow element</returns>
    public static T ShiftLeft<T>(T[] array, int firstShift, int lastShift)
    {
        T ret = array[firstShift];

        for (int i = firstShift; i < lastShift; i++)
        {
            array[i] = array[i + 1];
            array[i + 1] = default(T);
        }

        return ret;
    }

    /// <summary>
    /// Moves all elements in a sparse array to get close to a single point
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">Array to alter</param>
    /// <param name="singularity"></param>
    /// <returns></returns>
    public static void Coalesce<T>(T[] array, int singularity)
    {
        //TODO: Write this function!
    }
}
