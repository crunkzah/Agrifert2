using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using TMPro;


public class Authorization : MonoBehaviour
{
    Dictionary<string, string> logPassPair = new Dictionary<string, string>();
    
    
    public bool SignedIn = false;
    
    public TMP_InputField loginInputField;
    public TMP_InputField passInputField;
    
    public TextMeshProUGUI login_msg;
    
    

    string currentLogin = "";
    string currentPass = "";
    
    void ReadLogin()
    {
        currentLogin = loginInputField.text.ToLower();    
        
    }
    
    void ReadPass()
    {
        currentPass = passInputField.text;    
    }
    
    void Awake()
    {
        //logPassPair.Add("admin",  "dx3C5CGW");
        logPassPair.Add("admin",  "admin");
    }
    
    void ReadPrefs()
    {
        int isSignedInInt = PlayerPrefs.GetInt("isSignedInInt", -1);
        
        currentLogin  = PlayerPrefs.GetString("login", "Логин");
        
        SignedIn = isSignedInInt == 1 ? true : false;
    }
    
    public void ClearPassField()
    {
        passInputField.SetTextWithoutNotify(string.Empty);
    }
    
    void SetPrefsAsLoggedIn()
    {
        PlayerPrefs.SetInt("isSignedInInt", 1);
        PlayerPrefs.SetString("login", currentLogin);
    }
    
    void SetPrefsAsLoggedOut()
    {
        PlayerPrefs.SetInt("isSignedInInt", -1);
    }
    
    void ResetToInitial()
    {
        Debug.Log("ResetToInitial()");
        currentLogin = string.Empty;
        currentPass = string.Empty;
        
        HideLoginMessage();
        
        loginInputField.SetTextWithoutNotify(string.Empty);
        passInputField.SetTextWithoutNotify(string.Empty);
        loginButton_tmp.SetText("Войти");
        loginButton_img.color = new Color(47f/255f, 128f/255f, 54f/255f);
        proceedButton_obj.SetActive(false);
    }
    
    
    public TextMeshProUGUI loginButton_tmp;
    public Image loginButton_img;
    public GameObject proceedButton_obj;
    
    void OnEnable()
    {
        ClearPassField();
        ReadPrefs();
        
        if(!SignedIn)
            ResetToInitial();
        else
        {
            OnLoginSuccess();
            
        }
    }
    
    public void LoginButton()
    {
        if(!SignedIn)
        {
            ReadLogin();
            ReadPass();
            
            Login(currentLogin, currentPass);
        }
        else
        {
            Logout();
            proceedButton_obj.SetActive(false);
        }
    }
    
    void Logout()
    {
        ResetToInitial();
        SetPrefsAsLoggedOut();
        ReadPrefs();
    }
    
    public void HideLoginMessage()
    {
        login_msg.transform.parent.gameObject.SetActive(false);
    }
    
    public void SayLoginMessage(string msg)
    {
        login_msg.transform.parent.gameObject.SetActive(true);
        login_msg.SetText(msg);
    }
    
    public void Login(string login, string pass)
    {
        Debug.Log(string.Format("Trying to login as <color=yellow>{0}</color>", login));
        Debug.Log(string.Format("With password <color=green>{0}</color>", pass));
        
        if(logPassPair.ContainsKey(login))
        {
            if(logPassPair[login] == pass)
            {
                Debug.Log("<color=green>SUCCESS</color>");
                OnLoginSuccess();
            }
            else
            {
                SayLoginMessage("<color=red>Неправильный пароль!</color>");
                
                Debug.Log("<color=red>Invalid pass</color>");
            }
        }
        else
        {
            SayLoginMessage("<color=red>Неправильный логин</color>");
            Debug.Log("<color=red>Invalid login</color>");
        }
    }
    
    
    void OnLoginSuccess()
    {
        SetPrefsAsLoggedIn();
        
        loginInputField.SetTextWithoutNotify(currentLogin);
        proceedButton_obj.SetActive(true);
        SayLoginMessage("<color=#2F8036>Вы вошли!</color>");
        loginButton_tmp.SetText("Выйти");
        loginButton_img.color = new Color(1f, 140F/255F, 120F/255F);
    }
}
