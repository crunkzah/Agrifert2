using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using TMPro;



//0-3 Озимая пшеница
//4-6 Соя
//8-10 Рапс
//11-16 Картофель
//17-22 Кукуруза
public enum Culture
{
    None,
    Ozimaya_pshenica,
    Soy,
    Raps,
    Kartofel,
    Kukuruza,
    Podsolnechnik,
    Xlopchatnik,
    Svekla
}

public enum Microfertilizer
{
    Melafen,
    Metallocen_Universal,
    Metallocen_Bor,
    Metallocen_Med,
    Metallocen_Marganec,
    Metallocen_Zink
}

[System.Serializable]
public struct Standard
{
    public string name;
    public Culture culture;
    public Microfertilizer type;    // Тип микроудобрения
    public float seed;              // Литр/тонна
    public float vegetation;        // Литр/тонна
    public float price;             // Цена за 1 литр, руб/литр
    public int index;
}


public class Calculator : MonoBehaviour
{
    public GameObject debugItem;
    public GameObject debugLabel;
    
    public GameObject calculatedItemPrefab;
    public GameObject labelItemPrefab;
    
    
    //Adjusting ui size:
    public RectTransform content_rect;
    public RectTransform panel_parent;
    public RectTransform results_label;
    float results_label_height = 75;
    float y_item_height = 300;
    float y_step_size   = 50;
    float y_label_height = 75;
    float initial_offset = -100;
    
    
    public TMP_Dropdown dropdown_culture;
    public TMP_InputField inputField_weight;
    public TMP_InputField inputField_norma;
    public TMP_InputField inputField_area;
    
    public Standard[] standards;
    
    public Culture current_culture;
    public float current_weight;
    public float standard_per_ha;
    public float current_area; 
    
    List<GameObject> calculatedItems = new List<GameObject>();
    
    void Start()
    {
        debugItem.SetActive(false);
        debugLabel.SetActive(false);
    }    
    
    void ReadInput()
    {
        string input; 
        input = inputField_weight.text;
        
        current_weight = float.Parse(input);
        
        input = inputField_norma.text;
        
        standard_per_ha = float.Parse(input);
        
        input = inputField_area.text;
        
        current_area = float.Parse(input);
        
    }
    
    public void OnValueChangedArea(string area_value)
    {
        float area = float.Parse(inputField_area.text); //га
        float weight = float.Parse(inputField_weight.text);
        
        if((int)area > 0 && (int)weight > 0)
        {
            float norma = weight * 1000 / area;
            if(inputField_norma.interactable)
            {
                inputField_norma.text = ((int)norma).ToString();
            }
        }
    }
    
    public void OnValueChangedNorma(string norma_value)
    {
        float norma = float.Parse(inputField_norma.text); // кг/га
        
        float weight = float.Parse(inputField_weight.text);
        
        if((int)norma > 0)
        {
            float area = weight * 1000 / norma;
            
            if(inputField_area.interactable)
            {
                inputField_area.text = ((int)area).ToString();
            }
        }
    }
    
    void ReadCulture()
    {
        Debug.Log(dropdown_culture.value);
        current_culture = (Culture)dropdown_culture.value;
        Debug.Log(current_culture.ToString()); 
    }
    
    void ResetInput()
    {
        dropdown_culture.value = 0;
        dropdown_culture.RefreshShownValue();
                
        inputField_weight.text = "0";
        inputField_norma.text  = "0";
        inputField_area.text   = "0";
    }
    
    public void OnDropDownValueChanged()
    {
        if(dropdown_culture.value == (int)Culture.Svekla)
        {
            inputField_weight.interactable = false;
            inputField_norma.interactable = false;
            inputField_weight.text = "0";
            inputField_norma.text = "0";
        }
        else
        {
            inputField_weight.interactable = true;
            inputField_norma.interactable = true;
        }
    }
    
