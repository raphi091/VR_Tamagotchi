using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ch_TreatFood:MonoBehaviour
{
    public List<Transform> slices=new List<Transform>();
    public Ch_CuttedFood cuttedFood;

    void Awake()
    {
        if (slices.Count==0)
        {
            slices = GetComponentsInChildren<Transform>().Where(t=>t!=this.transform).ToList();
        }
    }
    

    public void OnCutted()
    {
        if (slices.Count == 0)
        {
            cuttedFood.OnCutted(this);
        }
    }
}
