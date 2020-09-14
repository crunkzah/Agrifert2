using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PanelResizer : MonoBehaviour
{
    public RectTransform children;

    public Vector2 size;
    RectTransform rect;
    public float totalHeight;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        totalHeight = 0f;
        foreach(Transform child in transform)
        {
           totalHeight += child.GetComponent<RectTransform>().sizeDelta.y;
        }
        Vector2 sizeD = rect.sizeDelta;
        rect.sizeDelta = new Vector2(sizeD.x, totalHeight);
     
    }

    void Update()
    {
        size = rect.sizeDelta;
    }
}
