using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Drawing;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] List<GameObject> healths;      // 체력 UI 
    [SerializeField] TMP_Text pageText;             // 페이즈 UI
    [SerializeField] TMP_Text timeText;             // 시간 UI
    [SerializeField] List<GameObject> dashStacks;   // 대쉬 UI 
    [SerializeField] GameObject fadePanel;          // 페이드 패널 게임오브젝트
    [SerializeField] Image fadeImage;               // 페이드 이미지 
    [SerializeField] GameObject optionPanel;        // 설정 창 

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

    // 체력 UI 업데이트
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

    // 현재 페이즈 UI 업데이트
    public void UpdatePageText(int pageNum)
    {
        pageText.text = $"페이즈 {pageNum.ToString()}";
    }

    // 현재 시간 UI 업데이트 
    public void UpdateTimeText(float time)
    {
        timeText.text = string.Format("{0:D2}:{1:D2}", time / 60, time % 60);
    } 

    // 대쉬 스택 UI 업데이트 
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
