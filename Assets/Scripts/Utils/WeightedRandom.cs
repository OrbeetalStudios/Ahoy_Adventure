using UnityEngine;

public static class WeightedRandom
{
    public static int GetRandomWeightedIndex(float[] weights)
    {
        if (weights == null || weights.Length == 0) return -1;

        float w, total = 0f;
        int i = 0;

        for (i = 0; i < weights.Length; i++)
        {
            w = weights[i];
            if (float.IsPositiveInfinity(w)) return i;
            else if (w >= 0 && !float.IsNaN(w)) total += w;
        }

        float randomValue = Random.value;
        float partialSum = 0f;

        for (i = 0; i < weights.Length; i++)
        {
            w = weights[i];
            if (float.IsNaN(w) || w <= 0f) continue;

            partialSum += w / total;
            if (partialSum >= randomValue) return i;
        }

        return -1;
    }
}