﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUpEffect : MonoBehaviour
{
    static StartUpEffect Instance;
    
    public static StartUpEffect Singleton()
    {
        if(Instance == null)
        {
            Instance = FindObjectOfType<StartUpEffect>();
        }
        
        return Instance;
    }
    
    float duration = 0.5f;
    
    Image img;


    public void Awake()
    {
        img = GetComponent<Image>();
        this.gameObject.SetActive(true); 
    }

    float alpha = 1f;
    float vel = 0f;
    float timer = 0f;
    bool isWorking = false;

    void Start()
    {
        // Color col = img.color;
        // Color col = Color.black;
        // img.color = new Color(col.r, col.g, col.b, 1f);
        FadeIn(0.7f);
    }
    
    public void FadeIn(float _duration)
    {
        isWorking = true;
        duration = _duration;
        vel = 0f;
        timer = 0f;
        alpha = 1f;
        Color col = Color.white;
        img.color = new Color(col.r, col.g, col.b, 1f);
    }
    
    // void FadeOut()
    // {
        
    // }

    void Update()
    {
        if(isWorking)
        {
            Color col = img.color;
            alpha = Mathf.SmoothDamp(alpha, 0f, ref vel, duration);
            img.color = new Color(col.r, col.g, col.b, alpha);
            timer += Time.deltaTime;

            // if(timer >= duration * 2f)
            if(alpha <= 0)
            {
                isWorking = false;
                #if UNITY_EDITOR
                Debug.Log("<color=yellow>Disabling startUp screen effect.</color>");
                #endif
                this.gameObject.SetActive(false);
                // Destroy(this.gameObject);
            }
        }
    }


}
