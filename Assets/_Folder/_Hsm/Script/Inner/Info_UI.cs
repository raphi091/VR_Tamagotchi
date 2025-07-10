using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Info_UI : MonoBehaviour
{
    public Image[] photos;
    public TextMeshProUGUI[] names;
    public Image[] gender;
    public Image[] H_fill;
    public Image[] P_fill;
    public Image[] F_fill;

    private void OnEnable()
    {
        PetController_J[] petControls = FindObjectsOfType<PetController_J>();

        for(int i =0; i <= petControls.Length; i++)
        {

            photos[i].sprite = DatabaseManager_J.instance.petProfiles[ petControls[i].petData.modelIndex].petPicture;             
            names[i].text = petControls[i].petData.petName;
            
            H_fill[i].fillAmount = petControls[i].petData.hungerper / 100;
            P_fill[i].fillAmount = petControls[i].petData.bowelper / 100;
            F_fill[i].fillAmount = petControls[i].petData.intimacyper / 100;

            if(petControls[i].petData.gender.Equals(Gender.Male))
            {
                this.gender[i].sprite = DatabaseManager_J.instance.maleIcon; 
            }
            else
            {
                this.gender[i].sprite = DatabaseManager_J.instance.femaleIcon;
            }
        }
    }
}
