using UnityEngine;
public class GameManager : MonoBehaviour
{
    // 예시: 반에 들어갈 펫의 수
    private const int PETS_PER_CLASS = 3;

    void Start()
    {
        // 게임 시작 시 데이터 불러오기
        DataManager_J.instance.LoadGameData();

        // 만약 저장된 데이터가 없다면 (처음 시작)
        if (DataManager_J.instance.gameData.allPetData.Count == 0)
        {
            Debug.Log("저장된 데이터 없음, 반 선택 UI 활성화!");
            // 여기서 반 선택 UI를 보여주는 로직을 실행합니다.
        }
        else
        {
            // 저장된 데이터가 있다면 해당 데이터로 펫들을 생성합니다.
            InstantiateSavedPets();
        }
    }

    // UI 버튼에 연결할 함수
    public void OnClassSelected(string className)
    {
        Debug.Log(className + " 선택됨! 펫을 랜덤으로 생성합니다.");
        DataManager_J.instance.gameData.selectedClassName = className;

        // 반에 들어갈 펫들을 랜덤으로 생성
        for (int i = 0; i < PETS_PER_CLASS; i++)
        {
            // 모델과 성격을 랜덤으로 선택
            int randomModelIndex = Random.Range(0, DatabaseManager_J.instance.allModels.Count);
            int randomPersonalityIndex = Random.Range(0, DatabaseManager_J.instance.personalities.Count);

            // 데이터 생성 후 DataManager에 추가
            PetStatusData_J newPet = new PetStatusData_J(randomModelIndex, randomPersonalityIndex);

            // 이름 랜덤으로 선택
            int randomNameIndex = Random.Range(0, DatabaseManager_J.instance.PetNames.Count);
            newPet.petName = DatabaseManager_J.instance.PetNames[randomNameIndex];

            DataManager_J.instance.gameData.allPetData.Add(newPet);
        }

        // 펫들을 씬에 실제로 생성
        InstantiateSavedPets();
    }

    // 저장된 데이터를 바탕으로 펫 오브젝트들을 씬에 생성하는 함수
    void InstantiateSavedPets()
    {
        foreach (PetStatusData_J petData in DataManager_J.instance.gameData.allPetData)
        {
            // 데이터에 맞는 모델 프리팹 가져오기
            GameObject petPrefab = DatabaseManager_J.instance.allModels[petData.modelIndex];
            // 펫 생성
            GameObject petInstance = Instantiate(petPrefab, Vector3.zero, Quaternion.identity);
            // 여기에 펫의 이름, 성격 정보 등을 petInstance에 넘겨주는 로직 추가
        }
    }


    // 하루가 지났을 때 호출될 함수
    public void EndOfDay()
    {
        Debug.Log("하루가 종료되어 게임을 자동 저장합니다.");

        // 현재 펫들의 수치를 gameData에 업데이트하는 로직이 필요합니다.
        // 예: for (int i=0; i < allPets.Count; i++) {
        //         DataManager.instance.gameData.allPetData[i].hunger_level = allPets[i].currentHunger;
        //     }

        // 업데이트된 데이터로 저장 실행
        DataManager_J.instance.SaveGameData();
    }
}