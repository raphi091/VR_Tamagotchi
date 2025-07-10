using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch_Throwable : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            
        }
    }
}
