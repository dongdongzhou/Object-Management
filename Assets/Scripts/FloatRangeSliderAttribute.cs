﻿using UnityEngine;
using UnityEditor;

public class FloatRangeSliderAttribute : PropertyAttribute
{
    public float Min { get; }
    public float Max { get; }

    public FloatRangeSliderAttribute(float min, float max)
    {
        Min = min;
        Max = max;
    }
}