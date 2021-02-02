using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using TMPro;

[System.Serializable]
public struct PageContentHeight
{
    public string Name;
    public GameObject panelPage;
    public float contentHeight;
    public StandardChangerBlock standardChangerBlock;
}

//0-3 Озимая пшеница
//4-6 Соя
//7-10 Рапс
//11-16 Картофель
//17-22 Кукуруза
//23-28 Подсолнечник
//29-34 Хлопчатник
//35-39 Свекла
// public enum Culture
// {
//     None,
//     Ozimaya_pshenica,
//     Soy,
//     Raps,
//     Kartofel,
//     Kukuruza,
//     Podsolnechnik,
//     Xlopchatnik,
//     Svekla
// }

public class StandardsChanger : MonoBehaviour
{
    
    public TMP_Dropdown cultureDropdown;
    
    [Header("Standards pages:")]
    public PageContentHeight ozimayaPshenica;
    public PageContentHeight soy;
    public PageContentHeight raps;
    public PageContentHeight kartofel;
    public PageContentHeight kukuruza;
    public PageContentHeight podsolnechnik;
    public PageContentHeight xlopchatnik;
    public PageContentHeight svekla;
    
    public Culture inspectedCulture = Culture.None;
    
    public void SaveCurrentStandardButton()
    {
        SaveCurrentStandards();
    }
    
    public void RevertButton()
    {
        switch(inspectedCulture)
        {
            case(Culture.None):
            {
                break;
            }
            case(Culture.Ozimaya_pshenica):
            {
                Calculator.Singleton().MakeStandardsBackupSection(0, 3);
                ReadCurrentStandards(inspectedCulture, false);
                
                break;
            }
            case(Culture.Soy):
            {
                Calculator.Singleton().MakeStandardsBackupSection(4, 6);
                ReadCurrentStandards(inspectedCulture, false);
                break;
            }
            case(Culture.Raps):
            {
                Calculator.Singleton().MakeStandardsBackupSection(7, 9);
                ReadCurrentStandards(inspectedCulture, false);
                break;
            }
            case(Culture.Kartofel):
            {
                Calculator.Singleton().MakeStandardsBackupSection(11, 13);
                Calculator.Singleton().MakeStandardsBackupSection(15, 16);
                ReadCurrentStandards(inspectedCulture, false);
                break;
            }
            case(Culture.Kukuruza):
            {
                Calculator.Singleton().MakeStandardsBackupSection(17, 18);
                Calculator.Singleton().MakeStandardsBackupSection(22, 22);
                ReadCurrentStandards(inspectedCulture, false);
                break;
            }
            case(Culture.Podsolnechnik):
            {
                Calculator.Singleton().MakeStandardsBackupSection(23, 25);
                ReadCurrentStandards(inspectedCulture, false);
                break;
            }
            case(Culture.Xlopchatnik):
            {
                Calculator.Singleton().MakeStandardsBackupSection(29, 30);
                ReadCurrentStandards(inspectedCulture, false);
                break;
            }
            case(Culture.Svekla):
            {
                break;
            }
        }
    }
    
