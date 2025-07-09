using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notice : MonoBehaviour
{
    public GameObject gesipan;
    public GameObject view;

    private void Start()
    {
        view.SetActive(false);  
    }

    public void viewGesipan(GameObject hitobj)
    {
        if (gesipan == null || view == null)
        {
            Debug.LogError("notice.cs의 gesipan/view가 Inspector에서 할당 안 됨!");
            return;
        }
        if (hitobj == gesipan)
       {
            view.SetActive(true);
       }
    }
    public void ToggleGesipan(GameObject hitobj)
    {
        Debug.Log($"ToggleGesipan 호출됨 / hitObj: {hitobj?.name} / gesipan: {gesipan?.name} / view: {view?.name}");
        if (gesipan == null || view == null)
        {
            Debug.LogError("notice.cs:gesipan이나 view가 널ㅇlㅁ");
            return;
        }
        else
        {
            view.SetActive(!view.activeSelf);
            Debug.Log("ToggleGesipan");
        }
    }


}
