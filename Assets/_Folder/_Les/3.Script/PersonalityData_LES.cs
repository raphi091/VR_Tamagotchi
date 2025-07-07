using System.Collections.Generic;
using CustomInspector;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public enum personalityTepy
{
    None,
    Grumpy, //까칠함
    Timid, //소심함
    Curious //호기심
}

public enum foodTepy
{
    None,
    Dry, // 건식
    Wet, // 습식
    Treat // 간식
}

[CreateAssetMenu(fileName = "NewPetPersonality", menuName = "Pet/PersonalityData")]
public class PersonalityData_LES : ScriptableObject
{

    [Header("성격종류")]
    public personalityTepy tepy;
    [Tooltip("친밀도")] public float intimacy;
    [Tooltip("이름")] public string name;
    [Tooltip("나이")] public int age;
    [Tooltip("성별")] public List<Image> sexuality;
    [Tooltip("먹이")] public foodTepy foodTepy;
    [Tooltip("배변활동")] public float bowel_movement;
    [Tooltip("이동 속도"), Range(0f,10f)] public float walkSpeed;
    [Tooltip("이동 반경")] public float MovingRang;

    [HorizontalLine("낯가림 수치", color: FixedColor.Cyan),HideField] public bool _b0;
    [Tooltip("이동 속도"), Range(0f,10f)] public float Shy_walkSpeed;
    [Tooltip("이동 반경")] public float Sky_MovingRang;

    [HorizontalLine("활발할 때 수치", color: FixedColor.Cyan),HideField] public bool _b1;
    [Tooltip("이동 속도"), Range(0f,10f)] public float Active_walkSpeed;
    [Tooltip("이동 반경")] public float Active_MovingRang;
    
    
}