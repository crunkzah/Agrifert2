using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

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
    
    // void Update()
    // {
    //     if(Input.GetKeyUp(KeyCode.R))
    //     {
    //         RequestWeather();
    //     }
    // }
    
    void RequestWeather()
    {
        StartCoroutine(GetWeather("55.833333", "37.616667"));
    }
    string yandexKey = "8a581eb6-cbd2-468a-9e7a-84e03a7cd648";
    
    string yandexUrl = "https://api.weather.yandex.ru/v2/forecast?";

    IEnumerator GetWeather(string lat, string lon) 
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(yandexUrl);
        sb.Append("lat="); sb.Append(lat); sb.Append("&");
        sb.Append("lon="); sb.Append(lon);// sb.Append("&");
        
        // sb.Append(yandexKey);
        
        //string request = sb.ToString();
        string request = "https://api.weather.yandex.ru/v2/forecast?lat=55.75396&lon=37.620393&extra=true";
        Debug.Log(string.Format("<color=yellow>{0}</color>", request));
        
        
        UnityWebRequest www = new UnityWebRequest();
        
        www.SetRequestHeader("X-Yandex-API-Key", yandexKey);
        Debug.Log(www.GetRequestHeader("X-Yandex-API-Key"));
        www = UnityWebRequest.Get(request);
        
      
        
        yield return www.SendWebRequest();
 
        // Debug.Log(string.Format("<color=white>RESULT: {0}</color>", www.downloadHandler.text));
            
        if(www.isNetworkError || www.isHttpError) 
        {
            Debug.Log(string.Format("<color=red>ERROR: {0}</color>", www.error));
        }
        else 
        {
            // Show results as text
            Debug.Log(string.Format("<color=white>RESULT: {0}</color>", www.downloadHandler.text));
 
            // Or retrieve results as binary data
            //byte[] results = www.downloadHandler.data;
        }
    }
    
}
