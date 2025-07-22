using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Report_K : MonoBehaviour
{
    [Header("Pet Card")]
    public List<Image> petImages;
    public List<TextMeshProUGUI> petNames;
    public List<TextMeshProUGUI> petFoodandImacys;
    public List<Image> genderIcons;

    public void SetupPetCard()
    {
        for (int i = 0; i <= GameManager.instance.petsInScene.Count; i++)
        {
            petImages[i].sprite = DatabaseManager_J.instance.petProfiles[GameManager.instance.petsInScene[i].petData.modelIndex].petPicture;
            petNames[i].text = GameManager.instance.petsInScene[i].petData.petName;

            if (GameManager.instance.petsInScene[i].petData.foodType.Equals(foodType.Dry))
                petFoodandImacys[i].text = $"{GameManager.instance.petsInScene[i].petData.intimacyper}\n건식";
            else if (GameManager.instance.petsInScene[i].petData.foodType.Equals(foodType.Wet))
                petFoodandImacys[i].text = $"{GameManager.instance.petsInScene[i].petData.intimacyper}\n습식";
            else
                petFoodandImacys[i].text = $"{GameManager.instance.petsInScene[i].petData.intimacyper}\n생식";

            if (GameManager.instance.petsInScene[i].petData.gender.Equals(Gender.Male))
                genderIcons[i].sprite = DatabaseManager_J.instance.maleIcon;
            else
                genderIcons[i].sprite = DatabaseManager_J.instance.femaleIcon;
        }
    }
}