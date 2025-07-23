using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public enum BGMTrackName
{
    None = 0,
    Lobby,
    Tutorial,
    Indoor,
    Lunch,
    Outdoor
}

[System.Serializable]
public class MusicTrack
{
    public BGMTrackName trackName;
    public AudioClip audioClip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance = null;

    [Header("BGM")]
    [SerializeField] private List<MusicTrack> musicTracks;
    [SerializeField] private AudioMixerGroup bgmMixerGroup;
    [SerializeField] private float crossfadeDuration = 1.5f;

    [Header("SFX")]
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    private AudioSource bgmA;
    private AudioSource bgmB;
    private bool isPlaying = true;
    private AudioSource sfxSource;

    private Dictionary<BGMTrackName, AudioClip> musicClipDict;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeSound();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSound()
    {
        bgmA = gameObject.AddComponent<AudioSource>();
        bgmB = gameObject.AddComponent<AudioSource>();
        bgmA.outputAudioMixerGroup = bgmMixerGroup;
        bgmB.outputAudioMixerGroup = bgmMixerGroup;
        bgmA.playOnAwake = false;
        bgmB.playOnAwake = false;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.outputAudioMixerGroup = sfxMixerGroup;
        sfxSource.playOnAwake = false;

        musicClipDict = new Dictionary<BGMTrackName, AudioClip>();
        foreach (var track in musicTracks)
        {
            musicClipDict[track.trackName] = track.audioClip;
        }
    }

    public void PlayBGM(BGMTrackName trackName, bool loop = true)
    {
        if (!musicClipDict.ContainsKey(trackName) || musicClipDict[trackName] == null)
        {
            return;
        }

        AudioClip clipToPlay = musicClipDict[trackName];
        AudioSource currentSource = isPlaying ? bgmA : bgmB;

        if (currentSource.isPlaying && currentSource.clip == clipToPlay)
        {
            return;
        }

        StopAllCoroutines();
        StartCoroutine(Crossfade(clipToPlay, loop));
    }

    private IEnumerator Crossfade(AudioClip newClip, bool loop)
    {
        AudioSource activeSource = isPlaying ? bgmA : bgmB;
        AudioSource inactiveSource = isPlaying ? bgmB : bgmA;

        inactiveSource.clip = newClip;
        inactiveSource.loop = loop;
        inactiveSource.volume = 0;
        inactiveSource.Play();

        float timer = 0f;
        while (timer < crossfadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(timer / crossfadeDuration);

            activeSource.volume = Mathf.Lerp(1, 0, progress);
            inactiveSource.volume = Mathf.Lerp(0, 1, progress);

            yield return new WaitForEndOfFrame();
        }

        activeSource.Stop();
        activeSource.clip = null;
        inactiveSource.volume = 1;

        isPlaying = !isPlaying;
    }


    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}