using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Ch_DryFood : XRSimpleInteractable
{
    [SerializeField] ParticleSystem particle;
    //[SerializeField] Rigidbody rb;
    private RaycastHit hit;
    private FoodTrayType foodtype;
    

    [Range(0f, -1f), SerializeField] private float upsideDownRange=-0.7f;
    [SerializeField] private Transform startPoint;

    protected override void Awake()
    {
        foodtype = FoodTrayType.Dry;
        particle = startPoint.gameObject.GetComponent<ParticleSystem>();
        //rb=GetComponent<Rigidbody>();
        base.Awake();
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
                    hit.collider.gameObject.TryGetComponent<Ch_FoodTray>(out Ch_FoodTray tray);
                }
            }
        }
        else
        {
            particle.Stop();
        }
    }
}
