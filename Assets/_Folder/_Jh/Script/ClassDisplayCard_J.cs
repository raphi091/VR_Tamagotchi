using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassDisplayCard_J : MonoBehaviour
{
    public PetDisplaySlot_J[] petSlots = new PetDisplaySlot_J[3];

    private string className;

    // 3������ �� ������ �޾� �� ���Կ� �����ϴ� �Լ�
    public void SetupCard(string className, List<PetStatusData_J> previewPets)
    {
        this.className = className;

        // 3���� ������ ��ȸ�ϸ� ���� ����
        for (int i = 0; i < petSlots.Length; i++)
        {
            // �̸������ �� �����Ͱ� ���� ����ŭ �ִ��� Ȯ��
            if (i < previewPets.Count)
            {
                // 1. �� �����ʿ��� ���� ��������
                PetProFile_LES profile = DatabaseManager_J.instance.petProfiles[previewPets[i].modelIndex];

                // 2. ������ UI(����, �̸�) ������Ʈ
                petSlots[i].UpdateSlot(profile.petPicture, previewPets[i].petName);

                // 3. ���� Ȱ��ȭ
                petSlots[i].gameObject.SetActive(true);
            }
            else
            {
                // ǥ���� �� �����Ͱ� ������ ���� ��Ȱ��ȭ
                petSlots[i].gameObject.SetActive(false);
            }
        }
    }

    public string GetClassName()
    {
        return this.className;
    }

    // ī�带 Ŭ������ �� �κ� �Ŵ������� �˸�
    private void OnMouseDown()
    {
       FindObjectOfType<LobbyManager_J>().OnSelectClass(this.className);
    }
}
