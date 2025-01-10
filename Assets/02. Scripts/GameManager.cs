using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        if (!GameObject.Find("Player").TryGetComponent(out Player))
            Debug.LogWarning("GameManager : PlayerClass - NULL");
    }

    public Player Player;
}
