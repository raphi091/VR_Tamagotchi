using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    IEnumerator Start()
    {
        if( TimeManager_LES.instance.debug)
        {
            yield return new WaitForSeconds(5f);
            GameManager.instance.GoToScene("H_Outdoor");
        }
    }

}
