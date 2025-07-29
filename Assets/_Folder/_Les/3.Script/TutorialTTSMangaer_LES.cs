using UnityEngine;

// 이 스크립트가 붙은 게임오브젝트에 AudioSource 컴포넌트가 없으면 자동으로 추가해줍니다.
[RequireComponent(typeof(AudioSource))] 
public class TutorialTTSMangaer_LES : MonoBehaviour
{
    public static TutorialTTSMangaer_LES Instance = null;
    private AudioSource ttsAudioSource; // 음성 재생기

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance.gameObject); // gameObject도 함께 파괴되지 않도록 수정
            
            // AudioSource 컴포넌트를 가져옵니다.
            ttsAudioSource = GetComponent<AudioSource>(); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 오디오 클립을 재생하는 공용 메서드
    public void PlayTTS(AudioClip clip)
    {
        // 이전 소리가 재생중이면 멈추고 새로운 소리를 재생합니다.
        if (clip != null && ttsAudioSource != null)
        {
            ttsAudioSource.Stop();
            ttsAudioSource.PlayOneShot(clip);
        }
    }
}