using UnityEngine;

public class Ch_DryFood : MonoBehaviour, Ch_BowlFood
{
    private foodType foodtype;
    public virtual foodType FoodType { get => foodtype; set => foodtype = value; }
    public virtual bool isFillable { get; set; }
    public virtual GameObject gameObj => gameObject;

    [SerializeField] ParticleSystem particle;
    //[SerializeField] Rigidbody rb;
    private RaycastHit hit;

    [Range(0f, -1f), SerializeField] private float upsideDownRange=-0.7f;
    [SerializeField] private Transform startPoint;

    void Awake()
    {
        foodtype = foodType.Dry;
        isFillable = true;
        particle = startPoint.gameObject.GetComponent<ParticleSystem>();
        //rb=GetComponent<Rigidbody>();
    }

    private void Update()
    {
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
}
