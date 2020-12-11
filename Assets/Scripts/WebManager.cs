using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;

[System.Serializable]
public struct City
{
    public float lat;
    public float lon;
    public int id;
    public City(float _lat, float _lon, int _id)
    {
        lat = _lat;
        lon = _lon;
        id = _id;
    }
}

public class CustomCertificateHandler : CertificateHandler
{
    // Encoded RSAPublicKey
    private static readonly string PUB_KEY = "";


    /// <summary>
    /// Validate the Certificate Against the Amazon public Cert
    /// </summary>
    /// <param name="certificateData">Certifcate to validate</param>
    /// <returns></returns>
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}

public class WebManager : MonoBehaviour
{
    
    public Dictionary<string, City> cities = new Dictionary<string, City>();
    

    //TODO
    void Awake()
    {
        cities.Add("Краснодар", new City(45f, 38f, -1));

        Application.targetFrameRate = 60;
    }

    public void OpenMainWebsite()
    {
        Application.OpenURL("https://agrifert.ru");
    }

    public void OpenInstagram()
    {
        Application.OpenURL("https://www.instagram.com/agri_fert/");
    }
    
    void Update()
    {
        // if(Input.GetKeyUp(KeyCode.R))
        // {
        //     RequestWeather();
        // }
    }
    
    string key = "4cb03038-2c22-11eb-a5a9-0242ac130002-4cb030ce-2c22-11eb-a5a9-0242ac130002";
    
    void RequestWeather()
    {
        StartCoroutine(GetWeather("45.02", "38.59"));
    }
   

    IEnumerator GetWeather(string lat, string lon) 
    {
        StringBuilder sb = new StringBuilder();
        //sb.Append(string.Format("api.openweathermap.org/data/2.5/forecast/daily?lat={0}&lon={1}&cnt=1&appid={2}", lat, lon, owKey));
        sb.Append("https://api.stormglass.io/v2/weather/point?");
        sb.Append("lat=");sb.Append(lat);
        sb.Append("&lng=");sb.Append(lon);
        sb.Append("&params=");sb.Append("waveHeight,airTemperature");
        
        string request = sb.ToString();
        Debug.Log(string.Format("<color=yellow>{0}</color>", request));
        
 

        
        
        UnityWebRequest www = new UnityWebRequest();
        // CustomCertificateHandler  certHandler = new CustomCertificateHandler();
        // www.certificateHandler = certHandler();
        www.SetRequestHeader("Authorization", key);
        
        www = UnityWebRequest.Get(request);
        
        yield return www.SendWebRequest();
            
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
