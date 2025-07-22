using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pet Profile", menuName = "Pet/Pet Profile_LES")]
public class PetProFile_LES : ScriptableObject
{
    public string petTypeName; // 펫 종류 (예: 리트리버, 푸들)
    public Sprite petPicture; // 대표 사진
    public GameObject modelPrefab; // 3D 모델링 프리팹
    public List<AudioClip> DogSound = new List<AudioClip>();
    [TextArea]
    public string description; // 펫 설명
}
