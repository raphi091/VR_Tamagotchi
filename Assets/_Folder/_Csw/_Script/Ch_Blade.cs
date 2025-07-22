using UnityEngine;
using UnityEngine.Events;

public class Ch_Blade : MonoBehaviour
{
    public UnityEvent<Transform> onSliceHit=new UnityEvent<Transform>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Slice"))
        {
            onSliceHit.Invoke(other.transform);
        }
    }
}
