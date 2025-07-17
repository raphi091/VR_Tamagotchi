using UnityEngine;
using UnityEngine.InputSystem;

public class Ch_ContollerVelocity : MonoBehaviour
{
    public InputActionProperty velocityProperty;
    
    public Vector3 velocity;

    private void FixedUpdate()
    {
        velocity=velocityProperty.action.ReadValue<Vector3>();
    }
}
