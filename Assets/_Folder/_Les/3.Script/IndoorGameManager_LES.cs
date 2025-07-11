using System;
using UnityEngine;
using TMPro;

public class IndoorGameManager_LES : MonoBehaviour
{
    public static IndoorGameManager_LES Instance;

    [SerializeField] private TextMeshProUGUI text;

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
        TimeManager_LES.instance.time = text;
        TimeManager_LES.instance.IndoorTime();
    }
}
