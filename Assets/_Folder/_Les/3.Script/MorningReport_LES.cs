using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MorningReport_LES : MonoBehaviour
{
    [Header("Pet Card 1")]
    public Image petImage1;
    public TextMeshProUGUI petName1;
    public TextMeshProUGUI petAge1;
    public TextMeshProUGUI petFood1;
    public Image genderIcon1;
    public Image intimacyBar1;
    public TextMeshProUGUI intimacyIncrease1;

    [Header("Pet Card 2")]
    public Image petImage2;
    public TextMeshProUGUI petName2;
    public TextMeshProUGUI petAge2;
    public TextMeshProUGUI petFood2;
    public Image genderIcon2;
    public Image intimacyBar2;
    public TextMeshProUGUI intimacyIncrease2;

    [Header("Pet Card 3")]
    public Image petImage3;
    public TextMeshProUGUI petName3;
    public TextMeshProUGUI petAge3;
    public TextMeshProUGUI petFood3;
    public Image genderIcon3;
    public Image intimacyBar3;
    public TextMeshProUGUI intimacyIncrease3;

    [Header("Pet Sprites")]
    public Sprite[] petSprites; // 각 펫 모델에 대응하는 스프라이트 배열 (옵션)

    private void Start()
    {
        InitializeReport();
    }

    public void InitializeReport()
    {
        // DataManager에서 모든 펫 데이터 가져오기
        if (DataManager_J.instance != null && DataManager_J.instance.gameData != null)
        {
            var allPetData = DataManager_J.instance.gameData.allPetData;
            
            // 펫 데이터가 3마리인지 확인
            if (allPetData.Count >= 3)
            {
                SetPetCardData(allPetData[0], petImage1, petName1, petAge1, petFood1, genderIcon1, intimacyBar1, intimacyIncrease1);
                SetPetCardData(allPetData[1], petImage2, petName2, petAge2, petFood2, genderIcon2, intimacyBar2, intimacyIncrease2);
                SetPetCardData(allPetData[2], petImage3, petName3, petAge3, petFood3, genderIcon3, intimacyBar3, intimacyIncrease3);
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
                               Image intimacyBar, TextMeshProUGUI intimacyIncreaseText)
    {
        // DatabaseManager의 petProfiles에서 스프라이트 가져오기 시도
        if (DataManager_J.instance != null && 
            DataManager_J.instance.gameData != null && 
            petData.modelIndex < DatabaseManager_J.instance.petProfiles.Count)
        {
            // PetProFile_LES에 스프라이트 필드가 있다면 사용
            var petProfile = DatabaseManager_J.instance.petProfiles[petData.modelIndex];
            // petProfile.petSprite 등의 필드가 있다면 사용
            // petImage.sprite = petProfile.petSprite;
            
            // 현재 PetProFile_LES 구조를 모르므로 임시로 petSprites 배열 사용
            if (petSprites != null && petData.modelIndex < petSprites.Length)
            {
                petImage.sprite = petSprites[petData.modelIndex];
            }
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

        // 친밀도 바 설정 (0~100 범위를 0~1로 변환)
        float intimacyFillAmount = petData.intimacyper / 100f;
        intimacyBar.fillAmount = intimacyFillAmount;

        // 친밀도 증가량 계산 및 표시
        float intimacyIncrease = CalculateIntimacyIncrease(petData);
        if (intimacyIncrease > 0)
        {
            intimacyIncreaseText.text = "+" + intimacyIncrease.ToString("F1");
        }
        else
        {
            intimacyIncreaseText.text = intimacyIncrease.ToString("F1");
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

    private float CalculateIntimacyIncrease(PetStatusData_J petData)
    {
        // 임시로 어제 친밀도를 현재 친밀도에서 임의값을 뺀 것으로 계산
        // 실제로는 이전 날의 데이터와 비교해야 함
        // 예: 어제 데이터가 별도로 저장되어 있다면 그것과 비교
        
        // 임시 계산 (실제 구현 시 수정 필요)
        float yesterdayIntimacy = petData.intimacyper - Random.Range(0f, 10f);
        return petData.intimacyper - yesterdayIntimacy;
    }

    // 외부에서 호출할 수 있는 보고서 업데이트 메서드
    public void UpdateReport()
    {
        InitializeReport();
    }
}