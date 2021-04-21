using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageLabel : MonoBehaviour
{
    TextMeshProUGUI tmp;
    Image img;
    
    public const float fadeTime_default = 2.5F;
    
    float fadeTime = 2.5F;
    
    
    void Awake()
    {
        img = GetComponent<Image>();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    void Start()
    {
        img.color = SetAlpha(img.color, 0);
        tmp.color = SetAlpha(tmp.color, 0);
    }
    
    Color SetAlpha(Color col, float alpha)
    {
        Color Result = col;
        Result.a = alpha;
        return Result;
    }
    
    public void ShowMessage(string msg, float time = fadeTime_default)
    {
        fadeTime = time;
        this.gameObject.SetActive(true);
        
        tmp.SetText(msg);
        v1 = v2 = 0f;
        img.color = SetAlpha(img.color, 1f);
        tmp.color = SetAlpha(tmp.color, 1f);
    }
    
    float v1, v2;
    
    void Update()
    {
        if(img.color.a > 0)
        {
            float a = img.color.a;
            a = Mathf.SmoothDamp(a, 0, ref v1, fadeTime);
            
            img.color = SetAlpha(img.color, a);
            tmp.color = SetAlpha(tmp.color, a);
        }
        else
        {
            img.color = SetAlpha(img.color, 0);
            tmp.color = SetAlpha(tmp.color, 0);
        }
    }
}
