using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private float cameraShakePower;
    private float cameraShakeDuration;
    private bool isShaking;

    private Player player;
    private GameObject cameraGameObject;
    public GameObject CameraGameObject
    {
        get
        {
            if (cameraGameObject == null)
            {
                cameraGameObject = GameObject.Find("Main Camera");
            }
            return cameraGameObject;
        }
        set
        {
            cameraGameObject = value;
        }
    }
    public Player Player
    {
        get
        {
            if (player == null)
            {
                player = GameObject.Find("Player").GetComponent<Player>();
            }
            return player;
        }
        set
        {
            player = value;
        }
    }

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
    public void ShakeCamera(float power, float duration)
    {
        if (isShaking) return;
        cameraShakePower = power;
        cameraShakeDuration = duration;
        StartCoroutine(ShakeCameraCoroutine());
    }
    private IEnumerator ShakeCameraCoroutine()
    {
        isShaking = true;

        float timer = cameraShakeDuration;
        Vector3 initPos = CameraGameObject.transform.position;
        while (timer > 0)
        {
            timer -= 0.05f;
            CameraGameObject.transform.position = Random.insideUnitSphere * cameraShakePower + initPos;
            yield return new WaitForSeconds(0.05f);
        }
        CameraGameObject.transform.position = initPos;

        isShaking = false;
    }

    public void GameOver(float fadeInDuration, float fadeOutDuration, int currentPhase)
    {
        StartCoroutine(GameOverCoroutine(fadeInDuration, fadeOutDuration, currentPhase));
    }

    private IEnumerator GameOverCoroutine(float fadeInDuration, float fadeOutDuration, int currentPhase)
    {
        yield return StartCoroutine(UIManager.Instance.FadeIn(fadeInDuration));

        BossManager.Instance.StartPhase(currentPhase);

        yield return StartCoroutine(UIManager.Instance.FadeOut(fadeOutDuration));
    }

    public void GameClear()
    {

    }
}
