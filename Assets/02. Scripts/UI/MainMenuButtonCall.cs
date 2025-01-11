using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtonCall : MonoBehaviour
{
    [SerializeField] GameObject fadePanel;          // 페이드 패널 게임오브젝트
    [SerializeField] Image fadeImage;               // 페이드 이미지 
    public void SceneLoad(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void _ChangeScene(string name)
    {
        StartCoroutine(ChangeScene(name, 2, 2));
    }
    public IEnumerator ChangeScene(string name, float fadeInDuration, float fadeOutDuration)
    {
        yield return StartCoroutine(UIManager.Instance.FadeIn(fadeInDuration));

        SceneLoad(name);

        yield return StartCoroutine(UIManager.Instance.FadeOut(fadeOutDuration));
    }

    public void Quit()
    {
        Application.Quit();
    }
}
