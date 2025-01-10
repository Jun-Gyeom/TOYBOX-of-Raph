using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileSetData
{
    [Header("공통")]
    [SerializeField] private float coolDownTime; 

    [Header("타일 상태 변환")]
    [SerializeField] private bool useTile;
    [SerializeField] private List<Vector2> tilePositions;
    [SerializeField] private TileType type;
    [SerializeField] private float startupTime;
    [SerializeField] private int holdingTime;

    [Header("기차 출현")]
    [SerializeField] private bool useTrail;
    [SerializeField] private List<Trail> trails;
}
