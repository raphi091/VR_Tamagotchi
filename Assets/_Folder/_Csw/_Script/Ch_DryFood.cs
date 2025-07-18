using UnityEngine;

public class Ch_DryFood : MonoBehaviour, Ch_BowlFood
{
    private foodType foodtype;
    public virtual foodType FoodType { get => foodtype; set => foodtype = value; }
    public virtual bool isFillable { get; set; }

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
            particle.Play();
            //var emissionModule = particle.emission;
            //emissionModule.rateOverTimeMultiplier = 30f * Mathf.Clamp01(rb.velocity.magnitude);
            if (Physics.Raycast(startPoint.position, startPoint.up, out hit, 30f))
            {
                if (hit.collider.CompareTag("Tray"))
                {
                    hit.collider.gameObject.TryGetComponent(out FoodBowl bowl);
                    if (bowl.IsFilled == false)
                    {
                        Ch_FoodEvent.I.InvokeFoodBowlAction(bowl, this);
                    }
                }
            }
        }
        else
        {
            particle.Stop();
        }
    }
}
