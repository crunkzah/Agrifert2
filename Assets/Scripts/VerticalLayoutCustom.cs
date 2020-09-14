using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class VerticalLayoutCustom : MonoBehaviour
{

    Vector2 anchorMin           = Vector2.up;
    Vector2 anchorMax           = Vector2.up;
    Vector2 pivot               = Vector2.up;
    
    [Header("Line height(font asset):")]
    public float lineHeight = 54f;
    [Header("Space between elements")]
    public float spaceBetweenElements = 30f;

    [Header("Margin:")]
    public float topMargin      = 0f;
    public float rightMargin    = 50f;
    public float leftMargin     = 50f;
    
    [Header("Additional height for container:")]
    public float additionalContainerHeight = 100f;
    // public float lineSpacing    = 0f;

    ScrollRect sr;

    public bool updateNeeded = false;

    public void ResizeTextContainers()
    {
        // print("Resize containers.");
        TextMeshProUGUI tmp;
        for (int i = 0; i < transform.childCount; i++)
        {
            tmp = transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            
            if(tmp != null)
            {
                // print(" LineCountBefore: " + tmp.GetComponent<TextMeshProUGUI>().textInfo.lineCount.ToString());
                tmp.ForceMeshUpdate();
                // print(" LineCountAfter: " + tmp.GetComponent<TextMeshProUGUI>().textInfo.lineCount.ToString());
                Vector2 size = new Vector2 (1080f - leftMargin - rightMargin,  tmp.textInfo.lineCount * (lineHeight));
                
                
                tmp.rectTransform.sizeDelta = size;
            }
            
        }
        
    }

    void Update()
    {
        if(updateNeeded)
        {
            AlignElements();
            ResizeContainer();
            updateNeeded = false;
        }
    }
    
    

    public void AlignElements()
    {
        // print("Align elements");
        RectTransform rect;
        float currentY = 0f;

        for(int i = 0; i < transform.childCount; i++)
        {
            rect = transform.GetChild(i).GetComponent<RectTransform>();
        
            
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot     =  pivot;   


            if(hasComponent<TextMeshProUGUI>(rect))
            {
                rect.anchoredPosition = new Vector2(leftMargin, -topMargin - currentY);
            }
            else
            {
                if(hasComponent<Image>(rect))
                {
                    rect.anchoredPosition = new Vector2(0f, -topMargin - currentY);
                }
                else
                    Debug.LogError("Unrecognized ui type: " + rect.name);
            }
            
            if(i != 0)  //@Important We assume that culture image header is first and the next title is second (i.e. 'i == 1')
                currentY += rect.sizeDelta.y + spaceBetweenElements;
            else
                currentY += rect.sizeDelta.y + 0f;
        }
        
    }

    public void ResizeContainer()
    {
        // print("Resize container");
        float resizedHeight = 0f;
        for(int i = 0; i < transform.childCount; i++)
        {
            
            float height = transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
            
            resizedHeight += height + spaceBetweenElements;
        }

        Vector2 oldSizeDelta = GetComponent<RectTransform>().sizeDelta;
        
        
        //print(resizedHeight);
        GetComponent<RectTransform>().sizeDelta = new Vector2(oldSizeDelta.x, resizedHeight + additionalContainerHeight);
    }
    bool hasComponent<T>(RectTransform rect)
    {
        
        return (rect.GetComponent<T>() != null);
    }
}
