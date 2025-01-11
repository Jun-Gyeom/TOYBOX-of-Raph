using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Drawing;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] GameObject fadePanel;          // ���̵� �г� ���ӿ�����Ʈ
    [SerializeField] Image fadeImage;               // ���̵� �̹��� 
    [SerializeField] GameObject optionPanel;        // ���� â 
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

    // ȭ�� ���̵���
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

    // ȭ�� ���̵�ƿ�
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
