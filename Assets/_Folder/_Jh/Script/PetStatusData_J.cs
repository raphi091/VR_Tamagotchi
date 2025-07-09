using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PetStatusData_J
{
    public string petName;
    public int modelIndex;
    public int personalityIndex;
    public PersonalityData_LES data;

    public foodTepy foodTepy;
    public float intimacyper;
    public float hungerper;
    public float bowelper;

    public PetStatusData_J(int modelIdx, int personalityIdx)
    {
        this.modelIndex = modelIdx;
        this.personalityIndex = personalityIdx;
        this.petName = "¥Û¥Û¿Ã";
        data = new PersonalityData_LES
        {
            tepy = personalityTepy.None
        };
        foodTepy = foodTepy.None;

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

    public GameData()
    {
        selectedClassName = "πÃ¡§";
        allPetData = new List<PetStatusData_J>();
    }
}

