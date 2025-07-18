using UnityEngine;

public class Ch_WetFood : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [Range(0f, -1f), SerializeField] private float upsideDownRange = -0.7f;
    [SerializeField] private Transform startPoint;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (transform.up.y < upsideDownRange)
        {

        }
        else
        {

        }
    }
}
