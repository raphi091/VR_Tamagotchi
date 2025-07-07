using UnityEngine;
using TMPro;

public class AffinityUIUpdater_J : MonoBehaviour
{
    public PetAffinityManager_J affinityManager;
    public TextMeshProUGUI affinityText;
    public Transform cameraTransform;

    void Update()
    {
        // �׻� ī�޶� ���ϰ�
        if (cameraTransform != null)
        {
            transform.LookAt(cameraTransform);
            transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
        }

        // �ؽ�Ʈ ����
        if (affinityManager != null && affinityText != null)
        {
            affinityText.text = $"ģ�е�: {affinityManager.affinity}";
        }
    }
}
