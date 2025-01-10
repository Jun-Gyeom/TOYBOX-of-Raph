using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileSetData
{
    [Header("����")]
    public float coolDownTime; 

    [Header("Ÿ�� ���� ��ȯ")]
    public bool useTile;
    public List<Vector2> tilePositions;
    public TileType type;
    public float startupTime;
    public int holdingTime;

    [Header("���� ����")]
    public bool useTrail;
    public List<TrailData> trails;
}
