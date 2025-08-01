using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class checkBoard : MonoBehaviour
{
    public PetController_J petController_J;

    public Image[] photo =  new Image[3];
    public TextMeshProUGUI[] DogName = new TextMeshProUGUI[3];
    public Image[] Hungry_status =  new Image[3];
    public Image[] Poopy_status =  new Image[3];
    public Image[] intimacy_status =  new Image[3];


    void Update()
    {
        Status();
    }


    public void Status()
    {

        for (int i = 0; i < GameManager.instance.petsInScene.Count; i++)
        {
            photo[i].sprite =
                DatabaseManager_J.instance.petProfiles[GameManager.instance.petsInScene[i].petData.modelIndex].petPicture;

            DogName[i].text =
                GameManager.instance.petsInScene[i].petData.petName;

            Hungry_status[i].fillAmount =
                GameManager.instance.petsInScene[i].currentHunger / 100f;

            Poopy_status[i].fillAmount =
                GameManager.instance.petsInScene[i].currentBowel / 100f;

            float imacy = (float)Math.Truncate(GameManager.instance.petsInScene[i].currentIntimacy);
            if (imacy >= 100f)
                imacy = 100f;
            else if (imacy <= 0f)
                imacy = 0f;

            intimacy_status[i].fillAmount =
                imacy / 100f;
        }

    }

}
