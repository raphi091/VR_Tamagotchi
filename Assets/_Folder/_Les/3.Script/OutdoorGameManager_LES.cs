using UnityEngine;
using TMPro;

public class OutdoorGameManager_LES : MonoBehaviour
{
    public static OutdoorGameManager_LES Instance;

    [SerializeField] private TextMeshPro text;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        TimeManager_LES.instance.timeText = text;
        TimeManager_LES.instance.OutdoorTime();
    }
}
