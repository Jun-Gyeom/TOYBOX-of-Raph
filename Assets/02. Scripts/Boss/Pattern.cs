using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO/Pattern")]
public class Pattern : ScriptableObject 
{
    [SerializeField] private List<TileSetData> tilesets;
    [SerializeField] private int startPatternNum;
    [SerializeField] private float coolDownTime; 
}
