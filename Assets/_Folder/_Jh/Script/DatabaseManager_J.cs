using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager_J : MonoBehaviour
{
    public static DatabaseManager_J instance;

    
    public List<PetProFile_LES> petProfiles;
    public List<PersonalityData_LES> personalities;
    public List<string> PetNames;
    public Sprite maleIcon;
    public Sprite femaleIcon;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
