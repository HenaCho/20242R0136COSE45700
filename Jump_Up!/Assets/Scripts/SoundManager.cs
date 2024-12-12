using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField]
    private AudioSource bgmSource; // BGM을 위한 AudioSource
    [SerializeField]
    private AudioSource sfxSource; // 효과음을 위한 AudioSource

    [Header("Audio Clips")]
    [SerializeField]
    private Dictionary<string, AudioClip> bgmDict = new Dictionary<string, AudioClip>();
    [SerializeField]
    private AudioClip[] sfxClips; // 효과음 Clips

    static public SoundManager instance;
    private float volume = 0.2f;

    void Awake()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        bgmDict.Add("GameScene", Resources.Load<AudioClip>("BGM/under the rainbow"));
        bgmDict.Add("TitleScene", Resources.Load<AudioClip>("BGM/Hurt_and_heart"));
    }

    // BGM을 재생하는 함수
    public void PlayBGM(string name)
    {
        bgmSource.clip = bgmDict[name];
        bgmSource.loop = true;
        bgmSource.volume = volume;
        bgmSource.Play();
    }

    // BGM을 멈추는 함수
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // 효과음을 재생하는 함수
    public void PlaySFX(int index)
    {
        sfxSource.volume = volume;
        if (index >= 0 && index < sfxClips.Length)
        {
            sfxSource.PlayOneShot(sfxClips[index]);
        }
    }
}
