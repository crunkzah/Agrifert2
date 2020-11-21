#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotTaker : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))        
        {
            TakeScreenshot();    
        }
    }
    
    int i = 0;
    
    void TakeScreenshot()
    {
        i++;
        Debug.Log("<color=yellow>TakeScreenshot()</color>");
        ScreenCapture.CaptureScreenshot(Screen.width.ToString() + "x" + Screen.height.ToString() + "_" + i.ToString() + ".png");
    }
}
#endif