using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileSetData
{
    [Header("����")]
    public float coolDownTime;
    public bool playAttackAnim;

    [Header("Ÿ�� ���� ��ȯ")]
    public bool useTile;
    public List<Vector2> tilePositions = new List<Vector2>();
    public TileType type;
    public float startupTime;
    public int holdingTime;

    [Header("���� ����")]
    public bool useTrail;
    public List<TrailData> trails = new List<TrailData>();
}
