using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScrollRectFixResetter : MonoBehaviour
{
    ScrollRect_fix sr_fix;

    void OnEnable()
    {
        if (sr_fix == null)
            sr_fix = GetComponent<ScrollRect_fix>();

        sr_fix.verticalNormalizedPosition = 1f;
    }

}
