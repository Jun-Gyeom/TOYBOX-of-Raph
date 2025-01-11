using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private BossManager bossManager;
    public BossManager BossManager {
        get
        {
            if (bossManager == null)
            {
                bossManager = GameObject.Find("BossManager").GetComponent<BossManager>();
            }
            return bossManager;
        }
        set
        {
            bossManager = value;
        }
    }

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
                player = GameObject.FindAnyObjectByType<Player>();
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

        // �������̴� ������ �ڷ�ƾ ����
        BossManager.StopAllCoroutines();

        TileManager.Instance.TileStateReset();
        BossManager.TrailsCloneReset();

        // �÷��̾� ������ ���� 
        GameObject.Find("PlayerGenerator").GetComponent<PlayerGenerator>().CreatePlayer();

        // ������ ����� 
        BossManager.StartPhase(currentPhase);


        yield return StartCoroutine(UIManager.Instance.FadeOut(fadeOutDuration));
    }

    public void GameClear(float fadeInDuration, float fadeOutDuration)
    {
        StartCoroutine(GameClearCoroutine(fadeInDuration, fadeOutDuration));
    }

    private IEnumerator GameClearCoroutine(float fadeInDuration, float fadeOutDuration)
    {
        yield return StartCoroutine(UIManager.Instance.FadeIn(fadeInDuration));

        // �������̴� ������ �ڷ�ƾ ����
        BossManager.StopAllCoroutines();

        TileManager.Instance.TileStateReset();
        BossManager.TrailsCloneReset();

        // Ŭ���� �г� ǥ�� 
        UIManager.Instance.ShowClearPanel();

        yield return StartCoroutine(UIManager.Instance.FadeOut(fadeOutDuration));
    }
}
