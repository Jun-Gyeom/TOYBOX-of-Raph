using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum HV
{
    LEFT, 
    RIGHT, 
    UP, 
    DOWN
}

[Serializable]
public class TrailData
{
    public bool isFast;
    public Vector2 pos;
    public HV hv;
    public float speed; 
}
