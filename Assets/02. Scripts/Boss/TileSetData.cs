using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileSetData
{
    [Header("공통")]
    public float coolDownTime; 

    [Header("타일 상태 변환")]
    public bool useTile;
    public List<Vector2> tilePositions;
    public TileType type;
    public float startupTime;
    public int holdingTime;

    [Header("기차 출현")]
    public bool useTrail;
    public List<TrailData> trails;
}
