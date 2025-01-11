using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Drawing;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] List<GameObject> healths;      // ü�� UI 
    [SerializeField] TMP_Text pageText;             // ������ UI
    [SerializeField] TMP_Text timeText;             // �ð� UI
    [SerializeField] List<GameObject> dashStacks;   // �뽬 UI 
    [SerializeField] GameObject fadePanel;          // ���̵� �г� ���ӿ�����Ʈ
    [SerializeField] Image fadeImage;               // ���̵� �̹��� 
    [SerializeField] GameObject optionPanel;        // ���� â 

    #region Main Menu
    public void GameStart()
    {
        StartCoroutine(GameManager.Instance.ChangeScene("02. Game", 2f, 2f));
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

    // ü�� UI ������Ʈ
    public void UpdateHealthUI(int amount)
    {
        for (int i = 0; i < healths.Count; i++)
        {
            if (i < amount)
            {
                healths[i].SetActive(true);
            }
            else
            {
                healths[i].SetActive(false);
            }
        }
    }

    // ���� ������ UI ������Ʈ
    public void UpdatePageText(int pageNum)
    {
        pageText.text = $"������ {pageNum.ToString()}";
    }

    // ���� �ð� UI ������Ʈ 
    public void UpdateTimeText(float time)
    {
        timeText.text = string.Format("{0:D2}:{1:D2}", time / 60, time % 60);
    } 

    // �뽬 ���� UI ������Ʈ 
    public void UpdateDashUI(int amount)
    {
        for (int i = 0; i < dashStacks.Count; i++)
        {
            if (i < amount)
            {
                dashStacks[i].SetActive(true);
            }
            else
            {
                dashStacks[i].SetActive(false);
            }
        }
    }

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
