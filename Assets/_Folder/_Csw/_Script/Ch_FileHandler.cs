using System;
using System.IO;
using UnityEngine;

public class Ch_SaveFileHandler : Ch_BehaviourSingleton<Ch_SaveFileHandler>
{
    protected override bool IsDontdestroy()
    {
        return true;
    }

    private string dirPath=Application.persistentDataPath;
    [SerializeField] private string fileName="";
    private string fullpath;

    private void Start()
    {
        fullpath=Path.Combine(dirPath,fileName);
    }

    public void SaveData(Ch_GameData data)
    {
        
        Directory.CreateDirectory((Path.GetDirectoryName(fullpath)));
        
        string dataTostore=JsonUtility.ToJson(data, true);

        using (FileStream stream = new FileStream(fullpath, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(dataTostore);
            }
        }
        
    }

    public Ch_GameData LoadData()
    {
        Ch_GameData loadedData = null;

        if (File.Exists(fullpath))
        {
            try
            {
                string datatoLoad = "";
                using (FileStream stream = new FileStream(fullpath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        datatoLoad = reader.ReadToEnd();
                    }
                }
                loadedData = JsonUtility.FromJson<Ch_GameData>(datatoLoad);
            }
            catch (Exception e)
            {
                Debug.Log("Error occured on"+fullpath+"\n"+e.Message);
            }
        }

        return loadedData;
    }
}
