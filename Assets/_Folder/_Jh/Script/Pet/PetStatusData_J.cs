using UnityEngine;
using System.Collections.Generic;

public enum Gender { Male, Female }
public enum foodType
{
    Dry, // 건식
    Wet, // 습식
    Treat, // 생식
    None
}

[System.Serializable]
public class PetStatusData_J
{
    public string petName;
    public int modelIndex;
    public int personalityIndex;

    public Gender gender;
    public int age;

    public foodType foodType;
    public float intimacyper;
    public float hungerper;
    public float bowelper;

    public PetStatusData_J(int modelIdx, int personalityIdx)
    {
        this.modelIndex = modelIdx;
        this.personalityIndex = personalityIdx;
        this.petName = "댕댕이";

        this.gender = (Gender)Random.Range(0, 2);
        this.age = Random.Range(1, 4);
        this.foodType = (foodType)Random.Range(0, 3);

        this.intimacyper = 0f;
        this.hungerper = 80f;
        this.bowelper = 100f;
    }
}

[System.Serializable]
public class GameData
{
    public string selectedClassName;
    public List<PetStatusData_J> allPetData = new List<PetStatusData_J>();
    public int Day;
    public bool tutorial;

    public GameData()
    {
        selectedClassName = "미정";
        allPetData = new List<PetStatusData_J>();
        Day = 1;
        tutorial = false;
    }
}

[System.Serializable]
public class GameSetting
{
    public float masterVolume;
    public float bgmVolume;
    public float sfxVolume;

    public GameSetting()
    {
        masterVolume = 1f;
        bgmVolume = 1f;
        sfxVolume = 1f;
    }
}