    public void Calculate()
    {
        Debug.Log("<color=yellow>Calculate() !</color>");
        ReadCulture();
        
        switch(current_culture)
        {
            case Culture.None:
            {
                ClearResults();
                break;
            }
            case Culture.Ozimaya_pshenica:
            {
                ReadInput();
                Result_Ozimaya_pshenica();
                break;
            }
            case Culture.Soy:
            {
                ReadInput();
                Result_Soy();
                break;
            }
            case Culture.Raps:
            {
                ReadInput();
                Result_Raps();
                break;
            }
            case Culture.Kartofel:
            {
                ReadInput();
                Result_Kartofel();
                break;
            }
            case Culture.Kukuruza:
            {
                ReadInput();
                Result_Kukuruza();
                break;
            }
            case Culture.Podsolnechnik:
            {
                ReadInput();
                Result_Podsolnechnik();
                break;
            }
            case Culture.Xlopchatnik:
            {
                ReadInput();
                Result_Xlopchatnik();
                break;
            }
            case Culture.Svekla:
            {
                ReadInput();
                Result_Svekla();
                break;
            }
        }
    }
    
    
    void ClearResults()
    {
        int len = calculatedItems.Count;
        for(int i = 0; i < len; i++)
        {
            Destroy(calculatedItems[i]);
        }
        
        content_rect.sizeDelta = new Vector2(0, 1550);
        
        
        Debug.Log("<color=green>ClearResults()</color>");
    }
    
    void Result_Svekla()
    {
        ClearResults();
        
        GameObject item;
       
        CalculatedItem ci;
        float delta_height = 0;
        float current_y = 0;
        current_y = results_label.anchoredPosition.y - initial_offset;
        {
            string txt = "Фазы для обработок (рекомендации): между фазой 4-6 настоящих листьев 20 дней до уборки:";
            MakeLabel(txt, ref current_y, ref delta_height);
        }
        float sum2 = 0;
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_melafen_v = current_area * standards[35].vegetation;
            float veg_melafen_price = veg_melafen_v * standards[35].price;
            
            //
            ci.label1.SetText("Обработка по вегетации регулятором роста растений <b><color=#41AB4A>Мелафен</color> (2 обработки по 15 мл)</b>:");
            ci.volume.SetText(FormatLitres(veg_melafen_v));
            ci.price.SetText(FormatPrice(veg_melafen_price));
            
            sum2 += veg_melafen_price;
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_universal_v = current_area * standards[36].vegetation;
            float veg_universal_price = veg_universal_v * standards[36].price;
            
            //
            ci.label1.SetText("Обработка по вегетации комплексным удобрением <color=#41AB4A><b>Универсал</b></color>:");
            ci.volume.SetText(FormatLitres(veg_universal_v));
            ci.price.SetText(FormatPrice(veg_universal_price));
            
            sum2 += veg_universal_price;
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_bor_v = current_area * standards[37].vegetation;
            float veg_bor_price = veg_bor_v * standards[37].price;
            
            //
            ci.label1.SetText("Обработка по вегетации комплексным удобрением <color=#41AB4A><b>Бор и Молибден</b></color>:");
            ci.volume.SetText(FormatLitres(veg_bor_v));
            ci.price.SetText(FormatPrice(veg_bor_price));
            
            sum2 += veg_bor_price;
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_med_v = current_area * standards[39].vegetation;
            float veg_med_price = veg_med_v * standards[39].price;
            
            //
            ci.label1.SetText("Обработка по вегетации удобрением <color=#41AB4A><b>Медь</b></color>:");
            ci.volume.SetText(FormatLitres(veg_med_v));
            ci.price.SetText(FormatPrice(veg_med_price));
            
            sum2 += veg_med_price;
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_marganec_v = current_area * standards[38].vegetation;
            float veg_marganec_price = veg_marganec_v * standards[38].price;
            
            //
            ci.label1.SetText("Обработка по вегетации удобрением <color=#41AB4A><b>Марганец</b></color>:");
            ci.volume.SetText(FormatLitres(veg_marganec_v));
            ci.price.SetText(FormatPrice(veg_marganec_price));
            
            sum2 += veg_marganec_price;
        }
        {
            string txt = "Итого: " + FormatPrice(sum2);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
        }
        
