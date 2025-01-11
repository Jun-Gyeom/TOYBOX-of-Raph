using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    public GameObject playerPrefab;

    private void Awake()
    {
        CreatePlayer();
    }

    public void CreatePlayer()
    {
        Instantiate(playerPrefab).name = "Player";
    }
}
