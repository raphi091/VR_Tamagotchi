using CustomInspector;
using UnityEngine;

public enum personalityType
{
    Grumpy, //까칠함
    Timid, //소심함
    Curious //호기심
}



[CreateAssetMenu(fileName = "NewPetPersonality", menuName = "Pet/PersonalityData")]
public class PersonalityData_LES : ScriptableObject
{
    [Header("성격종류")]
    public personalityType type;
    [Tooltip("이동 속도"), Range(0f,10f)] public float walkSpeed;
    [Tooltip("이동 반경")] public float MovingRang;

    [HorizontalLine("낯가림 수치", color: FixedColor.Cyan),HideField] public bool _b0;
    [Tooltip("이동 속도"), Range(0f,10f)] public float Shy_walkSpeed;
    [Tooltip("이동 반경")] public float Sky_MovingRang;

    [HorizontalLine("활발할 때 수치", color: FixedColor.Cyan),HideField] public bool _b1;
    [Tooltip("이동 속도"), Range(0f,10f)] public float Active_walkSpeed;
    [Tooltip("이동 반경")] public float Active_MovingRang;
}