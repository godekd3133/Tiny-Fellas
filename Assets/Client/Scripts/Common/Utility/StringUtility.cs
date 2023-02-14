using UnityEngine;

public class StringUtility
{

    public const string E = "E";

    public static string ToEpsilonFormat(float value, int decimalPointDigit = 6)
    {
        var digit = Mathf.Floor(Mathf.Log10(value));
        value /= Mathf.Pow(10, digit);

        var integetValue = Mathf.FloorToInt(value);
        var decimalValue = value - Mathf.Floor(value);
        var digitMultiplier = Mathf.Pow(10, decimalPointDigit);
        decimalValue = Mathf.Floor(decimalValue *digitMultiplier)/digitMultiplier;
        
        return integetValue + decimalValue+E+(decimalPointDigit+digit);
    }
}