    void Awake()
    {
        
    }
    
    
    public void ReadCurrentStandards(Culture cult, bool fromDisk)
    {
        switch(cult)
        {
            case(Culture.None):
            {
                break;
            }
            case(Culture.Ozimaya_pshenica):
            {
                if(fromDisk)
                {
                    Calculator.Singleton().ReadStandardsFromDisk(cult);
                }
                ozimayaPshenica.standardChangerBlock.seed_melafen_inputField.text = Calculator.Singleton().standards[0].seed.ToString();
                ozimayaPshenica.standardChangerBlock.veg_melafen_inputField.text = Calculator.Singleton().standards[0].vegetation.ToString();
                
                ozimayaPshenica.standardChangerBlock.seed_metallocenUniversal_inputField.text = Calculator.Singleton().standards[1].seed.ToString();
                ozimayaPshenica.standardChangerBlock.veg_metallocenUniversal_inputField.text = Calculator.Singleton().standards[1].vegetation.ToString();
                
                ozimayaPshenica.standardChangerBlock.seed_metallocenBor_inputField.text = Calculator.Singleton().standards[2].seed.ToString();
                ozimayaPshenica.standardChangerBlock.veg_metallocenBor_inputField.text = Calculator.Singleton().standards[2].vegetation.ToString();
                
                ozimayaPshenica.standardChangerBlock.seed_metallocenMed_inputField.text = Calculator.Singleton().standards[3].seed.ToString();
                ozimayaPshenica.standardChangerBlock.veg_metallocenMed_inputField.text = Calculator.Singleton().standards[3].vegetation.ToString();
                
                
                
                break;
            }
            case(Culture.Soy):
            {
                if(fromDisk)
                {
                    Calculator.Singleton().ReadStandardsFromDisk(cult);
                }
                
                soy.standardChangerBlock.seed_melafen_inputField.text = Calculator.Singleton().standards[4].seed.ToString();
                soy.standardChangerBlock.veg_melafen_inputField.text = Calculator.Singleton().standards[4].vegetation.ToString();
                
                soy.standardChangerBlock.seed_metallocenUniversal_inputField.text = Calculator.Singleton().standards[5].seed.ToString();
                soy.standardChangerBlock.veg_metallocenUniversal_inputField.text = Calculator.Singleton().standards[5].vegetation.ToString();
                
                soy.standardChangerBlock.seed_metallocenBor_inputField.text = Calculator.Singleton().standards[6].seed.ToString();
                soy.standardChangerBlock.veg_metallocenBor_inputField.text = Calculator.Singleton().standards[6].vegetation.ToString();
                
                break;
            }
            case(Culture.Raps):
            {
                if(fromDisk)
                {
                    Calculator.Singleton().ReadStandardsFromDisk(cult);
                }
                
                raps.standardChangerBlock.seed_melafen_inputField.text = Calculator.Singleton().standards[7].seed.ToString();
                raps.standardChangerBlock.veg_melafen_inputField.text = Calculator.Singleton().standards[7].vegetation.ToString();
                
                raps.standardChangerBlock.seed_metallocenUniversal_inputField.text = Calculator.Singleton().standards[8].seed.ToString();
                raps.standardChangerBlock.veg_metallocenUniversal_inputField.text = Calculator.Singleton().standards[8].vegetation.ToString();
                
                raps.standardChangerBlock.seed_metallocenBor_inputField.text = Calculator.Singleton().standards[9].seed.ToString();
                raps.standardChangerBlock.veg_metallocenBor_inputField.text = Calculator.Singleton().standards[9].vegetation.ToString();
                break;
            }
            case(Culture.Kartofel):
            {
                if(fromDisk)
                {
                    Calculator.Singleton().ReadStandardsFromDisk(cult);
                }
                
                kartofel.standardChangerBlock.seed_melafen_inputField.text = Calculator.Singleton().standards[11].seed.ToString();
                kartofel.standardChangerBlock.veg_melafen_inputField.text = Calculator.Singleton().standards[11].vegetation.ToString();
                
                kartofel.standardChangerBlock.seed_metallocenUniversal_inputField.text = Calculator.Singleton().standards[12].seed.ToString();
                kartofel.standardChangerBlock.veg_metallocenUniversal_inputField.text = Calculator.Singleton().standards[12].vegetation.ToString();
                
                kartofel.standardChangerBlock.seed_metallocenBor_inputField.text = Calculator.Singleton().standards[13].seed.ToString();
                kartofel.standardChangerBlock.veg_metallocenBor_inputField.text = Calculator.Singleton().standards[13].vegetation.ToString();
                
                kartofel.standardChangerBlock.seed_metallocenMed_inputField.text = Calculator.Singleton().standards[15].seed.ToString();
                kartofel.standardChangerBlock.veg_metallocenMed_inputField.text = Calculator.Singleton().standards[15].vegetation.ToString();
                
                kartofel.standardChangerBlock.seed_metallocenZink_inputField.text = Calculator.Singleton().standards[16].seed.ToString();
                kartofel.standardChangerBlock.veg_metallocenZink_inputField.text = Calculator.Singleton().standards[16].vegetation.ToString();
                break;
            }
            case(Culture.Kukuruza):
            {
                if(fromDisk)
                {
                    Calculator.Singleton().ReadStandardsFromDisk(cult);
                }
                
                kukuruza.standardChangerBlock.seed_melafen_inputField.text = Calculator.Singleton().standards[17].seed.ToString();
                kukuruza.standardChangerBlock.veg_melafen_inputField.text = Calculator.Singleton().standards[17].vegetation.ToString();
                
                kukuruza.standardChangerBlock.seed_metallocenUniversal_inputField.text = Calculator.Singleton().standards[18].seed.ToString();
                kukuruza.standardChangerBlock.veg_metallocenUniversal_inputField.text = Calculator.Singleton().standards[18].vegetation.ToString();
                
                kukuruza.standardChangerBlock.seed_metallocenZink_inputField.text = Calculator.Singleton().standards[22].seed.ToString();
                kukuruza.standardChangerBlock.veg_metallocenZink_inputField.text = Calculator.Singleton().standards[22].vegetation.ToString();
                
                break;
            }
            case(Culture.Podsolnechnik):
            {
                if(fromDisk)
                {
                    Calculator.Singleton().ReadStandardsFromDisk(cult);
                }
                
                podsolnechnik.standardChangerBlock.seed_melafen_inputField.text = Calculator.Singleton().standards[23].seed.ToString();
                podsolnechnik.standardChangerBlock.veg_melafen_inputField.text = Calculator.Singleton().standards[23].vegetation.ToString();
                
                podsolnechnik.standardChangerBlock.seed_metallocenUniversal_inputField.text = Calculator.Singleton().standards[24].seed.ToString();
                podsolnechnik.standardChangerBlock.veg_metallocenUniversal_inputField.text = Calculator.Singleton().standards[24].vegetation.ToString();
                
                podsolnechnik.standardChangerBlock.seed_metallocenBor_inputField.text = Calculator.Singleton().standards[25].seed.ToString();
                podsolnechnik.standardChangerBlock.veg_metallocenBor_inputField.text = Calculator.Singleton().standards[25].vegetation.ToString();
                
                break;
            }
            case(Culture.Xlopchatnik):
            {
                if(fromDisk)
                {
                    Calculator.Singleton().ReadStandardsFromDisk(cult);
                }
                
                xlopchatnik.standardChangerBlock.seed_melafen_inputField.text = Calculator.Singleton().standards[29].seed.ToString();
                xlopchatnik.standardChangerBlock.veg_melafen_inputField.text = Calculator.Singleton().standards[29].vegetation.ToString();
                
                xlopchatnik.standardChangerBlock.seed_metallocenUniversal_inputField.text = Calculator.Singleton().standards[30].seed.ToString();
                xlopchatnik.standardChangerBlock.veg_metallocenUniversal_inputField.text = Calculator.Singleton().standards[30].vegetation.ToString();
                break;
            }
            case(Culture.Svekla):
            {
                if(fromDisk)
                {
                    Calculator.Singleton().ReadStandardsFromDisk(cult);
                }
                
                svekla.standardChangerBlock.seed_melafen_inputField.text = Calculator.Singleton().standards[35].seed.ToString();
                svekla.standardChangerBlock.veg_melafen_inputField.text = Calculator.Singleton().standards[35].vegetation.ToString();
                
                svekla.standardChangerBlock.seed_metallocenUniversal_inputField.text = Calculator.Singleton().standards[36].seed.ToString();
                svekla.standardChangerBlock.veg_metallocenUniversal_inputField.text = Calculator.Singleton().standards[36].vegetation.ToString();
                
                svekla.standardChangerBlock.seed_metallocenBor_inputField.text = Calculator.Singleton().standards[37].seed.ToString();
                svekla.standardChangerBlock.veg_metallocenBor_inputField.text = Calculator.Singleton().standards[37].vegetation.ToString();
                
                svekla.standardChangerBlock.seed_metallocenMarganec_inputField.text = Calculator.Singleton().standards[38].seed.ToString();
                svekla.standardChangerBlock.veg_metallocenMarganec_inputField.text = Calculator.Singleton().standards[38].vegetation.ToString();
                
                svekla.standardChangerBlock.seed_metallocenMed_inputField.text = Calculator.Singleton().standards[39].seed.ToString();
                svekla.standardChangerBlock.veg_metallocenMed_inputField.text = Calculator.Singleton().standards[39].vegetation.ToString();
                
                break;
            }
            
        }
    }
    
