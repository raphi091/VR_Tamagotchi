using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Report_LES : MonoBehaviour
{
    [Header("Pet Card 0")]
    public Image petImage0;
    public TextMeshProUGUI petName0;
    public TextMeshProUGUI petAge0;
    public TextMeshProUGUI petFood0;
    public Image genderIcon0;
    public Image intimacyBarBF0;
    public Image intimacyBarAT0;
    public TextMeshProUGUI intimacyIncrease0;

    [Header("Pet Card 1")]
    public Image petImage1;
    public TextMeshProUGUI petName1;
    public TextMeshProUGUI petAge1;
    public TextMeshProUGUI petFood1;
    public Image genderIcon1;
    public Image intimacyBarBF1;
    public Image intimacyBarAT1;
    public TextMeshProUGUI intimacyIncrease1;

    [Header("Pet Card 2")]
    public Image petImage2;
    public TextMeshProUGUI petName2;
    public TextMeshProUGUI petAge2;
    public TextMeshProUGUI petFood2;
    public Image genderIcon2;
    public Image intimacyBarBF2;
    public Image intimacyBarAT2;
    public TextMeshProUGUI intimacyIncrease2;

    [Header("Pet Sprites")]
    public Sprite[] petSprites; // 각 펫 모델에 대응하는 스프라이트 배열 (옵션)

    private void Start()
    {
        InitializeReport();
    }

    // 기본값은 오전 보고서 (false)
    public void InitializeReport(bool isEveningReport = false)
    {
        // DataManager에서 모든 펫 데이터 가져오기
        if (DataManager_J.instance != null && DataManager_J.instance.gameData != null)
        {
            var allPetData = DataManager_J.instance.gameData.allPetData;
            
            // 펫 데이터가 3마리인지 확인
            if (allPetData.Count >= 3)
            {
                SetPetCardData(allPetData[0], petImage0, petName0, petAge0, petFood0, genderIcon0, 
                              intimacyBarBF0, intimacyBarAT0, intimacyIncrease0, isEveningReport);
                SetPetCardData(allPetData[1], petImage1, petName1, petAge1, petFood1, genderIcon1, 
                              intimacyBarBF1, intimacyBarAT1, intimacyIncrease1, isEveningReport);
                SetPetCardData(allPetData[2], petImage2, petName2, petAge2, petFood2, genderIcon2, 
                              intimacyBarBF2, intimacyBarAT2, intimacyIncrease2, isEveningReport);
            }
            else
            {
                Debug.LogWarning("펫 데이터가 3마리보다 적습니다. 현재 펫 수: " + allPetData.Count);
            }
        }
        else
        {
            Debug.LogError("DataManager 또는 게임 데이터가 없습니다.");
        }
    }

    private void SetPetCardData(PetStatusData_J petData, Image petImage, TextMeshProUGUI nameText, 
                               TextMeshProUGUI ageText, TextMeshProUGUI foodText, Image genderIcon,
                               Image intimacyBarBefore, Image intimacyBar, TextMeshProUGUI intimacyIncreaseText,
                               bool isEveningReport)
    {
        // 펫 이미지 설정 (DatabaseManager에서 가져오기)
        if (DatabaseManager_J.instance != null && 
            DatabaseManager_J.instance.petProfiles != null && 
            petData.modelIndex < DatabaseManager_J.instance.petProfiles.Count)
        {
            petImage.sprite = DatabaseManager_J.instance.petProfiles[petData.modelIndex].petPicture;
        }
        // 백업으로 petSprites 배열 사용
        else if (petSprites != null && petData.modelIndex < petSprites.Length)
        {
            petImage.sprite = petSprites[petData.modelIndex];
        }

        // 펫 이름 설정
        nameText.text = petData.petName;

        // 펫 나이 설정
        ageText.text = "Age : " + petData.age;

        // 선호 음식 설정
        foodText.text = "Preferred Food : " + GetFoodTypeText(petData.foodType);

        // 성별 아이콘 설정
        if (petData.gender == Gender.Male)
        {
            genderIcon.sprite = DatabaseManager_J.instance.maleIcon;
        }
        else
        {
            genderIcon.sprite = DatabaseManager_J.instance.femaleIcon;
        }

        // 친밀도 바 설정 (현재 친밀도 - After)
        float currentIntimacyFillAmount = petData.intimacyper / 100f;
        intimacyBar.fillAmount = currentIntimacyFillAmount;

        // 모드에 따른 UI 설정
        if (isEveningReport)
        {
            // 저녁 보고서: Before/After 비교 표시
            intimacyBarBefore.gameObject.SetActive(true);
            
            // Before 친밀도 계산 (임시: 현재 친밀도에서 랜덤값 뺀 것)
            float beforeIntimacy = CalculateBeforeIntimacy(petData.intimacyper);
            float beforeIntimacyFillAmount = beforeIntimacy / 100f;
            intimacyBarBefore.fillAmount = beforeIntimacyFillAmount;

            // 친밀도 증가량 계산 및 표시
            float intimacyIncrease = petData.intimacyper - beforeIntimacy;
            if (intimacyIncrease > 0)
            {
                intimacyIncreaseText.text = "+" + intimacyIncrease.ToString("F1");
            }
            else if (intimacyIncrease < 0)
            {
                intimacyIncreaseText.text = intimacyIncrease.ToString("F1");
            }
            else
            {
                intimacyIncreaseText.text = "0";
            }
        }
        else
        {
            // 오전 보고서: Before 바 숨기고 증가량 텍스트 비우기
            intimacyBarBefore.gameObject.SetActive(false);
            intimacyIncreaseText.text = "";
        }
    }

    private string GetFoodTypeText(foodType foodType)
    {
        switch (foodType)
        {
            case foodType.Dry:
                return "건식";
            case foodType.Wet:
                return "습식";
            case foodType.Treat:
                return "간식";
            default:
                return "알 수 없음";
        }
    }

    private float CalculateBeforeIntimacy(float currentIntimacy)
    {
        // 임시 계산: 하루 동안 증가할 수 있는 친밀도 범위 (0~15)
        float increase = Random.Range(0f, 15f);
        float beforeIntimacy = currentIntimacy - increase;
        
        // 0 이하로 내려가지 않도록 제한
        return Mathf.Max(0f, beforeIntimacy);
    }

    // 외부에서 호출할 수 있는 보고서 업데이트 메서드
    public void UpdateReport(bool isEveningReport = false)
    {
        InitializeReport(isEveningReport);
    }

    // 오전 보고서 전용 메서드
    public void ShowMorningReport()
    {
        InitializeReport(false);
    }

    // 저녁 보고서 전용 메서드
    public void ShowEveningReport()
    {
        InitializeReport(true);
    }
}