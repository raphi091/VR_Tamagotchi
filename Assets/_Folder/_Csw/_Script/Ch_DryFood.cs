using UnityEngine;

public class Ch_DryFood : MonoBehaviour, Ch_BowlFood
{
    private foodType foodtype;
    public virtual foodType FoodType { get => foodtype; set => foodtype = value; }
    public virtual bool isFillable { get; set; }
    public virtual GameObject gameObj => gameObject;
    
    private Ch_VelocityInteractable interactable;
    private ParticleSystem particle;
    private AudioSource audioSource;

    [Range(0f, -1f), SerializeField] private float upsideDownRange=-0.7f;
    [SerializeField] private Transform startPoint;
    [SerializeField] AudioClip shakeSound;
    
    public AudioClip FoodSound
    {
        get => foodSound;

        set => foodSound = value;
    }
    [SerializeField] AudioClip foodSound;

    void Awake()
    {
        foodtype = foodType.Dry;
        isFillable = true;
        particle = startPoint.gameObject.GetComponent<ParticleSystem>();
        TryGetComponent(out interactable);
        TryGetComponent(out audioSource);
        audioSource.clip = shakeSound;
    }

    private void Update()
    {
        if (interactable.velocity.magnitude > 0.08f)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            if (transform.up.y < upsideDownRange)
            {
                if (!particle.isPlaying)
                {
                    particle.Play();
                }
            }
            else
            {
                particle.Stop();
            }
        }
        else
        {
            particle.Stop();
            audioSource.Stop();
        }
    }
}
