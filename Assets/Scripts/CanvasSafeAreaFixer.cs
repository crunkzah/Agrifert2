using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSafeAreaFixer : MonoBehaviour
{
    Canvas canvas;
    
    void Awake()
    {
        canvas = GetComponent<Canvas>();
        RectTransform rect = canvas.GetComponent<RectTransform>();
        
        float ySize_offset = Screen.safeArea.height - Screen.height;
        
        Debug.Log("ySize_offset: <color=yellow>" + ySize_offset + "</color>");
        
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, ySize_offset);
    }    
}
