using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Ch_DataManager : Ch_BehaviourSingleton<Ch_DataManager>
{
    private Ch_GameData data;
    private List<Ch_IDataPersistence> dataPersistenceObjects;
    protected override bool IsDontdestroy()
    {
        return true;
    }

    private void Start()
    {
        this.dataPersistenceObjects = GetAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        data=new Ch_GameData();
    }

    public void SaveGame()
    {
        foreach (var dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(ref data);
        }
    }

    public void LoadGame()
    {
        foreach (var dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(data);
        }
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }
    
    private List<Ch_IDataPersistence> GetAllDataPersistenceObjects()
    {
        IEnumerable<Ch_IDataPersistence> dataPersistenceObject = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .OfType<Ch_IDataPersistence>();
        return new List<Ch_IDataPersistence>(dataPersistenceObject);
    }
}
