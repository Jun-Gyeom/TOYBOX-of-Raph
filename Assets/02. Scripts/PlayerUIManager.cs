using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    // UI
    [SerializeField] List<GameObject> healths;      // ü�� UI 
    [SerializeField] Text pageText;             // ������ UI
    [SerializeField] TMP_Text timeText;             // �ð� UI
    [SerializeField] List<GameObject> dashStacks;   // �뽬 UI
    [SerializeField] Image Inv_Gauge;

    #region UI

    private void Awake()
    {
        GameManager.Instance.BossManager.pud += UpdatePageText;
    }
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
        pageText.text = $"Page {pageNum.ToString()}";
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


    public void UpdateInvGaugeUI(float maxtime,float currttime)
    {
        Inv_Gauge.fillAmount = currttime / maxtime;
    }
    #endregion
}
