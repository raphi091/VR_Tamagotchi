using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class NamePlate_K : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI petName;
    [SerializeField] private GameObject class1, class2, class3;


    private void Awake()
    {
        class1.SetActive(false);
        class2.SetActive(false);
        class3.SetActive(false);
    }

    public void Setup(string name, string classname)
    {
        petName.text = name;

        switch (classname)
        {
            case "별님반":
                class1.SetActive(true);
                break;
            case "달님반":
                class2.SetActive(true);
                break;
            case "햇님반":
                class3.SetActive(true);
                break;
        }
    }
}
