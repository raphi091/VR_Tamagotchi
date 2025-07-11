using UnityEngine;

public class BillboardFaceCamera : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // 메인 카메라 방향 바라보기
        Vector3 dirToCam = transform.position - cam.transform.position;
        transform.rotation = Quaternion.LookRotation(dirToCam);
    }
    void LateUpdate()
    {
        if (cam == null) return;

        // 180도 회전 (글자가 거꾸로일 경우)
        transform.Rotate(0, 180f, 0);
    }
}
