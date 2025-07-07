using UnityEngine;

public class PetAffinityManager_J : MonoBehaviour
{
    [Range(0, 100)] public int affinity = 0;

    // ģ�е� ����
    public void IncreaseAffinity(int amount)
    {
        affinity = Mathf.Clamp(affinity + amount, 0, 100);
    }

    // ģ�е� �ܰ� (0~4)
    public int GetAffinityLevel()
    {
        if (affinity >= 100) return 4;
        if (affinity >= 80) return 3;
        if (affinity >= 40) return 2;
        if (affinity >= 20) return 1;
        return 0;
    }
}
