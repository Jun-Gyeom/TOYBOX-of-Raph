using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HV
{
    Horizontal_LEFT, 
    Horizontal_RIGHT, 
    Vertical_LEFT, 
    Vertical_RIGHT
}

[Serializable]
public class Trail
{
    public Vector2 pos;
    public HV hv;
    public float time; 
}
