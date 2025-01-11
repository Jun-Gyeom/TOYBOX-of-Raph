using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    public bool IsActive { get; set; }
    [SerializeField] private List<DialogData> dialogs;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Image characterImage;
    [SerializeField] private Text dialogueText;
    [SerializeField] private float typingSpeed = 0.1f;

    private int currentDialogIndex = -1;
    private bool isTypingEffect = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsActive)
        {
            NextDialogue();
        }
    }

    public void StartDialogue()
    {
        currentDialogIndex = 0;

        IsActive = true;
        dialoguePanel.SetActive(true);
        characterImage.sprite = dialogs[currentDialogIndex].characterImage.sprite;

        NextDialogue();
    }

    public void NextDialogue()
    {
        if (currentDialogIndex >= dialogs.Count)
        {
            EndDialogue();
            return;
        }
        StartCoroutine(TypeText(dialogs[currentDialogIndex].dialogue));
    }

    private void EndDialogue()
    {
        IsActive = false;
        StopAllCoroutines();
        dialoguePanel.SetActive(false);

        // 다음 페이즈로 
        BossManager.Instance.NextPhase();
    }

    private IEnumerator TypeText(string message)
    {
        if (isTypingEffect)
        {
            StopAllCoroutines();
            dialogueText.text = message; // 전체 텍스트 바로 출력
            isTypingEffect = false; // 타이핑 종료
            currentDialogIndex++;
            yield break;
        }

        isTypingEffect = true;

        // 한 글자씩 출력
        for (int i = 0; i <= message.Length; i++)
        {
            string currentText = message.Substring(0, i); // i번째까지의 텍스트 잘라내기
            dialogueText.text = currentText;              // 텍스트 업데이트
            yield return new WaitForSeconds(typingSpeed); // 타이핑 속도만큼 대기
        }

        isTypingEffect = false;
        currentDialogIndex++;
    }

    [System.Serializable]
    public struct DialogData
    {
        public Image characterImage;                // 캐릭터 이미지
        [TextArea(3, 5)] public string dialogue;    // 대사	
    }
}
