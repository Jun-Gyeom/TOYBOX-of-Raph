using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    // UI
    [SerializeField] List<GameObject> healths;      // 체력 UI 
    [SerializeField] Text pageText;             // 페이즈 UI
    [SerializeField] TMP_Text timeText;             // 시간 UI
    [SerializeField] List<GameObject> dashStacks;   // 대쉬 UI
    [SerializeField] Image Inv_Gauge;

    #region UI

    private void Awake()
    {
        GameManager.Instance.BossManager.pud += UpdatePageText;
    }
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
        pageText.text = $"Page {pageNum.ToString()}";
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


    public void UpdateInvGaugeUI(float maxtime,float currttime)
    {
        Inv_Gauge.fillAmount = currttime / maxtime;
    }
    #endregion
}
