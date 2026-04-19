using UnityEngine;
using UnityEngine.InputSystem;

public static class Extensions
{
    public static float NextFloat(this System.Random random, float minValue = 0f, float maxValue = 1f)
    {
        var range = maxValue - minValue;
        return (float)(random.NextDouble() * range + minValue);
    }

    public static float NextFloat(this System.Random random, float maxValue)
    {
        return random.NextFloat(0f, maxValue);
    }

    public static bool Prob(this System.Random random, float probability = .5f)
    {
        return random.NextDouble() < probability;
    }

    public static T Pick<T>(this System.Random random, T[] options) where T : notnull
    {
        return options[random.Next(options.Length)];
    }

    public static T Pick<T>(this System.Random random, System.Collections.Generic.List<T> options) where T : notnull
    {
        return options[random.Next(options.Count)];
    }
}
