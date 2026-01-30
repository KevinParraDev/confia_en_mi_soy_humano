using System.Collections.Generic;
using UnityEngine;

public static class RandomHelpers 
{
    public static void ShuffleArray<T>(T[] array)
    {
        int arrSize = array.Length;

        for (int i = 0; i < arrSize; i++)
        {
            int indexToSwap = Random.Range(0, arrSize);

            T elementToSwap = array[i];
            array[i] = array[indexToSwap];
            array[indexToSwap] = elementToSwap;
        }
    }

    public static void ShuffleList<T>(List<T> list)
    {
        int arrSize = list.Count;

        for (int i = 0; i < arrSize; i++)
        {
            int indexToSwap = Random.Range(0, arrSize);

            T elementToSwap = list[i];
            list[i] = list[indexToSwap];
            list[indexToSwap] = elementToSwap;
        }
    }
}
