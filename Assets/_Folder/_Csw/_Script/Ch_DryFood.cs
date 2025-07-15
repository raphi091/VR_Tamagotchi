using UnityEngine;

public class Ch_DryFood : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    //[SerializeField] Rigidbody rb;
    private RaycastHit hit;
    private foodType foodtype;
    public foodType FoodTrayType { get => foodtype; set => foodtype = value; }
    

    [Range(0f, -1f), SerializeField] private float upsideDownRange=-0.7f;
    [SerializeField] private Transform startPoint;

    void Awake()
    {
        foodtype = foodType.Dry;
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
            if (Physics.Raycast(startPoint.position, startPoint.position + Vector3.down, out hit, 10f))
            {
                if (hit.collider.CompareTag("Tray"))
                {
                    
                }
            }
        }
        else
        {
            particle.Stop();
        }
    }
}
