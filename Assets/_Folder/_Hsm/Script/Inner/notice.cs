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
            Debug.LogError("notice.cs�� gesipan/view�� Inspector���� �Ҵ� �� ��!");
            return;
        }
        if (hitobj == gesipan)
       {
            view.SetActive(true);
       }
    }
    public void ToggleGesipan(GameObject hitobj)
    {
        Debug.Log($"ToggleGesipan ȣ��� / hitObj: {hitobj?.name} / gesipan: {gesipan?.name} / view: {view?.name}");
        if (gesipan == null || view == null)
        {
            Debug.LogError("notice.cs:gesipan�̳� view�� �Τ�l��");
            return;
        }
        else
        {
            view.SetActive(!view.activeSelf);
            Debug.Log("ToggleGesipan");
        }
    }


}
