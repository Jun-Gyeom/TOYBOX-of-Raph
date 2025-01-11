using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    public AudioSource bgmSource; // ������ǿ� AudioSource
    public AudioSource sfxSourcePrefab; // ȿ���� ����� AudioSource ������

    [Header("Audio Clips")]
    public AudioClip[] bgmClips; // ������� Ŭ�� �迭
    public AudioClip[] sfxClips; // ȿ���� Ŭ�� �迭

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
    /// ������� ���
    /// </summary>
    /// <param name="index">����� ��������� �ε���</param>
    public void PlayBGM(int index)
    {
        if (index < 0 || index >= bgmClips.Length)
        {
            Debug.LogWarning("BGM �ε����� ��ȿ���� �ʽ��ϴ�!");
            return;
        }

        bgmSource.clip = bgmClips[index];
        bgmSource.loop = true;
        bgmSource.Play();
    }

    /// <summary>
    /// ȿ���� ��� (���ÿ� ���� �� ����)
    /// </summary>
    /// <param name="index">����� ȿ������ �ε���</param>
    public void PlaySFX(int index)
    {
        if (index < 0 || index >= sfxClips.Length)
        {
            Debug.LogWarning("SFX �ε����� ��ȿ���� �ʽ��ϴ�!");
            return;
        }

        // ���� AudioSource ����
        AudioSource newSfxSource = Instantiate(sfxSourcePrefab, transform);
        newSfxSource.clip = sfxClips[index];
        newSfxSource.Play();

        // ����� Ŭ���� ���� �� ����
        Destroy(newSfxSource.gameObject, newSfxSource.clip.length);
    }

    /// <summary>
    /// ������� ����
    /// </summary>
    public void StopBGM()
    {
        bgmSource.Stop();
    }
}