    public void SaveCurrentStandards()
    {
        switch(inspectedCulture)
        {
            case(Culture.None):
            {
                break;
            }
            case(Culture.Ozimaya_pshenica):
            {
                
                Calculator.Singleton().standards[0].seed = float.Parse(ozimayaPshenica.standardChangerBlock.seed_melafen_inputField.text);
                Calculator.Singleton().standards[0].vegetation = float.Parse(ozimayaPshenica.standardChangerBlock.veg_melafen_inputField.text);
                
                Calculator.Singleton().standards[1].seed = float.Parse(ozimayaPshenica.standardChangerBlock.seed_metallocenUniversal_inputField.text);
                Calculator.Singleton().standards[1].vegetation = float.Parse(ozimayaPshenica.standardChangerBlock.veg_metallocenUniversal_inputField.text);
                
                Calculator.Singleton().standards[2].seed = float.Parse(ozimayaPshenica.standardChangerBlock.seed_metallocenBor_inputField.text);
                Calculator.Singleton().standards[2].vegetation = float.Parse(ozimayaPshenica.standardChangerBlock.veg_metallocenBor_inputField.text);
                
                Calculator.Singleton().standards[3].seed = float.Parse(ozimayaPshenica.standardChangerBlock.seed_metallocenMed_inputField.text);
                Calculator.Singleton().standards[3].vegetation = float.Parse(ozimayaPshenica.standardChangerBlock.veg_metallocenMed_inputField.text);
                
                Calculator.Singleton().SaveStandardsIndexSection(0, 3);
                
                break;
            }
            case(Culture.Soy):
            {
                Calculator.Singleton().standards[4].seed = float.Parse(soy.standardChangerBlock.seed_melafen_inputField.text);
                Calculator.Singleton().standards[4].vegetation = float.Parse(soy.standardChangerBlock.veg_melafen_inputField.text);
                
                Calculator.Singleton().standards[5].seed = float.Parse(soy.standardChangerBlock.seed_metallocenUniversal_inputField.text);
                Calculator.Singleton().standards[5].vegetation = float.Parse(soy.standardChangerBlock.veg_metallocenUniversal_inputField.text);
                
                Calculator.Singleton().standards[6].seed = float.Parse(soy.standardChangerBlock.seed_metallocenBor_inputField.text);
                Calculator.Singleton().standards[6].vegetation = float.Parse(soy.standardChangerBlock.veg_metallocenBor_inputField.text);
                
                Calculator.Singleton().SaveStandardsIndexSection(4, 6);
                
                break;
            }
            case(Culture.Raps):
            {
                Calculator.Singleton().standards[7].seed = float.Parse(raps.standardChangerBlock.seed_melafen_inputField.text);
                Calculator.Singleton().standards[7].vegetation = float.Parse(raps.standardChangerBlock.veg_melafen_inputField.text);
                
                Calculator.Singleton().standards[8].seed = float.Parse(raps.standardChangerBlock.seed_metallocenUniversal_inputField.text);
                Calculator.Singleton().standards[8].vegetation = float.Parse(raps.standardChangerBlock.veg_metallocenUniversal_inputField.text);
                
                Calculator.Singleton().standards[9].seed = float.Parse(raps.standardChangerBlock.seed_metallocenBor_inputField.text);
                Calculator.Singleton().standards[9].vegetation = float.Parse(raps.standardChangerBlock.veg_metallocenBor_inputField.text);
                
                Calculator.Singleton().SaveStandardsIndexSection(7, 9);
                break;
            }
            case(Culture.Kartofel):
            {
                Calculator.Singleton().standards[11].seed = float.Parse(kartofel.standardChangerBlock.seed_melafen_inputField.text);
                Calculator.Singleton().standards[11].vegetation = float.Parse(kartofel.standardChangerBlock.veg_melafen_inputField.text);
                
                Calculator.Singleton().standards[12].seed = float.Parse(kartofel.standardChangerBlock.seed_metallocenUniversal_inputField.text);
                Calculator.Singleton().standards[12].vegetation = float.Parse(kartofel.standardChangerBlock.veg_metallocenUniversal_inputField.text);
                
                Calculator.Singleton().standards[13].seed = float.Parse(kartofel.standardChangerBlock.seed_metallocenBor_inputField.text);
                Calculator.Singleton().standards[13].vegetation = float.Parse(kartofel.standardChangerBlock.veg_metallocenBor_inputField.text);
                
                Calculator.Singleton().standards[15].seed = float.Parse(kartofel.standardChangerBlock.seed_metallocenMed_inputField.text);
                Calculator.Singleton().standards[15].vegetation = float.Parse(kartofel.standardChangerBlock.veg_metallocenMed_inputField.text);
                
                Calculator.Singleton().standards[16].seed = float.Parse(kartofel.standardChangerBlock.seed_metallocenZink_inputField.text);
                Calculator.Singleton().standards[16].vegetation = float.Parse(kartofel.standardChangerBlock.veg_metallocenZink_inputField.text);
                
                Calculator.Singleton().SaveStandardsIndexSection(11, 13);
                Calculator.Singleton().SaveStandardsIndexSection(15, 16);
                break;
            }
            case(Culture.Kukuruza):
            {
                Calculator.Singleton().standards[17].seed = float.Parse(kukuruza.standardChangerBlock.seed_melafen_inputField.text);
                Calculator.Singleton().standards[17].vegetation = float.Parse(kukuruza.standardChangerBlock.veg_melafen_inputField.text);
                
                Calculator.Singleton().standards[18].seed = float.Parse(kukuruza.standardChangerBlock.seed_metallocenUniversal_inputField.text);
                Calculator.Singleton().standards[18].vegetation = float.Parse(kukuruza.standardChangerBlock.veg_metallocenUniversal_inputField.text);
                
                Calculator.Singleton().standards[22].seed = float.Parse(kukuruza.standardChangerBlock.seed_metallocenZink_inputField.text);
                Calculator.Singleton().standards[22].vegetation = float.Parse(kukuruza.standardChangerBlock.veg_metallocenZink_inputField.text);
                
                break;
            }
            case(Culture.Podsolnechnik):
            {
                Calculator.Singleton().standards[23].seed = float.Parse(podsolnechnik.standardChangerBlock.seed_melafen_inputField.text);
                Calculator.Singleton().standards[23].vegetation = float.Parse(podsolnechnik.standardChangerBlock.veg_melafen_inputField.text);
                
                Calculator.Singleton().standards[24].seed = float.Parse(podsolnechnik.standardChangerBlock.seed_metallocenUniversal_inputField.text);
                Calculator.Singleton().standards[24].vegetation = float.Parse(podsolnechnik.standardChangerBlock.veg_metallocenUniversal_inputField.text);
                
                Calculator.Singleton().standards[25].seed = float.Parse(podsolnechnik.standardChangerBlock.seed_metallocenBor_inputField.text);
                Calculator.Singleton().standards[25].vegetation = float.Parse(podsolnechnik.standardChangerBlock.veg_metallocenBor_inputField.text);
                
                break;
            }
            case(Culture.Xlopchatnik):
            {
                Calculator.Singleton().standards[29].seed = float.Parse(xlopchatnik.standardChangerBlock.seed_melafen_inputField.text);
                Calculator.Singleton().standards[29].vegetation = float.Parse(xlopchatnik.standardChangerBlock.veg_melafen_inputField.text);
                
                Calculator.Singleton().standards[30].seed = float.Parse(xlopchatnik.standardChangerBlock.seed_metallocenUniversal_inputField.text);
                Calculator.Singleton().standards[30].vegetation = float.Parse(xlopchatnik.standardChangerBlock.veg_metallocenUniversal_inputField.text);
                break;
            }
            case(Culture.Svekla):
            {
                Calculator.Singleton().standards[35].seed = float.Parse(svekla.standardChangerBlock.seed_melafen_inputField.text);
                Calculator.Singleton().standards[35].vegetation = float.Parse(svekla.standardChangerBlock.veg_melafen_inputField.text);
                
                Calculator.Singleton().standards[36].seed = float.Parse(svekla.standardChangerBlock.seed_metallocenUniversal_inputField.text);
                Calculator.Singleton().standards[36].vegetation = float.Parse(svekla.standardChangerBlock.veg_metallocenUniversal_inputField.text);
                
                Calculator.Singleton().standards[37].seed = float.Parse(svekla.standardChangerBlock.seed_metallocenBor_inputField.text);
                Calculator.Singleton().standards[37].vegetation = float.Parse(svekla.standardChangerBlock.veg_metallocenBor_inputField.text);
                
                Calculator.Singleton().standards[38].seed = float.Parse(svekla.standardChangerBlock.seed_metallocenMarganec_inputField.text);
                Calculator.Singleton().standards[38].vegetation = float.Parse(svekla.standardChangerBlock.veg_metallocenMarganec_inputField.text);
                
                Calculator.Singleton().standards[39].seed = float.Parse(svekla.standardChangerBlock.seed_metallocenZink_inputField.text);
                Calculator.Singleton().standards[39].vegetation = float.Parse(svekla.standardChangerBlock.veg_metallocenZink_inputField.text);
                break;
            }
            
        }
    }
    
