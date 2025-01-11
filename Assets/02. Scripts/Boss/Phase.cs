using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Phase")]
public class Phase : ScriptableObject
{
    [Header("�Ǵ�")]
    public Ability ability;

    [Header("���� ����Ʈ")]
    public List<Pattern> patterns;
}
