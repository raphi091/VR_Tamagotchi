using UnityEngine;

public class Ch_Bell : MonoBehaviour
{
    public bool ringged=false;
    private Ch_VelocityInteractable interactable;
    [SerializeField]  AudioClip bellSound;

    private void Awake()
    {
        TryGetComponent(out interactable);
    }

    void Update()
    {
        if (!ringged&&interactable.velocity.magnitude>1f)
        {
            ringged = true;
            SoundManager.Instance.PlaySFX(bellSound);
        }
    }
}
