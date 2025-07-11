using System.IO; // 파일을 다루기 위해 꼭 필요합니다.
using UnityEngine;
public class DataManager_J : MonoBehaviour
{
    // 다른 스크립트에서 쉽게 접근할 수 있도록 static 변수로 자신을 저장 (싱글톤)
    public static DataManager_J instance;

    // 현재 게임의 모든 데이터를 담고 있는 객체
    public GameData gameData;

    // 저장될 파일의 전체 경로
    private string saveFilePath;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 이 오브젝트는 파괴되지 않음
        }
        else
        {
            Destroy(gameObject);
        }

        // Application.persistentDataPath는 PC, 모바일 등 어떤 환경에서도
        // 안전하게 파일을 저장할 수 있는 경로를 알려줍니다.
        saveFilePath = Path.Combine(Application.persistentDataPath, "MyPetData.json");
    }

    // JSON 파일에서 게임 데이터를 불러오는 함수
    public void LoadGameData()
    {
        // 저장된 파일이 실제로 존재하는지 확인
        if (File.Exists(saveFilePath))
        {
            // 파일이 있다면, 파일의 모든 텍스트(JSON)를 읽어옴
            string jsonData = File.ReadAllText(saveFilePath);

            // JSON 문자열을 GameData 객체로 변환하여 gameData 변수에 덮어씀
            gameData = JsonUtility.FromJson<GameData>(jsonData);

            Debug.Log("데이터 불러오기 완료.");
        }
        else
        {
            // 저장된 파일이 없으면, 새로운 게임 데이터 객체를 생성
            Debug.Log("저장된 파일 없음. 새 게임을 시작합니다.");
            gameData = new GameData();
        }
    }

    // 현재 게임 데이터를 JSON 파일로 저장하는 함수
    public void SaveGameData()
    {
        // gameData 객체를 JSON 형식의 문자열로 변환 (true 옵션은 보기 좋게 정렬)
        string jsonData = JsonUtility.ToJson(gameData, true);

        // 변환된 JSON 문자열을 파일 경로에 텍스트 파일로 저장
        File.WriteAllText(saveFilePath, jsonData);

        Debug.Log("데이터 저장 완료: " + saveFilePath);
    }
}
