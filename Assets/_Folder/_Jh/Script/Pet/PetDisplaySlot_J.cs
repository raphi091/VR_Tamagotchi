using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PetDisplaySlot_J : MonoBehaviour
{
    public Image petPictureUI;
    public TextMeshProUGUI nameText;

    // ������ �̸��� �޾� UI�� ������Ʈ�ϴ� �Լ�
    public void UpdateSlot(Sprite picture, string name)
    {
        petPictureUI.sprite = picture;
        nameText.text = name;
    }
}
