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

    public IEnumerator ChangeScene(string name, float fadeInDuration, float fadeOutDuration)
    {
        yield return StartCoroutine(UIManager.Instance.FadeIn(fadeInDuration));

        SceneLoad(name);

        yield return StartCoroutine(UIManager.Instance.FadeOut(fadeOutDuration));
    }

    public IEnumerator ReLoadScene(float fadeInDuration, float fadeOutDuration)
    {
        yield return StartCoroutine(UIManager.Instance.FadeIn(fadeInDuration));

        SceneReload();

        yield return StartCoroutine(UIManager.Instance.FadeOut(fadeOutDuration));
    }
}
