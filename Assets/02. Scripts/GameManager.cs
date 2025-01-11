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

    public GameObject cameraGameObject;
    public Player Player;

    protected override void Awake()
    {
        base.Awake();

        if (!GameObject.Find("Player").TryGetComponent(out Player))
            Debug.LogWarning("GameManager : PlayerClass - NULL");

        if (!GameObject.Find("Main Camera"))
        {
            Debug.LogWarning("GameManager : Main Camera - NULL");
        }
        else
        {
            cameraGameObject = GameObject.Find("Main Camera");
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
        Vector3 initPos = cameraGameObject.transform.position;
        while (timer > 0)
        {
            timer -= 0.05f;
            cameraGameObject.transform.position = Random.insideUnitSphere * cameraShakePower + initPos;
            yield return new WaitForSeconds(0.05f);
        }
        cameraGameObject.transform.position = initPos;

        isShaking = false;
    }
}
