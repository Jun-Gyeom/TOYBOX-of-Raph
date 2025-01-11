using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        if (!GameObject.Find("Player").TryGetComponent(out Player))
            Debug.LogWarning("GameManager : PlayerClass - NULL");
    }

    public Player Player;

    public void SceneLoad(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void SceneReload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