    public RectTransform scrollViewRectTransform;
    void ResizeScrollViewContent(float newHeight)
    {
        Vector2 size = scrollViewRectTransform.sizeDelta;
        size.y = newHeight;
        scrollViewRectTransform.sizeDelta = size;
    }
    
    public void OnDropDownValueChanged()
    {
        inspectedCulture = (Culture)cultureDropdown.value;
        
        //Debug.Log(string.Format("<color=yellow> StandardsChanger: current culture is {0}</color>", inspectedCulture));
        
        switch(inspectedCulture)
        {
            case(Culture.None):
            {
                ozimayaPshenica.panelPage.SetActive(false);
                soy.panelPage.SetActive(false);
                raps.panelPage.SetActive(false);
                kartofel.panelPage.SetActive(false);
                kukuruza.panelPage.SetActive(false);
                podsolnechnik.panelPage.SetActive(false);
                xlopchatnik.panelPage.SetActive(false);
                svekla.panelPage.SetActive(false);
                ReadCurrentStandards(inspectedCulture, true);
                ResizeScrollViewContent(1550);
                
                break;
            }
            case(Culture.Ozimaya_pshenica):
            {
                ozimayaPshenica.panelPage.SetActive(true);
                soy.panelPage.SetActive(false);
                raps.panelPage.SetActive(false);
                kartofel.panelPage.SetActive(false);
                kukuruza.panelPage.SetActive(false);
                podsolnechnik.panelPage.SetActive(false);
                xlopchatnik.panelPage.SetActive(false);
                svekla.panelPage.SetActive(false);
                ReadCurrentStandards(inspectedCulture, true);
                ResizeScrollViewContent(ozimayaPshenica.contentHeight);
                
                break;
            }
            case(Culture.Soy):
            {
                ozimayaPshenica.panelPage.SetActive(false);
                soy.panelPage.SetActive(true);
                raps.panelPage.SetActive(false);
                kartofel.panelPage.SetActive(false);
                kukuruza.panelPage.SetActive(false);
                podsolnechnik.panelPage.SetActive(false);
                xlopchatnik.panelPage.SetActive(false);
                svekla.panelPage.SetActive(false);
                ReadCurrentStandards(inspectedCulture, true);
                ResizeScrollViewContent(soy.contentHeight);
                
                break;
            }
            case(Culture.Raps):
            {
                ozimayaPshenica.panelPage.SetActive(false);
                soy.panelPage.SetActive(false);
                raps.panelPage.SetActive(true);
                kartofel.panelPage.SetActive(false);
                kukuruza.panelPage.SetActive(false);
                podsolnechnik.panelPage.SetActive(false);
                xlopchatnik.panelPage.SetActive(false);
                svekla.panelPage.SetActive(false);
                ReadCurrentStandards(inspectedCulture, true);
                ResizeScrollViewContent(raps.contentHeight);
                
                break;
            }
            case(Culture.Kartofel):
            {
                ozimayaPshenica.panelPage.SetActive(false);
                soy.panelPage.SetActive(false);
                raps.panelPage.SetActive(false);
                kartofel.panelPage.SetActive(true);
                kukuruza.panelPage.SetActive(false);
                podsolnechnik.panelPage.SetActive(false);
                xlopchatnik.panelPage.SetActive(false);
                svekla.panelPage.SetActive(false);
                ReadCurrentStandards(inspectedCulture, true);
                ResizeScrollViewContent(kartofel.contentHeight);
                
                break;
            }
            case(Culture.Kukuruza):
            {
                ozimayaPshenica.panelPage.SetActive(false);
                soy.panelPage.SetActive(false);
                raps.panelPage.SetActive(false);
                kartofel.panelPage.SetActive(false);
                kukuruza.panelPage.SetActive(true);
                podsolnechnik.panelPage.SetActive(false);
                xlopchatnik.panelPage.SetActive(false);
                svekla.panelPage.SetActive(false);
                ReadCurrentStandards(inspectedCulture, true);
                ResizeScrollViewContent(kukuruza.contentHeight);
                break;
            }
            case(Culture.Podsolnechnik):
            {
                ozimayaPshenica.panelPage.SetActive(false);
                soy.panelPage.SetActive(false);
                raps.panelPage.SetActive(false);
                kartofel.panelPage.SetActive(false);
                kukuruza.panelPage.SetActive(false);
                podsolnechnik.panelPage.SetActive(true);
                xlopchatnik.panelPage.SetActive(false);
                svekla.panelPage.SetActive(false);
                ReadCurrentStandards(inspectedCulture, true);
                ResizeScrollViewContent(podsolnechnik.contentHeight);
                break;
            }
            case(Culture.Xlopchatnik):
            {
                ozimayaPshenica.panelPage.SetActive(false);
                soy.panelPage.SetActive(false);
                raps.panelPage.SetActive(false);
                kartofel.panelPage.SetActive(false);
                kukuruza.panelPage.SetActive(false);
                podsolnechnik.panelPage.SetActive(false);
                xlopchatnik.panelPage.SetActive(true);
                svekla.panelPage.SetActive(false);
                ReadCurrentStandards(inspectedCulture, true);
                ResizeScrollViewContent(xlopchatnik.contentHeight);
                break;
            }
            case(Culture.Svekla):
            {
                ozimayaPshenica.panelPage.SetActive(false);
                soy.panelPage.SetActive(false);
                raps.panelPage.SetActive(false);
                kartofel.panelPage.SetActive(false);
                kukuruza.panelPage.SetActive(false);
                podsolnechnik.panelPage.SetActive(false);
                xlopchatnik.panelPage.SetActive(false);
                svekla.panelPage.SetActive(true);
                ReadCurrentStandards(inspectedCulture, true);
                ResizeScrollViewContent(svekla.contentHeight);
                break;
            }   
        }
    }
}
