using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingUIPlacer : MonoBehaviour
{
    public GameObject buttonPrefab;
    public int buttonCount = 6;
    public float radius = 0.2f;


    private void Start()
    {
        for(int i =0; i < buttonCount; i++)
        {
            float angle = i * Mathf.PI * 2f / buttonCount;
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            GameObject btn = Instantiate(buttonPrefab, transform);
            btn.transform.localPosition = pos;
            btn.transform.LookAt(transform.position + transform.forward);  // 정면을 향하게
        }
    }
}
