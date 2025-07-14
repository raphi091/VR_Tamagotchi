using UnityEngine;

public class BillboardFaceCamera : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (cam == null) return;

        Vector3 direction = cam.transform.position - transform.position;
        direction.y = 0f; // 수직 방향은 무시하고 수평으로만 회전

        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(-direction.normalized);
        }
    }
}
