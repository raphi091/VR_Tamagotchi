using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassDisplayCard_J : MonoBehaviour
{
    public PetDisplaySlot_J[] petSlots = new PetDisplaySlot_J[3];

    private string className;

    // 3마리의 펫 정보를 받아 각 슬롯에 전달하는 함수
    public void SetupCard(string className, List<PetStatusData_J> previewPets)
    {
        this.className = className;

        // 3개의 슬롯을 순회하며 정보 설정
        for (int i = 0; i < petSlots.Length; i++)
        {
            // 미리보기용 펫 데이터가 슬롯 수만큼 있는지 확인
            if (i < previewPets.Count)
            {
                // 1. 펫 프로필에서 사진 가져오기
                PetProFile_LES profile = DatabaseManager_J.instance.petProfiles[previewPets[i].modelIndex];

                // 2. 슬롯의 UI(사진, 이름) 업데이트
                petSlots[i].UpdateSlot(profile.petPicture, previewPets[i].petName);

                // 3. 슬롯 활성화
                petSlots[i].gameObject.SetActive(true);
            }
            else
            {
                // 표시할 펫 데이터가 없으면 슬롯 비활성화
                petSlots[i].gameObject.SetActive(false);
            }
        }
    }

    public string GetClassName()
    {
        return this.className;
    }

    // 카드를 클릭했을 때 로비 매니저에게 알림
    private void OnMouseDown()
    {
       FindObjectOfType<LobbyManager_J>().OnSelectClass(this.className);
    }
}
