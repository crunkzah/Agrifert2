using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System.Text;

[System.Serializable]
public struct StringPair
{
    public string v1;
    public string v2;
    
    public StringPair(string _v1, string _v2)
    {
        v1 = _v1;
        v2 = _v2;
    }
}

public class SurveyManager : MonoBehaviour
{
    public TextMeshProUGUI panel_name;
    public TextMeshProUGUI question_number_label;
    
    void SetQuestionNumber()
    {
        if(current_panel + 1 <= survey_panels.Length)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Вопрос ");
            sb.Append((current_panel + 1));
            sb.Append(" из ");
            sb.Append((survey_panels.Length));
            question_number_label.SetText(sb.ToString());
        }
    }
    
    public SurveyPanel[] survey_panels;
    
    void SetPanelName(string _txt)
    {
        panel_name.SetText(_txt);
    }
    
    public GameObject save_button;
    public GameObject back_button;
    public GameObject next_button;
    
    void HideAllPanels()
    {
        for(int i = 0; i < survey_panels.Length; i++)
        {
            survey_panels[i].gameObject.SetActive(false);
        }
    }
    
    void ShowPanel(int panel_index)
    {
        if(panel_index > survey_panels.Length)
        {
            //Debug.Log("Trying to show panel out of range");
            return;
        }
        
        if(panel_index == survey_panels.Length)
        {
            save_button.SetActive(true);
            HideAllPanels();
        }
        else
        {
            HideAllPanels();
            save_button.SetActive(false);
            survey_panels[panel_index].gameObject.SetActive(true);
        }
    }
    
    public int current_panel = 0;
    
    void Start()
    {
        InitSurveyState();
    }
    
    public List<StringPair> saved_results = new List<StringPair>();
    
    public void ResetButton()
    {
        InitSurveyState();
    }
    
    public void SaveButton()
    {
        string survey_name = "Отчет_опросника";
        string html_as_string = HTMLMaker.MakeHTMLPage_FromSurvey(ref saved_results);
        SaveSystem.SaveHTML_IOS_Android(html_as_string, survey_name);
        
    // #if UNITY_ANDROID || UNITY_IOS
    //     SaveSystem.SaveHTML_IOS_Android(html_as_string, survey_name);
    // #else
    //     SaveSystem.SaveHTML_Survey(html_as_string, survey_name);
    // #endif
        // UI_Manager.ShowMessage("Отчет сохранен!");
    }
    
    public void BackButton()
    {
        ReturnPrevPanel();
    }
    
    void InitSurveyState()
    {
        current_panel = 0;
        SurveyPanel.ResetAllSurveyPanels();
        saved_results.Clear();
        ShowPanel(current_panel);
        SetPanelName("Описание почвы (для определения уровня потенциального плодородия и доступности элементов питания):");
        SetQuestionNumber();
        //OnQuestionBeforeSwitch();
    }
   
    void ReturnPrevPanel()
    {
        if(current_panel > 0)
        {
            // if(current_panel == survey_panels.Length)
            // {
                
            // }
            // else
            // {
            // }
            current_panel--;
            DisplayThingsForCurrentPanel();
            
            //OnProceedForward();
            
            ShowPanel(current_panel);
            SetQuestionNumber();
        }
        else
        {
            Debug.Log("<color=yellow>First panel reached!</color>");
        }
    }
    
    public void ProceedNextPanel()
    {
        if(current_panel == survey_panels.Length - 1)
        {
            OnProceedForward();
            current_panel++;
            ShowPanel(current_panel);
            DisplayThingsForCurrentPanel();
        }
        else
        {
            if(current_panel < survey_panels.Length - 1)
            {
                OnProceedForward();
                current_panel++;
                ShowPanel(current_panel);
                DisplayThingsForCurrentPanel();
                SetQuestionNumber();
            }
            else
            {
                Debug.Log("<color=yellow>Last panel reached!</color>");
            }
        }
    }
    SurveyPanel GetCurrentSurveyPanel()
    {
        return survey_panels[current_panel];
    }