        FitContentSize(delta_height);
    }
    
    void Result_Xlopchatnik()
    {
        ClearResults();
        
        GameObject item;
       
        CalculatedItem ci;
        float delta_height = 0;
        float current_y = 0;
        current_y = results_label.anchoredPosition.y - initial_offset;
        
        float sum1 = 0;
        {
            string txt = "Обработка семян: ";
            MakeLabel(txt, ref current_y, ref delta_height);
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float seed_melafen_v = current_weight * standards[29].seed;
            float seed_melafen_price = seed_melafen_v * standards[29].price;
            
            //
            ci.label1.SetText("Обработка семян регулятором роста растений <color=#41AB4A><b>Мелафен</b></color>:");
            ci.volume.SetText(FormatLitres(seed_melafen_v));
            ci.price.SetText(FormatPrice(seed_melafen_price));
            
            sum1 += seed_melafen_price;
        }
        {
            string txt = "Итого: " + FormatPrice(sum1);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
        }
        {
            string txt = "Фазы для обработок (рекомендации): бутонизация или начало формирования коробочки:";
            MakeLabel(txt, ref current_y, ref delta_height);
        }
        float sum2 = 0;
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_melafen_v = current_area * standards[29].vegetation;
            float veg_melafen_price = veg_melafen_v * standards[29].price;
            
            //
            ci.label1.SetText("Обработка по вегетации регулятором роста растений <color=#41AB4A><b>Мелафен</b></color>:");
            ci.volume.SetText(FormatLitres(veg_melafen_v));
            ci.price.SetText(FormatPrice(veg_melafen_price));
            
            sum2 += veg_melafen_price;
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_universal_v = current_area * standards[30].vegetation;
            float veg_universal_price = veg_universal_v * standards[30].price;
            
            //
            ci.label1.SetText("Обработка по вегетации комплексным удобрением <color=#41AB4A><b>Универсал</b></color>:");
            ci.volume.SetText(FormatLitres(veg_universal_v));
            ci.price.SetText(FormatPrice(veg_universal_price));
            
            sum2 += veg_universal_price;
        }
        {
            string txt = "Итого: " + FormatPrice(sum2);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
        }
        FitContentSize(delta_height);
    }
    
    void Result_Podsolnechnik()
    {
        ClearResults();
        
        GameObject item;
       
        CalculatedItem ci;
        float delta_height = 0;
        float current_y = 0;
        current_y = results_label.anchoredPosition.y - initial_offset;
        
        float sum1 = 0;
        {
            string txt = "Обработка семян: ";
            MakeLabel(txt, ref current_y, ref delta_height);
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float seed_melafen_v = current_weight * standards[23].seed;
            float seed_melafen_price = seed_melafen_v * standards[23].price;
            
            //
            ci.label1.SetText("Обработка семян регулятором роста растений <color=#41AB4A><b>Мелафен</b></color>:");
            ci.volume.SetText(FormatLitres(seed_melafen_v));
            ci.price.SetText(FormatPrice(seed_melafen_price));
            
            sum1 += seed_melafen_price;
        }
        {
            string txt = "Итого: " + FormatPrice(sum1);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
        }
        {
            string txt = "Фазы для обработок (рекомендации): между фазой 2-4 пары листьев и до окончания фазы формирования корзинок:";
            MakeLabel(txt, ref current_y, ref delta_height);
        }
        float sum2 = 0;
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_universal_v = current_area * standards[24].vegetation;
            float veg_universal_price = veg_universal_v * standards[24].price;
            
            //
            ci.label1.SetText("Обработка по вегетации комплексным удобрением <color=#41AB4A><b>Универсал</b></color>:");
            ci.volume.SetText(FormatLitres(veg_universal_v));
            ci.price.SetText(FormatPrice(veg_universal_price));
            
            sum2 += veg_universal_price;
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_bor_v = current_area * standards[25].vegetation;
            float veg_bor_price = veg_bor_v * standards[25].price;
            
            //
            ci.label1.SetText("Обработка по вегетации комплексным удобрением <color=#41AB4A><b>Бор и Молибден</b></color>:");
            ci.volume.SetText(FormatLitres(veg_bor_v));
            ci.price.SetText(FormatPrice(veg_bor_price));
            
            sum2 += veg_bor_price;
        }
        {
            string txt = "Итого: " + FormatPrice(sum2);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
        }
        FitContentSize(delta_height);
    }
    
    void Result_Kukuruza()
    {
        ClearResults();
        
        GameObject item;
       
        CalculatedItem ci;
        float delta_height = 0;
        float current_y = 0;
        current_y = results_label.anchoredPosition.y - initial_offset;
        
        float sum1 = 0;
        {
            string txt = "Обработка семян: ";
            MakeLabel(txt, ref current_y, ref delta_height);
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float seed_melafen_v = current_weight * standards[17].seed;
            float seed_melafen_price = seed_melafen_v * standards[17].price;
            
            //
            ci.label1.SetText("Обработка семян регулятором роста растений <color=#41AB4A><b>Мелафен</b></color>:");
            ci.volume.SetText(FormatLitres(seed_melafen_v));
            ci.price.SetText(FormatPrice(seed_melafen_price));
            
            sum1 += seed_melafen_price;
        }
        {
            string txt = "Итого: " + FormatPrice(sum1);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
        }
        /*{
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float seed_universal_v = current_weight * standards[18].seed;
            float seed_universal_price = seed_universal_v * standards[18].price;
            
            //
            ci.label1.SetText("Обработка семян комплексным удобрением <color=#41AB4A><b>Универсал</b></color>");
            ci.volume.SetText(FormatLitres(seed_universal_v));
            ci.price.SetText(FormatPrice(seed_universal_price));
            
            sum1 += seed_universal_price;
        }*/
        {
            string txt = "Фазы для обработок (рекомендации): между фазой 6-7 листьев и фазой выбрасывания метелки:";
            MakeLabel(txt, ref current_y, ref delta_height);
        }
        float sum2 = 0;
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_universal_v = current_area * standards[18].vegetation;
            float veg_universal_price = veg_universal_v * standards[18].price;
            
            //
            ci.label1.SetText("Обработка по вегетации комплексным удобрением <color=#41AB4A><b>Универсал</b></color>:");
            ci.volume.SetText(FormatLitres(veg_universal_v));
            ci.price.SetText(FormatPrice(veg_universal_price));
            
            sum2 += veg_universal_price;
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_zink_v = current_area * standards[22].vegetation;
            float veg_zink_price = veg_zink_v * standards[22].price;
            
            //
            ci.label1.SetText("Обработка по вегетации удобрением <color=#41AB4A><b>Цинк</b></color>:");
            ci.volume.SetText(FormatLitres(veg_zink_v));
            ci.price.SetText(FormatPrice(veg_zink_price));
            
            sum2 += veg_zink_price;
        }
        {
            string txt = "Итого: " + FormatPrice(sum2);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
        }
        
        FitContentSize(delta_height);
    }
    
    void Result_Kartofel()
    {
        ClearResults();
        
        GameObject item;
       
        CalculatedItem ci;
        float delta_height = 0;
        float current_y = 0;
        current_y = results_label.anchoredPosition.y - initial_offset;
        
        float sum1 = 0;
        {
            string txt = "Обработка семян: ";
            MakeLabel(txt, ref current_y, ref delta_height);
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float seed_melafen_v = current_weight * standards[11].seed;
            float seed_melafen_price = seed_melafen_v * standards[11].price;
            
            //
            ci.label1.SetText("Обработка семян регулятором роста растений <color=#41AB4A><b>Мелафен</b></color>:");
            ci.volume.SetText(FormatLitres(seed_melafen_v));
            ci.price.SetText(FormatPrice(seed_melafen_price));
            
            sum1 += seed_melafen_price;
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float seed_universal_v = current_weight * standards[12].seed;
            float seed_universal_price = seed_universal_v * standards[12].price;
            
            //
            ci.label1.SetText("Обработка семян комплексным удобрением <color=#41AB4A><b>Универсал</b></color>:");
            ci.volume.SetText(FormatLitres(seed_universal_v));
            ci.price.SetText(FormatPrice(seed_universal_price));
            
            sum1 += seed_universal_price;
        }
        {
            string txt = "Итого: " + FormatPrice(sum1);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
        }
        {
            string txt = "Фазы для обработок (рекомендации): между фазой высоты всходов 15 см. до окончания цветения:";
            MakeLabel(txt, ref current_y, ref delta_height);
        }
        float sum2 = 0;
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_melafen_v = current_area * standards[11].vegetation;
            float veg_melafen_price = veg_melafen_v * standards[11].price;
            
            //
            ci.label1.SetText("Обработка по вегетации регулятором роста растений <color=#41AB4A><b>Мелафен</b></color>:");
            ci.volume.SetText(FormatLitres(veg_melafen_v));
            ci.price.SetText(FormatPrice(veg_melafen_price));
            
            sum2 += veg_melafen_price;
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_universal_v = current_area * standards[12].vegetation;
            float veg_universal_price = veg_universal_v * standards[12].price;
            
            //
            ci.label1.SetText("Обработка по вегетации комплексным удобрением <color=#41AB4A><b>Универсал</b></color>:");
            ci.volume.SetText(FormatLitres(veg_universal_v));
            ci.price.SetText(FormatPrice(veg_universal_price));
            
            sum2 += veg_universal_price;
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_bor_v = current_area * standards[13].vegetation;
            float veg_bor_price = veg_bor_v * standards[13].price;
            
            //
            ci.label1.SetText("Обработка по вегетации комплексным удобрением <color=#41AB4A><b>Бор и Молибден</b></color>:");
            ci.volume.SetText(FormatLitres(veg_bor_v));
            ci.price.SetText(FormatPrice(veg_bor_price));
            
            sum2 += veg_bor_price;
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_med_v = current_area * standards[15].vegetation;
            float veg_med_price = veg_med_v * standards[15].price;
            
            //
            ci.label1.SetText("Обработка по вегетации удобрением <color=#41AB4A><b>Медь</b></color>:");
            ci.volume.SetText(FormatLitres(veg_med_v));
            ci.price.SetText(FormatPrice(veg_med_price));
            
            sum2 += veg_med_price;
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_zink_v = current_area * standards[16].vegetation;
            float veg_zink_price = veg_zink_v * standards[16].price;
            
            //
            ci.label1.SetText("Обработка по вегетации удобрением <color=#41AB4A><b>Цинк</b></color>:");
            ci.volume.SetText(FormatLitres(veg_zink_v));
            ci.price.SetText(FormatPrice(veg_zink_price));
            
            sum2 += veg_zink_price;
        }
        /*{
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_marganec_v = current_area * standards[14].vegetation;
            float veg_marganec_price = veg_marganec_v * standards[14].price;
            
            //
            ci.label1.SetText("Обработка по вегетации удобрением <color=#41AB4A><b>Марганец</b></color>");
            ci.volume.SetText(FormatLitres(veg_marganec_v));
            ci.price.SetText(FormatPrice(veg_marganec_price));
            
            sum2 += veg_marganec_price;
        }*/
        {
            string txt = "Итого: " + FormatPrice(sum2);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
        }
        
        FitContentSize(delta_height);
    }
    
    void Result_Raps()
    {
        ClearResults();
        
        GameObject item;
       
        CalculatedItem ci;
        float delta_height = 0;
        float current_y = 0;
        current_y = results_label.anchoredPosition.y - initial_offset;
        
        float sum1 = 0;
        
        {
            string txt = "Обработка семян: ";
            MakeLabel(txt, ref current_y, ref delta_height);
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float seed_melafen_v = current_weight * standards[7].seed;
            float seed_melafen_price = seed_melafen_v * standards[7].price;
            
            //
            ci.label1.SetText("Обработка семян регулятором роста растений <color=#41AB4A><b>Мелафен</b></color>:");
            ci.volume.SetText(FormatLitres(seed_melafen_v));
            ci.price.SetText(FormatPrice(seed_melafen_price));
            
            sum1 += seed_melafen_price;
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float seed_universal_v = current_weight * standards[8].seed;
            float seed_universal_price = seed_universal_v * standards[8].price;
            
            //
            ci.label1.SetText("Обработка семян комплексным удобрением <color=#41AB4A><b>Универсал</b></color>:");
            ci.volume.SetText(FormatLitres(seed_universal_v));
            ci.price.SetText(FormatPrice(seed_universal_price));
            
            sum1 += seed_universal_price;
        }
        {
            string txt = "Итого: " + FormatPrice(sum1);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
        }
        {
            string txt = "Фазы для обработок (рекомендации): между фазой формирования листовой розетки до развития стручков:";
            MakeLabel(txt, ref current_y, ref delta_height);
        }
        float sum2 = 0;
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_melafen_v = current_area * standards[7].vegetation;
            float veg_melafen_price = veg_melafen_v * standards[7].price;
            
            //
            ci.label1.SetText("Обработка по вегетации регулятором роста растений <color=#41AB4A><b>Мелафен</b></color>:");
            ci.volume.SetText(FormatLitres(veg_melafen_v));
            ci.price.SetText(FormatPrice(veg_melafen_price));
            
            sum2 += veg_melafen_price;
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_universal_v = current_area * standards[8].vegetation;
            float veg_universal_price = veg_universal_v * standards[8].price;
            
            //
            ci.label1.SetText("Обработка по вегетации регулятором роста растений <color=#41AB4A><b>Универсал</b></color>:");
            ci.volume.SetText(FormatLitres(veg_universal_v));
            ci.price.SetText(FormatPrice(veg_universal_price));
            
            sum2 += veg_universal_price;
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_bor_v = current_area * standards[9].vegetation;
            float veg_bor_price = veg_bor_v * standards[9].price;
            
            //
            ci.label1.SetText("Обработка по вегетации комплексным удобрением <color=#41AB4A><b>Бор и Молибден</b></color>:");
            ci.volume.SetText(FormatLitres(veg_bor_v));
            ci.price.SetText(FormatPrice(veg_bor_price));
            
            sum2 += veg_bor_price;
        }
        {
            string txt = "Итого: " + FormatPrice(sum2);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
        }
        
        FitContentSize(delta_height);
    }
    
    void Result_Soy()
    {
        ClearResults();
        
        GameObject item;
       
        CalculatedItem ci;
        float delta_height = 0;
        float current_y = 0;
        current_y = results_label.anchoredPosition.y - initial_offset;
        
        float sum1 = 0;
        {
            string txt = "Обработка семян: ";
            MakeLabel(txt, ref current_y, ref delta_height);
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float seed_melafen_v = current_weight * standards[4].seed;
            float seed_melafen_price = seed_melafen_v * standards[4].price;
            
            //
            ci.label1.SetText("Обработка семян регулятором роста растений <color=#41AB4A><b>Мелафен</b></color>:");
            ci.volume.SetText(FormatLitres(seed_melafen_v));
            ci.price.SetText(FormatPrice(seed_melafen_price));
            
            sum1 += seed_melafen_price;
        }
        {
            string txt = "Итого: " + FormatPrice(sum1);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
        }
        {
            string txt = "Фазы для обработок (рекомендации): между полностью развитые листья 1 узла и начала бобо-образования:";
            MakeLabel(txt, ref current_y, ref delta_height);
        }
        float sum2 = 0;
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_melafen_v = current_area * standards[4].vegetation;
            float veg_melafen_price = veg_melafen_v * standards[4].price;
            //
            ci.label1.SetText("Обработка по вегетации регулятором роста растений <color=#41AB4A><b>Мелафен</b></color>:");
            ci.volume.SetText(FormatLitres(veg_melafen_v));
            ci.price.SetText(FormatPrice(veg_melafen_price));
            
            sum2 += veg_melafen_price;
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_universal_v = current_area * standards[5].vegetation;
            float veg_universal_price = veg_universal_v * standards[5].price;
            //
            ci.label1.SetText("Обработка по вегетации комплексным удобрением <color=#41AB4A><b>Универсал</b></color>:");
            ci.volume.SetText(FormatLitres(veg_universal_v));
            ci.price.SetText(FormatPrice(veg_universal_price));
            
            sum2 += veg_universal_price;
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_bor_v = current_area * standards[6].vegetation;
            float veg_bor_price = veg_bor_v * standards[6].price;
            //
            ci.label1.SetText("Обработка по вегетации комплексным удобрением <color=#41AB4A><b>Бор и Молибден</b></color>:");
            ci.volume.SetText(FormatLitres(veg_bor_v));
            ci.price.SetText(FormatPrice(veg_bor_price));
            
            sum2 += veg_bor_price;
        }
        {
            string txt = "Итого: " + FormatPrice(sum2);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
        }
        
        FitContentSize(delta_height);
    }
    
    void Result_Ozimaya_pshenica()
    {
        ClearResults();
        
        
        GameObject item;
       
        CalculatedItem ci;
        float delta_height = 0;
        float current_y = 0;
        current_y = results_label.anchoredPosition.y - initial_offset;
        
        float sum1 = 0;
        {
            string txt = "Обработка семян: ";
            MakeLabel(txt, ref current_y, ref delta_height);
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float seed_melafen_v = current_weight * standards[0].seed;
            float seed_melafen_price = seed_melafen_v * standards[0].price;
            
            //
            ci.label1.SetText("Обработка семян регулятором роста растений <color=#41AB4A><b>Мелафен</b></color>:");
            ci.volume.SetText(FormatLitres(seed_melafen_v));
            ci.price.SetText(FormatPrice(seed_melafen_price));
            
            sum1 += seed_melafen_price;
        }
        
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float seed_universal_v = current_weight * standards[1].seed;
            float seed_universal_price = seed_universal_v * standards[1].price;
            //
            ci.label1.SetText("Обработка семян комплексным удобрением <color=#41AB4A><b>Универсал</b></color>:");
            ci.volume.SetText(FormatLitres(seed_universal_v));
            ci.price.SetText(FormatPrice(seed_universal_price));
            
            sum1 += seed_universal_price;
        }
        {
            string txt = "Итого: " + FormatPrice(sum1);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
        }
        {
            string txt = "Обработки по вегетации, фазы: кущения, выход в трубку, колошения: ";
            MakeLabel(txt, ref current_y, ref delta_height);
        }
        float sum2 = 0;
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_melafen_v = current_area * standards[0].vegetation;
            float veg_melafen_price = veg_melafen_v * standards[0].price;
            //
            ci.label1.SetText("Обработка по вегетации регулятором роста растений <color=#41AB4A><b>Мелафен</b></color>:");
            ci.volume.SetText(FormatLitres(veg_melafen_v));
            ci.price.SetText(FormatPrice(veg_melafen_price));
            
            sum2 += veg_melafen_price;
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_universal_v = current_area * standards[1].vegetation;
            float veg_universal_price = veg_universal_v * standards[1].price;
            //
            ci.label1.SetText("Обработка по вегетации комплексным удобрением <color=#41AB4A><b>Универсал</b></color>:");
            ci.volume.SetText(FormatLitres(veg_universal_v));
            ci.price.SetText(FormatPrice(veg_universal_price));
            sum2 += veg_universal_price;
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_bor_v = current_area * standards[2].vegetation;
            float veg_bor_price = veg_bor_v * standards[2].price;
            //
            ci.label1.SetText("Обработка по вегетации комплексным удобрением <color=#41AB4A><b>Бор и Молибден</b></color>:");
            ci.volume.SetText(FormatLitres(veg_bor_v));
            ci.price.SetText(FormatPrice(veg_bor_price));
            
            sum2 += veg_bor_price;
        }
        {
            item = MakeCalculatedItem(ref current_y, ref delta_height);
            ci = item.GetComponent<CalculatedItem>();
            //    
            float veg_med_v = current_area * standards[3].vegetation;
            float veg_med_price = veg_med_v * standards[3].price;
            //
            ci.label1.SetText("Обработка по вегетации комплексным удобрением <color=#41AB4A><b>Медь</b></color>:");
            ci.volume.SetText(FormatLitres(veg_med_v));
            ci.price.SetText(FormatPrice(veg_med_price));
            
            sum2 += veg_med_price;
        }
        {
            string txt = "Итого: " + FormatPrice(sum2);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
        }
        
        FitContentSize(delta_height);
    }
    
    
    GameObject MakeLabel(string txt, ref float yCoord, ref float yDeltaHeight)
    {
        GameObject label = Instantiate(labelItemPrefab, Vector3.zero, Quaternion.identity, content_rect);
        
        Label_calculated lc = label.GetComponent<Label_calculated>();
        lc.SetText(txt);
        
        RectTransform rectTransform;
        calculatedItems.Add(label);
        
        rectTransform = label.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, yCoord);
        rectTransform.sizeDelta = new Vector2(750, y_label_height);
        
        yCoord -= y_step_size + y_label_height;
        yDeltaHeight -= y_step_size + y_label_height;
        
        return label;
        
    }
    GameObject MakeCalculatedItem(ref float yCoord, ref float yDeltaHeight)
    {
        GameObject item;
        RectTransform rectTransform;
        
        item = Instantiate(calculatedItemPrefab, Vector3.zero, Quaternion.identity, content_rect);
        calculatedItems.Add(item);
        rectTransform = item.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, yCoord);
        rectTransform.sizeDelta = new Vector2(750, y_item_height);
        
        yCoord -= y_step_size + y_item_height;
        yDeltaHeight -= y_step_size + y_item_height;
        
        return item;
    }
    
    string FormatPrice(float val)
    {
        string s = FormatMillion(((int)val).ToString());
        StringBuilder sb = new StringBuilder(s);
        
        sb.Append("  руб");
        
        return sb.ToString();
    }
    
    string FormatLitres(float val)
    {
        int val_int = Mathf.CeilToInt(val);
        string s = (val_int).ToString();
        StringBuilder sb = new StringBuilder(s);
        
        sb.Append(" л");
        
        return sb.ToString();
    }
    
    void FitContentSize(float deltaHeight)
    {
         content_rect.sizeDelta = new Vector2(content_rect.sizeDelta.x, content_rect.sizeDelta.y - deltaHeight);
    }
    
    
    string FormatMillion(string s)
    {
        StringBuilder Result = new StringBuilder(s);
        
        if(s.Length < 4)
        {
            return s;
        }
        
        int k = 0;
        for(int i = s.Length-1; i >= 0; i--)
        {
            k++;
            if(k % 3 == 0)
            {
                k = 0;
                if(i != 0)
                    Result.Insert(i, " ");
            }
        }
        
        return Result.ToString();
    }
}
