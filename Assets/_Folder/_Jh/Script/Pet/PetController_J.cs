using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PetController_J : MonoBehaviour
{
    public PetStatusData_J petData;

    [Header("�ð��� ���")]
    // �𵨸� �������� ������ ������ ��ġ (�ڽ� ������Ʈ)
    public GameObject petModelSlot;
    // �̸�ǥ UI (TextMeshProUGUI �Ǵ� UI.Text)
    public TextMeshProUGUI nameText;

    [Header("���� ���� (�ǽð� ���氪)")]
    // ���� �÷��� �߿� ��� ���ϴ� ���� ��ġ��
    public float currentHunger;
    public float currentIntimacy;
    public float currentBowel;

    // GameManager�� ȣ���� ������ ���� �Լ�
    public void ApplyData(PetStatusData_J data)
    {
        // 1. ���޹��� �����͸� �� ��Ʈ�ѷ��� ����
        this.petData = data;

        // 2. �𵨸� ����
        // ���� ������ ������ ���� �ִٸ� ���� �ı�
        if (petModelSlot.transform.childCount > 0)
        {
            Destroy(petModelSlot.transform.GetChild(0).gameObject);
        }

        // DatabaseManager���� �����Ϳ� �´� �� '������'�� ������
        GameObject modelPrefab = DatabaseManager_J.instance.petProfiles[data.modelIndex].modelPrefab;
        // ������ �������� petModelSlot�� �ڽ����� '����(Instantiate)'
        Instantiate(modelPrefab, petModelSlot.transform);

        // 3. �̸� ����
        // ���ӿ�����Ʈ�� �̸��� �� �̸����� �����ϸ� ������ ����
        this.name = data.petName;
        if (nameText != null)
        {
            nameText.text = data.petName; // UI �ؽ�Ʈ ����
        }

        // 4. ����� ��ġ�� ���� ���� ������ ���� (���� ���� ��)
        this.currentHunger = data.hungerper;
        this.currentIntimacy = data.intimacyper;
        this.currentBowel = data.bowelper;

        DogFSM_K fsm = GetComponent<DogFSM_K>();
        Transform mouth = modelPrefab.transform.Find("_MOUSE_");
        if(mouth != null)
        {
            fsm.mouthTransform = mouth;
        }
        else
        {
            Debug.LogWarning("_MOUSE_ Ʈ�������� ã�� �� �����ϴ�. ������� ����");
        }

        // 5. ��� ������ �������� ������Ʈ�� Ȱ��ȭ
        this.gameObject.SetActive(true);

        //TEMP
        GetComponent<DogFSM_K>().cubeRenderer = GetComponentInChildren<Renderer>();
        //TEMP
    }
}
