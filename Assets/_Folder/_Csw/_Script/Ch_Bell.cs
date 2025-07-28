using UnityEngine;

public class Ch_Bell : MonoBehaviour
{
    private AudioSource audioSource;
    public bool ringged=false;
    private Ch_VelocityInteractable interactable;
    [SerializeField]  AudioClip bellSound;

    private void Awake()
    {
        TryGetComponent(out interactable);
        TryGetComponent(out audioSource);
        audioSource.clip = bellSound;
    }

    void Update()
    {
        if (!ringged&&interactable.velocity.magnitude>1f)
        {
            audioSource.Play();
            ringged = true;
        }
    }
}
