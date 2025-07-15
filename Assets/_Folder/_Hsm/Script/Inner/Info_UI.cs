using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Info_UI : MonoBehaviour
{
    // 내부에서 강아지 UI 묶어줄 클래스
    [System.Serializable]
    private class PetCardUI
    {
        [Header("UI 연결")]
        public Image photo;
        public TextMeshProUGUI nameText;
        public Image genderIcon;
        public Image hungerFill;
        public Image bowelFill;
        public Image intimacyFill;

        // 부모 오브젝트를 통째로 켜고 끄기 위한 참조
        public GameObject cardObject;
    }

    // [SerializeField]를 사용하면 private 변수도 인스펙터 창에 노출됩니다.
    // 여기에 3개의 PetCard UI 요소들을 직접 드래그 앤 드롭으로 연결합니다.
    [SerializeField]
    private List<PetCardUI> petCardUIs = new List<PetCardUI>();

    private void OnEnable()
    {
        Refresh();
    }

    // 이제 InitializeUI() 함수는 필요 없으므로 삭제합니다.

    public void Refresh()
    {
        // 데이터가 없으면 모든 카드를 숨깁니다.
        if (DataManager_J.instance == null || DataManager_J.instance.gameData == null || DataManager_J.instance.gameData.allPetData == null || DataManager_J.instance.gameData.allPetData.Count == 0)
        {
            Debug.LogWarning("강아지 데이터가 없습니다. 모든 UI 카드를 숨깁니다.");
            foreach (var card in petCardUIs)
            {
                card.cardObject.SetActive(false);
            }
            return;
        }

        var petList = DataManager_J.instance.gameData.allPetData;

        // petCardUIs 리스트를 기준으로 반복
        for (int i = 0; i < petCardUIs.Count; i++)
        {
            // 만약 데이터가 UI 카드보다 적으면 남는 카드는 숨깁니다.
            if (i < petList.Count)
            {
                var data = petList[i];
                var ui = petCardUIs[i];

                ui.cardObject.SetActive(true); // 카드를 다시 보이게 함

                // 각 필드가 null이 아닌지 확인하고 할당 (더욱 안전)
                if (ui.photo != null) ui.photo.sprite = DatabaseManager_J.instance.petProfiles[data.modelIndex].petPicture;
                if (ui.nameText != null) ui.nameText.text = data.petName;
                if (ui.genderIcon != null) ui.genderIcon.sprite = data.gender == Gender.Male ? DatabaseManager_J.instance.maleIcon : DatabaseManager_J.instance.femaleIcon;
                if (ui.hungerFill != null) ui.hungerFill.fillAmount = data.hungerper / 100f;
                if (ui.bowelFill != null) ui.bowelFill.fillAmount = data.bowelper / 100f;
                if (ui.intimacyFill != null) ui.intimacyFill.fillAmount = data.intimacyper / 100f;
            }
            else
            {
                petCardUIs[i].cardObject.SetActive(false);
            }
        }
    }
}