// #if UNITY_EDITOR
//     void Update()
//     {
//         if(Input.GetKeyDown(KeyCode.E))
//         {
//             PrintList();
//         }
//     }
// #endif
    
    public void PrintList()
    {
        if(saved_results.Count == 0)
        {
            Debug.Log("<color=#eb34d5>Saved results are empty!</color>");    
        }
        
        foreach(StringPair sp in saved_results)
        {
            if(sp.v2 == "title")
            {
                Debug.Log(string.Format("<color=red>{0}</color>", sp.v1));
            }
            else
            {
                Debug.Log(string.Format("<color=yellow>{0}; {1}</color>", sp.v1, sp.v2));
            }
        }    
    }
    
    void AddRecord(StringPair x)
    {
        for(int i = 0; i < saved_results.Count; i++)
        {
            if(saved_results[i].v1 == x.v1)
            {
                saved_results[i] = x;
                return;
            }
        }
        
        saved_results.Add(x);
    }
    
    public void DisplayThingsForCurrentPanel()
    {
        back_button.SetActive(true);
        next_button.SetActive(true);
        switch(current_panel)
        {
            case 0:
            {
                back_button.SetActive(false);
                SetPanelName("Описание почвы (для определения уровня потенциального плодородия и доступности элементов питания):");
                break;
            }
            case 1:
            {
                SetPanelName("Описание почвы (для определения уровня потенциального плодородия и доступности элементов питания):");
                break;
            }
            case 2:
            {
                SetPanelName("Описание почвы (для определения уровня потенциального плодородия и доступности элементов питания):");
                break;
            }
            case 3:
            {
                SetPanelName("Описание почвы (для определения уровня потенциального плодородия и доступности элементов питания):");
                break;
            }
            case 4:
            {
                SetPanelName("Описание почвы (для определения уровня потенциального плодородия и доступности элементов питания):");
                break;
            }
            case 5:
            {
                SetPanelName("Описание почвы (для определения уровня потенциального плодородия и доступности элементов питания):");
                break;
            }
            case 6:
            {
                SetPanelName("Описание почвы (для определения уровня потенциального плодородия и доступности элементов питания):");
                break;
            }
            case 7:
            {
                SetPanelName("Состав воды в рабочем растворе при опрыскивании урожая:");
                break;
            }
            case 8:
            {
                SetPanelName("Климатические особенности:");
                break;
            }
            case 9:
            {
                SetPanelName("Климатические особенности:");
                break;
            }
            case 10:
            {
                SetPanelName("Семенной материал:");
                break;
            }
            case 11:
            {
                SetPanelName("Генные и сортовые особенности реакции на минеральное питание:");
                break;
            }
            case 12:
            {
                SetPanelName("Генные и сортовые особенности реакции на минеральное питание:");
                break;
            }
            case 13:
            {
                SetPanelName("Генные и сортовые особенности реакции на минеральное питание:");
                break;
            }
            case 14:
            {
                SetPanelName("Минеральное питание:");
                break;
            }
            case 15:
            {
                SetPanelName("Минеральное питание:");
                break;
            }
            case 16:
            {
                SetPanelName("Болезни культуры и сорность полей:");
                break;
            }
            case 17:
            {
                SetPanelName("Исторические данные:");
                break;
            }
            case 18:
            {
                SetPanelName("Сохраните результаты опроса");
                next_button.SetActive(false);
                break;
            }
            default:
            {
                SetPanelName("Тематика вопроса:");
                break;
            }
        }
    }
    
    public void OnProceedForward()
    {
        switch(current_panel)
        {
            case 0:
            {
                // SetPanelName("Описание почвы (для определения уровня потенциального плодородия и доступности элементов питания):");
                AddRecord(new StringPair("Описание почвы (для определения уровня потенциального плодородия и доступности элементов питания)", "title"));
                
                StringPair question_record = new StringPair(GetCurrentSurveyPanel().GetQuestionName(), GetCurrentSurveyPanel().GetResult());
                
                AddRecord(question_record);
                break;
            }
            case 1:
            {
                
                // SetPanelName("Описание почвы (для определения уровня потенциального плодородия и доступности элементов питания):");
                AddRecord(new StringPair("Описание почвы (для определения уровня потенциального плодородия и доступности элементов питания)", "title"));
                
                StringPair question_record = new StringPair(GetCurrentSurveyPanel().GetQuestionName(), GetCurrentSurveyPanel().GetResult());
                AddRecord(question_record);
                break;
            }
            case 2:
            {
                // SetPanelName("Описание почвы (для определения уровня потенциального плодородия и доступности элементов питания):");
                AddRecord(new StringPair("Описание почвы (для определения уровня потенциального плодородия и доступности элементов питания)", "title"));
                
                StringPair question_record = new StringPair(GetCurrentSurveyPanel().GetQuestionName(), GetCurrentSurveyPanel().GetResult());
                AddRecord(question_record);
                break;
            }
            case 3:
            {
                // SetPanelName("Описание почвы (для определения уровня потенциального плодородия и доступности элементов питания):");
                AddRecord(new StringPair("Описание почвы (для определения уровня потенциального плодородия и доступности элементов питания)", "title"));
                
                StringPair question_record = new StringPair(GetCurrentSurveyPanel().GetQuestionName(), GetCurrentSurveyPanel().GetResult());
                AddRecord(question_record);
                break;
            }
            case 4:
            {
                // SetPanelName("Описание почвы (для определения уровня потенциального плодородия и доступности элементов питания):");
                AddRecord(new StringPair("Описание почвы (для определения уровня потенциального плодородия и доступности элементов питания)", "title"));
                
                StringPair question_record = new StringPair(GetCurrentSurveyPanel().GetQuestionName(), GetCurrentSurveyPanel().GetResult());
                AddRecord(question_record);
                break;
            }
            case 5:
            {
                // SetPanelName("Описание почвы (для определения уровня потенциального плодородия и доступности элементов питания):");
                AddRecord(new StringPair("Описание почвы (для определения уровня потенциального плодородия и доступности элементов питания)", "title"));
                
                StringPair question_record = new StringPair(GetCurrentSurveyPanel().GetQuestionName(), GetCurrentSurveyPanel().GetResult());
                AddRecord(question_record);
                break;
            }
            case 6:
            {
                // SetPanelName("Описание почвы (для определения уровня потенциального плодородия и доступности элементов питания):");
                AddRecord(new StringPair("Описание почвы (для определения уровня потенциального плодородия и доступности элементов питания)", "title"));
                
                StringPair question_record = new StringPair(GetCurrentSurveyPanel().GetQuestionName(), GetCurrentSurveyPanel().GetResult());
                AddRecord(question_record);
                break;
            }
            case 7:
            {
                // SetPanelName("Состав воды в рабочем растворе при опрыскивании урожая:");
                AddRecord(new StringPair("Состав воды в рабочем растворе при опрыскивании урожая", "title"));
                
                StringPair question_record = new StringPair(GetCurrentSurveyPanel().GetQuestionName(), GetCurrentSurveyPanel().GetResult());
                AddRecord(question_record);
                break;
            }
            case 8:
            {
                // SetPanelName("Климатические особенности:");
                AddRecord(new StringPair("Климатические особенности", "title"));
                
                
                StringPair question_record = new StringPair(GetCurrentSurveyPanel().GetQuestionName(), GetCurrentSurveyPanel().GetResult());
                AddRecord(question_record);
                break;
            }
            case 9:
            {
                // SetPanelName("Климатические особенности:");
                AddRecord(new StringPair("Климатические особенности", "title"));
                
                StringPair question_record = new StringPair(GetCurrentSurveyPanel().GetQuestionName(), GetCurrentSurveyPanel().GetResult());
                AddRecord(question_record);
                break;
            }
            case 10:
            {
                // SetPanelName("Семенной материал:");
                
                AddRecord(new StringPair("Семенной материал", "title"));
                
                StringPair question_record = new StringPair(GetCurrentSurveyPanel().GetQuestionName(), GetCurrentSurveyPanel().GetResult());
                AddRecord(question_record);
                break;
            }
            case 11:
            {
                // SetPanelName("Генные и сортовые особенности реакции на минеральное питание:");
                AddRecord(new StringPair("Генные и сортовые особенности реакции на минеральное питание", "title"));
                
                StringPair question_record = new StringPair(GetCurrentSurveyPanel().GetQuestionName(), GetCurrentSurveyPanel().GetResult());
                AddRecord(question_record);
                break;
            }
            case 12:
            {
                // SetPanelName("Генные и сортовые особенности реакции на минеральное питание:");
                AddRecord(new StringPair("Генные и сортовые особенности реакции на минеральное питание", "title"));
                
                StringPair question_record = new StringPair(GetCurrentSurveyPanel().GetQuestionName(), GetCurrentSurveyPanel().GetResult());
                AddRecord(question_record);
                break;
            }
            case 13:
            {
                // SetPanelName("Генные и сортовые особенности реакции на минеральное питание:");
                AddRecord(new StringPair("Генные и сортовые особенности реакции на минеральное питание", "title"));
                
                StringPair question_record = new StringPair(GetCurrentSurveyPanel().GetQuestionName(), GetCurrentSurveyPanel().GetResult());
                AddRecord(question_record);
                break;
            }
            case 14:
            {
                // SetPanelName("Минеральное питание:");
                AddRecord(new StringPair("Минеральное питание", "title"));
                
                StringPair question_record = new StringPair(GetCurrentSurveyPanel().GetQuestionName(), GetCurrentSurveyPanel().GetResult());
                AddRecord(question_record);
                break;
            }
            case 15:
            {
                // SetPanelName("Минеральное питание:");
                AddRecord(new StringPair("Минеральное питание", "title"));
                
                StringPair question_record = new StringPair(GetCurrentSurveyPanel().GetQuestionName(), GetCurrentSurveyPanel().GetResult());
                AddRecord(question_record);
                break;
            }
            case 16:
            {
                // SetPanelName("Болезни культуры и сорность полей:");
                AddRecord(new StringPair("Болезни культуры и сорность полей", "title"));
                
                StringPair question_record = new StringPair(GetCurrentSurveyPanel().GetQuestionName(), GetCurrentSurveyPanel().GetResult());
                AddRecord(question_record);
                break;
            }
            case 17:
            {
                // SetPanelName("Исторические данные:");
                AddRecord(new StringPair("Исторические данные", "title"));
                
                StringPair question_record = new StringPair(GetCurrentSurveyPanel().GetQuestionName(), GetCurrentSurveyPanel().GetResult());
                AddRecord(question_record);
                break;
            }
            case 18:
            {
                SetPanelName("Сохраните результаты опроса");
                
                break;
            }
            default:
            {
                SetPanelName("Тематика вопроса:");
                break;
            }
        }
    }
    
}
