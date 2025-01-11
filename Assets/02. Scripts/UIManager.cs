using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Drawing;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] GameObject fadePanel;          // 페이드 패널 게임오브젝트
    [SerializeField] Image fadeImage;               // 페이드 이미지 
    [SerializeField] GameObject optionPanel;        // 설정 창 
    [SerializeField] GameObject PausePanel;

    #region Main Menu
    public void GameStart()
    {
        StartCoroutine(GameManager.Instance.ChangeScene("02. Game", 2f, 2f));
        AudioManager.Instance.StopBGM();
    }

    public void ShowOption()
    {
        optionPanel.SetActive(true);
    }

    public void HideOption()
    {
        optionPanel.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
    #endregion

    // 화면 페이드인
    public IEnumerator FadeIn(float duration)
    {
        fadePanel.SetActive(true);

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            UnityEngine.Color color = fadeImage.color;
            color.a = Mathf.Clamp01(timer / duration);
            fadeImage.color = color;
            yield return null;
        }
    }

    // 화면 페이드아웃
    public IEnumerator FadeOut(float duration)
    {

        float timer = duration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            UnityEngine.Color color = fadeImage.color;
            color.a = Mathf.Clamp01(timer / duration);
            fadeImage.color = color;
            yield return null;
        }

        fadePanel.SetActive(false);
    }
}
