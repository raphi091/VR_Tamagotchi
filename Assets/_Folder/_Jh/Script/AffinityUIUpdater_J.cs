using UnityEngine;
using TMPro;

public class AffinityUIUpdater_J : MonoBehaviour
{
    public PetAffinityManager_J affinityManager;
    public TextMeshProUGUI affinityText;
    public Transform cameraTransform;

    void Update()
    {
        // 항상 카메라를 향하게
        if (cameraTransform != null)
        {
            transform.LookAt(cameraTransform);
            transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
        }

        // 텍스트 갱신
        if (affinityManager != null && affinityText != null)
        {
            affinityText.text = $"친밀도: {affinityManager.affinity}";
        }
    }
}
