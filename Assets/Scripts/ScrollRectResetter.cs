using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScrollRectResetter : MonoBehaviour
{
    ScrollRect sr;

    void OnEnable()
    {
        if(sr == null)
            sr = GetComponent<ScrollRect>();

        sr.verticalNormalizedPosition = 1f;
    }
    
}
