using UnityEngine;

public class Ch_Bell : MonoBehaviour
{
    public bool ringged=false;
    private Ch_VelocityInteractable interactable;

    private void Awake()
    {
        TryGetComponent(out interactable);
    }

    void Update()
    {
        if (!ringged&&interactable.velocity.magnitude>2f)
        {
            ringged = true;
        }
    }
}
