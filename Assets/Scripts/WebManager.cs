using UnityEngine;

public class WebManager : MonoBehaviour
{


    //TODO
    void Awake()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        Application.targetFrameRate = 120;
#endif

#if UNITY_IOS || UNITY_ANDROID
        Application.targetFrameRate = 60;
#endif
    }

    public void OpenMainWebsite()
    {
        Application.OpenURL("http://agrifert.ru");
    }

    public void OpenInstagram()
    {
        Application.OpenURL("https://www.instagram.com/agri_fert/");
    }

    
}
