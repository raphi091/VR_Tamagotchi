using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource uiSource; // UI 전용 AudioSource

    [Header("BGM Clips")]
    [SerializeField] private AudioClip lobbyBGM;
    [SerializeField] private AudioClip tutorialBGM;
    [SerializeField] private AudioClip indoorBGM;
    [SerializeField] private AudioClip lunchBGM;
    [SerializeField] private AudioClip outdoorBGM;

    [Header("UI SFX")]
    [SerializeField] private AudioClip buttonClickSFX;
    [SerializeField] private AudioClip SelectingbuttonSFX;

    [Header("Dog SFX")]
    [SerializeField] private AudioClip dogBark1;
    [SerializeField] private AudioClip dogBark2;
    [SerializeField] private AudioClip dogBark3;
    [SerializeField] private AudioClip dogBark4;
    [SerializeField] private AudioClip dogBark5;
    [SerializeField] private AudioClip dogEat1;
    [SerializeField] private AudioClip dogEat2;
    [SerializeField] private AudioClip dogEat3;
    [SerializeField] private AudioClip dogRunIndoor;
    [SerializeField] private AudioClip dogRunOutdoor;

    [Header("Environment SFX")]
    [SerializeField] private AudioClip frontDoor;
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip dryFoodShake;
    [SerializeField] private AudioClip dryFoodGive;
    [SerializeField] private AudioClip handBell;
    [SerializeField] private AudioClip playerWalkIndoor;
    [SerializeField] private AudioClip playerWalkOutdoor;
    [SerializeField] private AudioClip outsideAmbient;
    [SerializeField] private AudioClip reportPick;
    [SerializeField] private AudioClip throwObject;
    [SerializeField] private AudioClip sceneTransition;
    [SerializeField] private AudioClip cutTing;
    [SerializeField] private AudioClip giveMeat;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float bgmVolume = 0.7f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float uiVolume = 1f;

    [Header("Mute Settings")]
    private bool isBGMMuted = false;
    private bool isSFXMuted = false;
    private float previousBGMVolume;
    private float previousSFXVolume;

    private Dictionary<string, AudioClip> bgmDict;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        //싱글톤 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
            InitializeBGMDictionary();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudioSources()
    {
        //AudioSource가 없으면 생성
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.playOnAwake = false;
            bgmSource.loop = true;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
        }

        if (uiSource == null)
        {
            uiSource = gameObject.AddComponent<AudioSource>();
            uiSource.playOnAwake = false;
        }

        UpdateVolume();
    }

    private void InitializeBGMDictionary()
    {
        bgmDict = new Dictionary<string, AudioClip>
        {
            {"H_Lobby", lobbyBGM},
            //{"TutorialScene", tutorialBGM},
            {"H_Indoor", indoorBGM},
            {"H_Lunch", lunchBGM},
            {"H_Outdoor", outdoorBGM}
        };
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //씬에 맞는 BGM 재생
        PlaySceneBGM(scene.name);

        //버튼에 클릭 사운드 자동 추가
        StartCoroutine(AddButtonSoundsDelayed());
    }

    private IEnumerator AddButtonSoundsDelayed()
    {
        //씬 로드 직후 버튼들이 생성되기를 기다림
        yield return new WaitForEndOfFrame();

        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button btn in buttons)
        {
            btn.onClick.RemoveListener(PlayButtonClick);
            btn.onClick.AddListener(PlayButtonClick);
        }
    }

    //특정 부모 오브젝트 하위의 버튼들에 사운드 추가
    public void AddButtonSoundsToParent(GameObject parent)
    {
        if (parent == null) return;

        Button[] buttons = parent.GetComponentsInChildren<Button>(true);
        foreach (Button btn in buttons)
        {
            btn.onClick.RemoveListener(PlayButtonClick);
            btn.onClick.AddListener(PlayButtonClick);
        }
    }

    #region BGM Methods

    private void PlaySceneBGM(string sceneName)
    {
        if (bgmDict.TryGetValue(sceneName, out AudioClip clip))
        {
            if (clip != null && bgmSource.clip != clip)
            {
                if (fadeCoroutine != null)
                    StopCoroutine(fadeCoroutine);

                fadeCoroutine = StartCoroutine(FadeBGM(clip));
            }
        }
    }

    private IEnumerator FadeBGM(AudioClip newClip, float fadeDuration = 0.5f)
    {
        //Fade out
        float startVolume = bgmSource.volume;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        //Change clip
        bgmSource.Stop();
        bgmSource.clip = newClip;
        bgmSource.Play();

        //Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(0, bgmVolume * masterVolume, t / fadeDuration);
            yield return null;
        }

        bgmSource.volume = bgmVolume * masterVolume;
    }

    // 개별 BGM 재생 메서드들
    public void PlayLobbyBGM()
    {
        if (lobbyBGM != null)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeBGM(lobbyBGM));
        }
    }

    public void PlayTutorialBGM()
    {
        if (tutorialBGM != null)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeBGM(tutorialBGM));
        }
    }

    public void PlayIndoorBGM()
    {
        if (indoorBGM != null)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeBGM(indoorBGM));
        }
    }

    public void PlayLunchBGM()
    {
        if (lunchBGM != null)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeBGM(lunchBGM));
        }
    }

    public void PlayOutdoorBGM()
    {
        if (outdoorBGM != null)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeBGM(outdoorBGM));
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PauseBGM()
    {
        bgmSource.Pause();
    }

    public void ResumeBGM()
    {
        bgmSource.UnPause();
    }

    #endregion

    #region SFX Methods

    //UI 사운드
    public void PlayButtonClick()
    {
        PlayUISound(buttonClickSFX);
    }

    public void playSelectingButton()
    {
        PlayUISound(SelectingbuttonSFX);
    }

    // 강아지 사운드 메서드들
    public void PlayDogBark1()
    {
        PlaySFX(dogBark1);
    }

    public void PlayDogBark2()
    {
        PlaySFX(dogBark2);
    }

    public void PlayDogBark3()
    {
        PlaySFX(dogBark3);
    }

    public void PlayDogBark4()
    {
        PlaySFX(dogBark4);
    }

    public void PlayDogBark5()
    {
        PlaySFX(dogBark5);
    }

    public void PlayDogEat1()
    {
        PlaySFX(dogEat1);
    }

    public void PlayDogEat2()
    {
        PlaySFX(dogEat2);
    }

    public void PlayDogEat3()
    {
        PlaySFX(dogEat3);
    }

    public void PlayDogRunIndoor()
    {
        PlaySFX(dogRunIndoor);
    }

    public void PlayDogRunOutdoor()
    {
        PlaySFX(dogRunOutdoor);
    }

    // 환경음 메서드들
    public void PlayDoorOpen()
    {
        PlaySFX(doorOpen);
    }

    public void PlayfrontDoorOpen()
    {
        PlaySFX(frontDoor);
    }

    public void PlayDryFoodShake()
    {
        PlaySFX(dryFoodShake);
    }

    public void PlayDryFoodGive()
    {
        PlaySFX(dryFoodGive);
    }

    public void PlayHandBell()
    {
        PlaySFX(handBell);
    }

    public void PlayPlayerWalkIndoor()
    {
        PlaySFX(playerWalkIndoor);
    }

    public void PlayPlayerWalkOutdoor()
    {
        PlaySFX(playerWalkOutdoor);
    }

    public void PlayOutsideAmbient()
    {
        PlaySFX(outsideAmbient);
    }

    public void PlayReportPick()
    {
        PlaySFX(reportPick);
    }

    public void PlayThrowObject()
    {
        PlaySFX(throwObject);
    }

    public void PlaySceneTransition()
    {
        PlaySFX(sceneTransition);
    }

    public void PlayCutting()
    {
        PlaySFX(cutTing);
    }

    public void PlayGiveMeat()
    {
        PlaySFX(giveMeat);
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null && !isSFXMuted)
        {
            sfxSource.PlayOneShot(clip, sfxVolume * masterVolume);
        }
    }

    private void PlayUISound(AudioClip clip)
    {
        if (clip != null && uiSource != null && !isSFXMuted)
        {
            uiSource.PlayOneShot(clip, uiVolume * masterVolume);
        }
    }

    // 3D 위치에서 사운드 재생
    public void PlaySFXAtPosition(AudioClip clip, Vector3 position)
    {
        if (clip != null && !isSFXMuted)
        {
            AudioSource.PlayClipAtPoint(clip, position, sfxVolume * masterVolume);
        }
    }

    #endregion

    #region Mute/Unmute Methods

    public void SetBGMOn()
    {
        UnmuteBGM();
    }

    public void SetBGMOff()
    {
        MuteBGM();
    }

    public void SetSFXOn()
    {
        UnmuteSFX();
    }

    public void SetSFXOff()
    {
        MuteSFX();
    }

    public void MuteBGM()
    {
        if (!isBGMMuted)
        {
            previousBGMVolume = bgmVolume;
            bgmVolume = 0f;
            isBGMMuted = true;
            UpdateVolume();
            SaveMuteSettings();
        }
    }

    public void UnmuteBGM()
    {
        if (isBGMMuted)
        {
            bgmVolume = previousBGMVolume > 0 ? previousBGMVolume : 0.7f;
            isBGMMuted = false;
            UpdateVolume();
            SaveMuteSettings();
        }
    }

    public void MuteSFX()
    {
        if (!isSFXMuted)
        {
            previousSFXVolume = sfxVolume;
            sfxVolume = 0f;
            isSFXMuted = true;
            UpdateVolume();
            SaveMuteSettings();
        }
    }

    public void UnmuteSFX()
    {
        if (isSFXMuted)
        {
            sfxVolume = previousSFXVolume > 0 ? previousSFXVolume : 1f;
            isSFXMuted = false;
            UpdateVolume();
            SaveMuteSettings();
        }
    }

    public bool IsBGMMuted() => isBGMMuted;
    public bool IsSFXMuted() => isSFXMuted;

    #endregion

    #region Volume Control

    public void UpdateVolume()
    {
        if (bgmSource != null)
            bgmSource.volume = bgmVolume * masterVolume;
        if (sfxSource != null)
            sfxSource.volume = sfxVolume * masterVolume;
        if (uiSource != null)
            uiSource.volume = uiVolume * masterVolume;
    }

    public void SetMasterVolume(float value)
    {
        masterVolume = Mathf.Clamp01(value);
        UpdateVolume();
        SaveVolumeSettings();
    }

    public void SetBGMVolume(float value)
    {
        bgmVolume = Mathf.Clamp01(value);
        UpdateVolume();
        SaveVolumeSettings();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
        UpdateVolume();
        SaveVolumeSettings();
    }

    private void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetFloat("UIVolume", uiVolume);
        PlayerPrefs.Save();
    }

    private void LoadVolumeSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0.7f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        uiVolume = PlayerPrefs.GetFloat("UIVolume", 1f);
        UpdateVolume();
    }

    private void SaveMuteSettings()
    {
        PlayerPrefs.SetInt("BGMMuted", isBGMMuted ? 1 : 0);
        PlayerPrefs.SetInt("SFXMuted", isSFXMuted ? 1 : 0);
        PlayerPrefs.SetFloat("PreviousBGMVolume", previousBGMVolume);
        PlayerPrefs.SetFloat("PreviousSFXVolume", previousSFXVolume);
        PlayerPrefs.Save();
    }

    private void LoadMuteSettings()
    {
        isBGMMuted = PlayerPrefs.GetInt("BGMMuted", 0) == 1;
        isSFXMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
        previousBGMVolume = PlayerPrefs.GetFloat("PreviousBGMVolume", 0.7f);
        previousSFXVolume = PlayerPrefs.GetFloat("PreviousSFXVolume", 1f);

        if (isBGMMuted)
            bgmVolume = 0f;
        if (isSFXMuted)
            sfxVolume = 0f;
    }

    #endregion

    private void Start()
    {
        LoadVolumeSettings();
        LoadMuteSettings();
        UpdateVolume();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}