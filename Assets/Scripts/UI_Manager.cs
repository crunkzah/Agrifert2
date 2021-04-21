using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class UI_Manager : MonoBehaviour
{
    [Header("Message holder:")]
    public MessageLabel message;
    
    static UI_Manager _instance;
    
    public static UI_Manager Singleton()
    {
        if(_instance == null)
        {
            _instance = FindObjectOfType<UI_Manager>();
        }
        
        return _instance;
    }
    
    public static void ShowMessage(string msg, float msg_fadeTime = msg_fadeTime_default)
    {
       Singleton()._ShowMessage(msg, msg_fadeTime);
    }
    
    const float msg_fadeTime_default = MessageLabel.fadeTime_default;
    
    void _ShowMessage(string msg, float msg_fadeTime = msg_fadeTime_default)
    {
        message.ShowMessage(msg, msg_fadeTime);
    }
    
    [Header("Objects in footer:")]
    public GameObject back_button;
    public GameObject website_button;
    public GameObject instagram_button;
    public GameObject phone_number;
    

    public string previousMenu = "";
    [Header("Pages:")]
    public GameObject[] pages;
    [Header("Background image:")]
    public GameObject bg_image;

    public string startPage = "CultureSingleCanvas";

    [Header("Tweening settings:")]
    public float timeToAnimate = 0.5f;
    public float maxSpeed = 400f;
    public AnimationCurve curve;

    [Header("Runtime vars:")]
    public GameObject currentPage;
    public GameObject pageToSwitch;
    public bool isWorking = false;
    
    Stack<GameObject> pageStack = new Stack<GameObject>();
    public int screenWidth = 0;

    //TODO Don't watch at screen size, watch at canvas target resolution (1080x1920)

    public Vector2 targetResolution = new Vector2(1080, 1920);

    public void DialPhoneNumber()
    {
        Application.OpenURL("tel://[+79625590001]");
    }
    
    void PrepareSaveDirectory()
    {
        string savePath = Application.persistentDataPath;
        string dir_name = "Agrifert_exported_results";
        
//        Debug.Log(string.Format("Path.DirectorySeparatorChar: {0}", Path.DirectorySeparatorChar));
        
        if(!Directory.Exists(savePath + Path.DirectorySeparatorChar + dir_name))
        {
            Directory.CreateDirectory(savePath + Path.DirectorySeparatorChar + dir_name);
        }
    }
    
    public void Start()
    {
        PrepareSaveDirectory();
                
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        
        SwitchPageWithoutAnimation(startPage);
        screenWidth = Screen.width;
        maxSpeed = 1080F / timeToAnimate;
    }
    [SerializeField] TMPro.TextMeshProUGUI label1;
    [SerializeField] TMPro.TextMeshProUGUI label2;
    void Update()
    {
        if(screenWidth != Screen.width)
        {
            screenWidth = Screen.width;
            maxSpeed = 1080 / timeToAnimate;
            print("Resolution changed !");
        }
        // label1.text = "Screen (" + Screen.width + ", " + Screen.height + ")";

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchBack();
        }

        // label2.text = "Safe (" + Screen.safeArea.width + ", " + Screen.safeArea.height + ")";
    }

    GameObject GetPage(string pageName)
    {
        GameObject page = null;
        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i].name == pageName)
                page = pages[i];
        }

        return page;
    }

    public void SwitchPageWithoutAnimation(string pageName)
    {
        GameObject page = GetPage(pageName);

        currentPage = GetPage(pageName);
        pageStack.Push(currentPage);
        

        if (currentPage.name == "MainMenu")
        {
            if (back_button != null)
            {
                back_button.SetActive(false);
            }

            instagram_button.SetActive(true);
            website_button.SetActive(true);
            phone_number.SetActive(true);
            
        }
        else
        {
            instagram_button.SetActive(false);
            website_button.SetActive(false);
            
            phone_number.SetActive(true);
        }

        if (page != null)
        {
            for (int i = 0; i < pages.Length; i++)
            {
                pages[i].SetActive(pages[i] == page);
            }
        }
        else
            Debug.LogError(pageName + " not found in pages!");
    }

    

    // (Marat) Coming up back through pages
    public void SwitchBack()
    {
        if(pageStack.Count > 1 && isWorking == false)
        {
            RectTransform current = pageStack.Pop().GetComponent<RectTransform>();
            RectTransform last = pageStack.Peek().GetComponent<RectTransform>();
            
            
            //StartCoroutine(animatePage(current, last, true));
             if(timeToAnimate > 0f)
                 StartCoroutine(animatePageBack(current, last));
        }
    }
    

    // (Marat) Diving deeper through pages
    public void DiveToPage(string pageName)
    {
        pageToSwitch = GetPage(pageName);
        
        if(pageToSwitch != null)
        {
            if(isWorking == false)
            {
                if(timeToAnimate > 0f)
                {
                    StartCoroutine(
                        animatePage(
                            pageStack.Peek().GetComponent<RectTransform>()
                            , pageToSwitch.GetComponent<RectTransform>()
                            , false
                            ));
                }

                pageStack.Push(pageToSwitch);
            }
        }
        else
            Debug.LogError(pageName + " not found in pages!");
        
    }
    

    //TODO: Non-linear animation
    IEnumerator animatePageBack(RectTransform page_current, RectTransform page_previous)
    {
        isWorking = true;
        float timer = 0f;

        float xPos = 0f;

        page_previous.gameObject.SetActive(true);

        while(timer < timeToAnimate)
        {
            timer += Time.deltaTime;
            //float speed = maxSpeed * curve.Evaluate(Mathf.InverseLerp(0f, timeToAnimate, timer));
            //xPos += speed * Time.deltaTime;
            xPos += maxSpeed * Time.deltaTime;
            if(xPos > 1080)
                xPos = 1080;

            page_current.anchoredPosition = new Vector2(xPos, page_current.anchoredPosition.y);
            yield return null;
        }
        
        page_previous.SetSiblingIndex(2);
        page_current.gameObject.SetActive(false);
        currentPage = page_previous.gameObject;
        if (currentPage.name == "MainMenu")
        {
            if (back_button != null)
            {
                back_button.SetActive(false);
            }

            instagram_button.SetActive(true);
            website_button.SetActive(true);
            
            phone_number.SetActive(true);
        }
        else
        {
            // #if !UNITY_ANDROID || !UNITY_EDITOR
            if (back_button != null)
            {
                back_button.SetActive(true);

            }
            // #endif


            instagram_button.SetActive(false);
            website_button.SetActive(false);
            
            //phone_number.SetActive(false);
        }
        isWorking = false;
        
    }
    
    public void ShowExportedFiles_Button()
    {
        string Agrifert_exported_results = "Agrifert_exported_results";
        
        string path_to_open = Application.persistentDataPath + Path.DirectorySeparatorChar + Agrifert_exported_results;
        
        Debug.Log("<color=yellow>ShowExportedFiles_Button()</color>");
        
        Application.OpenURL(path_to_open);
    }

    //TODO: Non-linear animation
    IEnumerator animatePage(RectTransform page_old, RectTransform page_new, bool fromLeft)
    {
        isWorking = true;
        page_old.SetAsFirstSibling();
        if(bg_image != null)
            bg_image.transform.SetAsFirstSibling();
        float timer = 0f;
        
        float dir = fromLeft ? -1 : 1;
        page_new.anchoredPosition = dir * Vector2.right * 1080; //(-1080, 0) or (1080, 0)


        
        
        page_new.gameObject.SetActive(true);
        

        float xPos = page_new.anchoredPosition.x;

        

        while(timer < timeToAnimate)
        {
            timer += Time.deltaTime;


            xPos += -dir * maxSpeed * Time.deltaTime;
            if(fromLeft)
            {
                if(xPos > 0f)
                    xPos = 0f;
            }
            else
            {
                if(xPos < 0f)
                    xPos = 0f;
            }
            
            page_new.anchoredPosition = new Vector2(xPos , page_new.anchoredPosition.y);
            yield return null;
        }

        page_new.anchoredPosition = Vector2.zero;

        currentPage = page_new.gameObject;
        if(currentPage.name == "MainMenu")
        {
            if(back_button != null)
            {
                back_button.SetActive(false);
            }

            instagram_button.SetActive(true);
            website_button.SetActive(true);
            
            phone_number.SetActive(true);
        }
        else
        {
            // #if !UNITY_ANDROID || !UNITY_EDITOR
            if(back_button != null)
            {
                back_button.SetActive(true);
                
            }


            // #endif

            instagram_button.SetActive(false);
            website_button.SetActive(false);
            
            //phone_number.SetActive(false);
        }
        pageToSwitch = null;

        page_old.gameObject.SetActive(false);
        previousMenu = page_old.name;
        isWorking = false;
    }

    public void FadeOutFade()
    {
        
    }


}
