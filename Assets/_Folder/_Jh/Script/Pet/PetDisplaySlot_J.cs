using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PetDisplaySlot_J : MonoBehaviour
{
    public Image petPictureUI;
    public TextMeshProUGUI nameText;

    // 사진과 이름을 받아 UI를 업데이트하는 함수
    public void UpdateSlot(Sprite picture, string name)
    {
        petPictureUI.sprite = picture;
        nameText.text = name;
    }
}
