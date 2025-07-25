using UnityEngine;

public class Ch_DryFood : MonoBehaviour, Ch_BowlFood
{
    private foodType foodtype;
    public virtual foodType FoodType { get => foodtype; set => foodtype = value; }
    public virtual bool isFillable { get; set; }
    public virtual GameObject gameObj => gameObject;

    [SerializeField] ParticleSystem particle;
    private Ch_VelocityInteractable interactable;

    [Range(0f, -1f), SerializeField] private float upsideDownRange=-0.7f;
    [SerializeField] private Transform startPoint;
    [SerializeField] AudioClip shakeSound;
    [SerializeField] AudioClip pourSound;

    void Awake()
    {
        foodtype = foodType.Dry;
        isFillable = true;
        particle = startPoint.gameObject.GetComponent<ParticleSystem>();
        interactable = GetComponent<Ch_VelocityInteractable>();
    }

    private void Update()
    {
        if (interactable.velocity.magnitude > 0.3f)
        {
            SoundManager.Instance.PlaySFX(shakeSound);
            if (transform.up.y < upsideDownRange)
            {
                if (!particle.isPlaying)
                {
                    particle.Play();
                    SoundManager.Instance.PlaySFX(pourSound);
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
        }
    }
}
