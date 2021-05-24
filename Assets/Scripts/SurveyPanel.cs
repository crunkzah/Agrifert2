using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public enum SurveyPanelType
{
    value,
    variant1
}

public class SurveyPanel : MonoBehaviour
{
    public SurveyPanelType panel_type;
    public TextMeshProUGUI panel_question_tmp;
    public TMP_InputField inputField_result;
    public TMP_Dropdown dropdown_result;
    
    static List<SurveyPanel> All_survey_panels = new List<SurveyPanel>();
    
    public static void ResetAllSurveyPanels()
    {
        foreach(SurveyPanel sp in All_survey_panels)
        {
            if(sp.inputField_result != null)
            {
                sp.inputField_result.text = "";
            }
            else
            {
                //Debug.Log("<color=yellow>sp.inputField_result is null on </color>" + sp.gameObject.name);
            }
        }
    }
    
    void Start()
    {
        if(!All_survey_panels.Contains(this))
        {
            All_survey_panels.Add(this);
        }
    }
    
    public string GetQuestionName()
    {
        
        string question_name = panel_question_tmp.text;
        
        if(question_name.Length > 0)
        {
            //Removing ':' 
            if(question_name[question_name.Length - 1] == ':')
            {
                StringBuilder sb = new StringBuilder(question_name);
                sb.Remove(sb.Length - 1, 1);
                question_name = sb.ToString();
            }
        }
        
        return question_name;
    }
    
    public string GetResult()
    {
        string Result = "Result";
        
        switch(panel_type)
        {
            case(SurveyPanelType.value):
            {
                
                Result = inputField_result.text;
                if(Result == string.Empty)
                {
                    Result = "-";
                }
                break;
            }
            case(SurveyPanelType.variant1):
            {
                Result = dropdown_result.options[dropdown_result.value].text;
                //Result = "Variant";
                break;
            }
        }
        
        return Result;
    }
    
}
