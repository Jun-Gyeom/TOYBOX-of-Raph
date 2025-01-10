using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Pattern")]
public class Pattern : ScriptableObject 
{
    public List<TileSetData> tilesets;
    public int startPatternNum;
    public float coolDownTime; 
}
