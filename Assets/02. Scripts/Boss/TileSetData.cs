using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileSetData
{
    [Header("����")]
    [SerializeField] private float coolDownTime; 

    [Header("Ÿ�� ���� ��ȯ")]
    [SerializeField] private bool useTile;
    [SerializeField] private List<Vector2> tilePositions;
    [SerializeField] private TileType type;
    [SerializeField] private float startupTime;
    [SerializeField] private int holdingTime;

    [Header("���� ����")]
    [SerializeField] private bool useTrail;
    [SerializeField] private List<Trail> trails;
}
