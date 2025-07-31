using System;
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
    public TextMeshProUGUI className;

    public void SetupPetCard()
    {
        for (int i = 0; i < GameManager.instance.petsInScene.Count; i++)
        {
            petImages[i].sprite = DatabaseManager_J.instance.petProfiles[GameManager.instance.petsInScene[i].petData.modelIndex].petPicture;
            petNames[i].text = GameManager.instance.petsInScene[i].petData.petName;

            float imacy = (float)Math.Truncate(GameManager.instance.petsInScene[i].petData.intimacyper);
            if (imacy >= 100f)
                imacy = 100f;
            else if (imacy <= 0f)
                imacy = 0f;

            if (GameManager.instance.petsInScene[i].petData.foodType.Equals(foodType.Dry))
                petFoodandImacys[i].text = $"{imacy}\n건식";
            else if (GameManager.instance.petsInScene[i].petData.foodType.Equals(foodType.Wet))
                petFoodandImacys[i].text = $"{imacy}\n습식";
            else
                petFoodandImacys[i].text = $"{imacy}\n생식";

            if (GameManager.instance.petsInScene[i].petData.gender.Equals(Gender.Male))
                genderIcons[i].sprite = DatabaseManager_J.instance.maleIcon;
            else
                genderIcons[i].sprite = DatabaseManager_J.instance.femaleIcon;
        }

        className.text = DataManager_J.instance.gameData.selectedClassName;
    }
}