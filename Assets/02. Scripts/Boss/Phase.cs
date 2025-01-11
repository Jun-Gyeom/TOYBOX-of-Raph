using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Phase")]
public class Phase : ScriptableObject
{
    [Header("권능")]
    public Ability ability;

    [Header("패턴 리스트")]
    public List<Pattern> patterns;
}
