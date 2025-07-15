using UnityEngine;
using TMPro;

/// <summary>
/// DataManager의 현재 펫 데이터 수를 실시간으로 화면에 표시하는 디버그용 스크립트입니다.
/// </summary>
public class Debug_DataStatusDisplay : MonoBehaviour
{
    public static Debug_DataStatusDisplay instance;
    public TextMeshProUGUI statusText;

    void Awake()
    {
        // 이 디버그 HUD도 싱글톤으로 만들어 씬이 바뀌어도 파괴되지 않게 합니다.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject.transform.root.gameObject); // 캔버스 전체가 파괴되지 않도록 root를 전달
        }
        else
        {
            Destroy(gameObject.transform.root.gameObject);
        }
    }

    // 매 프레임마다 데이터 상태를 확인하고 UI 텍스트를 업데이트합니다.
    void Update()
    {
        // statusText가 할당되지 않았다면 아무것도 하지 않습니다.
        if (statusText == null) return;

        // DataManager 인스턴스가 있는지 확인합니다.
        if (DataManager_J.instance == null)
        {
            statusText.text = "Status: DataManager is NULL";
            statusText.color = Color.red;
            return;
        }

        // GameData 객체가 있는지 확인합니다.
        if (DataManager_J.instance.gameData == null)
        {
            statusText.text = "Status: GameData is NULL";
            statusText.color = Color.yellow;
            return;
        }

        // 펫 데이터 리스트가 있는지 확인합니다.
        if (DataManager_J.instance.gameData.allPetData == null)
        {
            statusText.text = "Status: Pet List is NULL";
            statusText.color = Color.yellow;
            return;
        }

        // 모든 데이터가 정상이면, 펫의 수를 표시합니다.
        int petCount = DataManager_J.instance.gameData.allPetData.Count;
        statusText.text = $"Pet Count: {petCount}";

        // 펫이 없으면 노란색, 있으면 녹색으로 표시합니다.
        statusText.color = (petCount == 0) ? Color.yellow : Color.green;
    }
}


















