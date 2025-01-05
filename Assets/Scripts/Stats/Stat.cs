using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField]private float baseValue;

    public List<float> modifiers = new();

    public Stat(float baseValue = 0)
    {
        this.baseValue = baseValue;
    }

    public float Value()
    {
        return Mathf.Max(0, baseValue + modifiers.Sum(m => m)); // clamp stat values to 0 after modifiers
    }

    public void SetBaseValue(float value)
    {
        baseValue = value;
    }

    public void ModifyBaseValue(float value)
    {
        baseValue += value;
    }

    public float GetBaseValue()
    {
        return baseValue;
    }

    public static implicit operator float(Stat stat)
    {
        return stat.Value();
    }
}

