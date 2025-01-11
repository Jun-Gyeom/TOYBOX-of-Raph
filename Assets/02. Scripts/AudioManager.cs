using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    public AudioSource bgmSource; // 배경음악용 AudioSource
    public AudioSource sfxSourcePrefab; // 효과음 재생용 AudioSource 프리팹

    [Header("Audio Clips")]
    public AudioClip[] bgmClips; // 배경음악 클립 배열
    public AudioClip[] sfxClips; // 효과음 클립 배열

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += SceneLoaded;
        AudioManager.Instance.PlayBGM(0);
    }

    public void SceneLoaded(Scene scene,LoadSceneMode mode)
    {
        Debug.Log("LOADED");
    }

    /// <summary>
    /// 배경음악 재생
    /// </summary>
    /// <param name="index">재생할 배경음악의 인덱스</param>
    public void PlayBGM(int index)
    {
        if (index < 0 || index >= bgmClips.Length)
        {
            Debug.LogWarning("BGM 인덱스가 유효하지 않습니다!");
            return;
        }

        bgmSource.clip = bgmClips[index];
        bgmSource.loop = true;
        bgmSource.Play();
    }

    /// <summary>
    /// 효과음 재생 (동시에 여러 개 가능)
    /// </summary>
    /// <param name="index">재생할 효과음의 인덱스</param>
    public void PlaySFX(int index)
    {
        if (index < 0 || index >= sfxClips.Length)
        {
            Debug.LogWarning("SFX 인덱스가 유효하지 않습니다!");
            return;
        }

        // 동적 AudioSource 생성
        AudioSource newSfxSource = Instantiate(sfxSourcePrefab, transform);
        newSfxSource.clip = sfxClips[index];
        newSfxSource.Play();

        // 오디오 클립이 끝난 후 삭제
        Destroy(newSfxSource.gameObject, newSfxSource.clip.length);
    }

    /// <summary>
    /// 배경음악 정지
    /// </summary>
    public void StopBGM()
    {
        bgmSource.Stop();
    }
}
