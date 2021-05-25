using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using TMPro;


[System.Serializable]
public struct StringTriplet
{
    public string v1;
    public string v2;
    public string v3;
    
    public StringTriplet(string _v1, string _v2, string _v3)
    {
        v1 = _v1;
        v2 = _v2;
        v3 = _v3;
    }
}

//0-3 Озимая пшеница
//4-6 Соя
//7-10 Рапс
//11-16 Картофель
//17-22 Кукуруза
//23-28 Подсолнечник
//29-34 Хлопчатник
//35-39 Свекла

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
    static Calculator instance;
    public static Calculator Singleton()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<Calculator>();
        }
        
        return instance;
    }
    
    public GameObject debugItem;
    public GameObject debugLabel;
    
    public List<StringTriplet> export_triplets = new List<StringTriplet>();
    
    void AddToExport(StringTriplet x)
    {
        export_triplets.Add(x);
    }
    
    public void MakeExportCurrentResult()
    {
        if(canExport)
        {
            string export_name = CultureItemReader.GetCultureName(current_culture);
            
            // if(current_culture == Culture.Ozimaya_pshenica || current_culture == Culture.Soy)
            // {
                string calculator_html_FileName = "Расчеты_" + export_name;
                string html_as_string = HTMLMaker.MakeHTMLPage_FromCalculator(CultureItemReader.GetCultureNamePretty(current_culture), ref export_triplets);
                SaveSystem.SaveHTML_Calculator(html_as_string, calculator_html_FileName);
            // }
            // else
            // {
            //     SaveSystem.SaveTxt(export_buffer, export_name);
            // }
            UI_Manager.ShowMessage("Выгрузка <color=#19d40f>произошла</color>.");
        }
        else
        {
            UI_Manager.ShowMessage("Выгрузка <color=#d4360f>не удалась</color>...");
        }
    }
    
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
    
    CultureInfo ci;
    
    string ExportSpecifier = "G";
    CultureInfo ExportCulture = CultureInfo.CreateSpecificCulture("eu-ES");
    
    
    
    void Start()
    {
        int isFirstTime = PlayerPrefs.GetInt("FirstTime", 1);
        if(isFirstTime == 1)
        {
            SaveAllStandards();
            PlayerPrefs.SetInt("FirstTime", -1);
        }
        
        // PlayerPrefs.DeleteAll();
        // PlayerPrefs.Save();
        
        ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";
        
        //avarage = double.Parse("0.0",NumberStyles.Any,ci);
        debugItem.SetActive(false);
        debugLabel.SetActive(false);
    }    
    
    void ReadInput()
    {
        string input; 
        input = inputField_weight.text;
        
        if(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
        {
            current_weight = 0f;    
        }
        else
        {
            Debug.Log(input);
            current_weight = float.Parse(input, NumberStyles.Any, ci);
        }
        
        input = inputField_norma.text;
        
        if(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
        {
            standard_per_ha = 0;
        }
        else
        {
            Debug.Log(input);
            standard_per_ha = float.Parse(input, NumberStyles.Any, ci);
        }
        
        input = inputField_area.text;
        
        if(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
        {
            current_area = 0;
        }
        else
        {
            Debug.Log(input);   
            current_area = float.Parse(input, NumberStyles.Any, ci);
        }
    }
    
    public void OnValueChangedArea()
    {
        string area_value = inputField_area.text;
        if(string.IsNullOrEmpty(area_value))
        {
            return;
        }
        
        //Debug.Log(string.Format("<color=yellow>{0}</color>", inputField_area.text));
        float area = float.Parse(inputField_area.text, NumberStyles.Any, ci); //га
        //Debug.Log(string.Format("<color=yellow>{0}</color>", inputField_weight.text));
        float weight = float.Parse(inputField_weight.text, NumberStyles.Any, ci);
            
        if(area > 0 && weight > 0)
        {
            float norma = weight * 1000f / area;
            if(inputField_norma.interactable)
            {
                inputField_norma.text = (norma).ToString();
            }
        }
    }
    
    
    
    public void OnValueChangedNorma()
    {
        string norma_value = inputField_norma.text;
        if(string.IsNullOrEmpty(norma_value))
        {
            return;
        }
        
        //Debug.Log(string.Format("<color=yellow>{0}</color>", inputField_norma.text));
        float norma = float.Parse(inputField_norma.text, NumberStyles.Any, ci); // кг/га
        
        //Debug.Log(string.Format("<color=yellow>{0}</color>", inputField_weight.text));
        float weight = float.Parse(inputField_weight.text, NumberStyles.Any, ci);
        
        if(norma > 0)
        {
            float area = weight * 1000f / norma;
            
            if(inputField_area.interactable)
            {
                inputField_area.text = (area).ToString();
            }
        }
    }
    
    void ReadCulture()
    {
        // Debug.Log(dropdown_culture.value);
        current_culture = (Culture)dropdown_culture.value;
        // Debug.Log(current_culture.ToString()); 
    }
    
    void ResetInput()
    {
        // dropdown_culture.value = 0;
        // dropdown_culture.RefreshShownValue();
        
        
                
        inputField_weight.text = string.Empty;
        inputField_norma.text  = string.Empty;
        inputField_area.text   = string.Empty;
    }
    
    public void OnDropDownValueChanged()
    {
        ResetInput();
        ClearResults();
        canExport = false;
        
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
        
        //UI_Manager.ShowMessage("Расчет!");
    }
    
    public void Calculate()
    {
        // Debug.Log("<color=yellow>Calculate() !</color>");
        ReadCulture();
        
        switch(current_culture)
        {
            case Culture.None:
            {
                export_buffer.Clear();
                export_triplets.Clear();
                ClearResults();
                canExport = false;
                
                UI_Manager.ShowMessage("Сначала нужно выбрать культуру!");
                
                break;
            }
            case Culture.Ozimaya_pshenica:
            {
                export_buffer.Clear();
                export_triplets.Clear();
                export_buffer.AppendLine("Расчет продукции для культуры \"Озимая пшеница\": ");
                //AddToExport(new StringTriplet("Расчет продукции для культуры \"Озимая пшеница\"", "title_1", ""));
                //export_buffer.AppendLine("");
                
                ReadInput();
                Result_Ozimaya_pshenica();
                canExport = true;
                UI_Manager.ShowMessage("Расчет для Озимой пшеницы произведен!");
                
                
                break;
            }
            case Culture.Soy:
            {
                export_buffer.Clear();
                export_triplets.Clear();
                export_buffer.AppendLine("Расчет продукции для культуры \"Соя\": ");
                export_buffer.AppendLine("");
                
                ReadInput();
                Result_Soy();
                canExport = true;
                UI_Manager.ShowMessage("Расчет для Сои произведен!");
                
                break;
            }
            case Culture.Raps:
            {
                export_buffer.Clear();
                export_triplets.Clear();
                export_buffer.AppendLine("Расчет продукции для культуры \"Рапс\": ");
                export_buffer.AppendLine("");
                
                ReadInput();
                Result_Raps();
                canExport = true;
                UI_Manager.ShowMessage("Расчет для Рапса произведен!");
                
                break;
            }
            case Culture.Kartofel:
            {
                export_buffer.Clear();
                export_triplets.Clear();
                export_buffer.AppendLine("Расчет продукции для культуры \"Картофель\": ");
                export_buffer.AppendLine("");
                
                ReadInput();
                Result_Kartofel();
                canExport = true;
                UI_Manager.ShowMessage("Расчет для Картофеля произведен!");
                
                break;
            }
            case Culture.Kukuruza:
            {
                export_buffer.Clear();
                export_triplets.Clear();
                export_buffer.AppendLine("Расчет продукции для культуры \"Кукуруза\": ");
                export_buffer.AppendLine("");
                
                ReadInput();
                Result_Kukuruza();
                canExport = true;
                UI_Manager.ShowMessage("Расчет для Кукурузы произведен!");
                
                break;
            }
            case Culture.Podsolnechnik:
            {
                export_buffer.Clear();
                export_triplets.Clear();
                export_buffer.AppendLine("Расчет продукции для культуры \"Подсолнечник\": ");
                export_buffer.AppendLine("");
                
                ReadInput();
                Result_Podsolnechnik();
                canExport = true;
                UI_Manager.ShowMessage("Расчет для Подсолнечника произведен!");
                
                break;
            }
            case Culture.Xlopchatnik:
            {
                export_buffer.Clear();
                export_triplets.Clear();
                export_buffer.AppendLine("Расчет продукции для культуры \"Хлопчатник\": ");
                export_buffer.AppendLine("");
                
                ReadInput();
                Result_Xlopchatnik();
                canExport = true;
                UI_Manager.ShowMessage("Расчет для Хлопчатника произведен!");
                
                break;
            }
            case Culture.Svekla:
            {
                export_buffer.Clear();
                export_triplets.Clear();
                export_buffer.AppendLine("Расчет продукции для культуры \"Сахарная свёкла\": ");
                export_buffer.AppendLine("");
                
                ReadInput();
                Result_Svekla();
                canExport = true;
                UI_Manager.ShowMessage("Расчет для Сахарной свёклы произведен!");
                
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
            
            AddToExport(new StringTriplet(txt, "title_1", ""));
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
            
            export_buffer.AppendLine("Мелафен (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_melafen_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_melafen_price));
            
            StringTriplet st = new StringTriplet("Мелафен (вегетация)", FormatForExport_Litres(veg_melafen_v), FormatForExport_Price(veg_melafen_price));
            AddToExport(st);
            
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
            
            export_buffer.AppendLine("Универсал (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_universal_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_universal_price));
            
            StringTriplet st = new StringTriplet("Универсал (вегетация)", FormatForExport_Litres(veg_universal_v), FormatForExport_Price(veg_universal_price));
            AddToExport(st);
            
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
            
            export_buffer.AppendLine("Бор и Молибден (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_bor_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_bor_price));
            
            StringTriplet st = new StringTriplet("Бор и Молибден (вегетация)", FormatForExport_Litres(veg_bor_v), FormatForExport_Price(veg_bor_price));
            AddToExport(st);
            
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
            
            export_buffer.AppendLine("Медь (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_med_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_med_price));
            
            StringTriplet st = new StringTriplet("Медь (вегетация)", FormatForExport_Litres(veg_med_v), FormatForExport_Price(veg_med_price));
            AddToExport(st);
            
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
            
            export_buffer.AppendLine("Марганец (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_marganec_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_marganec_price));
            
            StringTriplet st = new StringTriplet("Марганец (вегетация)", FormatForExport_Litres(veg_marganec_v), FormatForExport_Price(veg_marganec_price));
            AddToExport(st);
            
            sum2 += veg_marganec_price;
        }
        {
            string txt = "Итого: " + FormatPrice(sum2);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
            
            export_buffer.AppendLine("Итого (вегетация): " + FormatForExport(sum2));
            AddToExport(new StringTriplet("Итого (вегетация):", "sum_price", FormatForExport_Price(sum2)));
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
            
            export_buffer.AppendLine(txt);
            AddToExport(new StringTriplet("Обработка семян:", "title_1", ""));
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
            
            export_buffer.AppendLine("Мелафен:");
            export_buffer.AppendLine(FormatForExport_Litres(seed_melafen_v));
            export_buffer.AppendLine(FormatForExport_Price(seed_melafen_price));
            
            StringTriplet st = new StringTriplet("Мелафен", FormatForExport_Litres(seed_melafen_v), FormatForExport_Price(seed_melafen_price));
            AddToExport(st);
        }
        {
            string txt = "Итого: " + FormatPrice(sum1);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
            
            export_buffer.AppendLine("Итого:" + FormatForExport_Price(sum1));
            
            AddToExport(new StringTriplet("Итого: ", "sum_price", FormatForExport_Price(sum1)));
        }
        {
            string txt = "Фазы для обработок (рекомендации): бутонизация или начало формирования коробочки:";
            MakeLabel(txt, ref current_y, ref delta_height);
            
            export_buffer.AppendLine(txt);
            AddToExport(new StringTriplet(txt, "title_1", ""));
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
            
            export_buffer.AppendLine("Мелафен (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_melafen_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_melafen_price));
            
            StringTriplet st = new StringTriplet("Мелафен (вегетация)", FormatForExport_Litres(veg_melafen_v), FormatForExport_Price(veg_melafen_price));
            AddToExport(st);
            
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
            
            export_buffer.AppendLine("Универсал (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_universal_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_universal_price));
            
            StringTriplet st = new StringTriplet("Универсал (вегетация)", FormatForExport_Litres(veg_universal_v), FormatForExport_Price(veg_universal_price));
            AddToExport(st);
            
            sum2 += veg_universal_price;
        }
        {
            string txt = "Итого: " + FormatPrice(sum2);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
            
            export_buffer.AppendLine("Итого (вегетация): " + FormatForExport_Price(sum2));
            AddToExport(new StringTriplet("Итого (вегетация):", "sum_price", FormatForExport_Price(sum2)));
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
            
            export_buffer.AppendLine("Мелафен:");
            export_buffer.AppendLine(FormatForExport_Litres(seed_melafen_v));
            export_buffer.AppendLine(FormatForExport_Price(seed_melafen_price));
        }
        {
            string txt = "Итого: " + FormatPrice(sum1);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
            
            export_buffer.AppendLine("Итого: " + FormatForExport_Price(sum1));
        }
        {
            string txt = "Фазы для обработок (рекомендации): между фазой 2-4 пары листьев и до окончания фазы формирования корзинок:";
            MakeLabel(txt, ref current_y, ref delta_height);
            export_buffer.AppendLine(txt);
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
            
            export_buffer.AppendLine("Универсал (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_universal_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_universal_price));
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
            
            export_buffer.AppendLine("Бор и Молибден (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_bor_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_bor_price));
        }
        {
            string txt = "Итого: " + FormatPrice(sum2);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
            
            export_buffer.AppendLine("Итого: " + FormatForExport_Price(sum2));
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
            
            export_buffer.AppendLine(txt);
            AddToExport(new StringTriplet("Обработка семян:", "title_1", ""));
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
            
            export_buffer.AppendLine("Мелафен:");
            export_buffer.AppendLine(FormatForExport_Litres(seed_melafen_v));
            export_buffer.AppendLine(FormatForExport_Price(seed_melafen_price));
            
            StringTriplet st = new StringTriplet("Мелафен", FormatForExport_Litres(seed_melafen_v), FormatForExport_Price(seed_melafen_price));
            AddToExport(st);
        }
        {
            string txt = "Итого: " + FormatPrice(sum1);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
            
            export_buffer.AppendLine("Итого: " + FormatForExport_Price(sum1));
            AddToExport(new StringTriplet("Итого: ", "sum_price", FormatForExport_Price(sum1)));
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
            export_buffer.AppendLine(txt);
            
            AddToExport(new StringTriplet("Фазы для обработок (рекомендации): между фазой 6-7 листьев и фазой выбрасывания метелки", "title_1", ""));
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
            
            export_buffer.AppendLine("Универсал (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_universal_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_universal_price));
            
            StringTriplet st = new StringTriplet("Универсал (вегетация)", FormatForExport_Litres(veg_universal_v), FormatForExport_Price(veg_universal_price));
            AddToExport(st);
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
            
            export_buffer.AppendLine("Цинк (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_zink_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_zink_price));
            
            StringTriplet st = new StringTriplet("Цинк (вегетация)", FormatForExport_Litres(veg_zink_v), FormatForExport_Price(veg_zink_price));
            AddToExport(st);
        }
        {
            string txt = "Итого: " + FormatPrice(sum2);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
            
            export_buffer.AppendLine("Итого: " + FormatForExport_Price(sum2));
            AddToExport(new StringTriplet("Итого (вегетация):", "sum_price", FormatForExport_Price(sum2)));
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
            AddToExport(new StringTriplet("Обработка семян:", "title_1", ""));
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
            
            export_buffer.AppendLine("Мелафен:");
            export_buffer.AppendLine(FormatForExport_Litres(seed_melafen_v));
            export_buffer.AppendLine(FormatForExport_Price(seed_melafen_price));
            
            StringTriplet st = new StringTriplet("Мелафен", FormatForExport_Litres(seed_melafen_v), FormatForExport_Price(seed_melafen_price));
            AddToExport(st);
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
            
            export_buffer.AppendLine("Универсал:");
            export_buffer.AppendLine(FormatForExport_Litres(seed_universal_v));
            export_buffer.AppendLine(FormatForExport_Price(seed_universal_price));
            
            StringTriplet st = new StringTriplet("Универсал", FormatForExport_Litres(seed_universal_v), FormatForExport_Price(seed_universal_price));
            AddToExport(st);
            
            sum1 += seed_universal_price;
        }
        {
            string txt = "Итого: " + FormatPrice(sum1);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
            
            export_buffer.AppendLine("Итого: " + FormatForExport_Price(sum1));
            AddToExport(new StringTriplet("Итого: ", "sum_price", FormatForExport_Price(sum1)));
        }
        {
            string txt = "Фазы для обработок (рекомендации): между фазой высоты всходов 15 см. до окончания цветения:";
            MakeLabel(txt, ref current_y, ref delta_height);
            export_buffer.AppendLine(txt);
            AddToExport(new StringTriplet(txt, "title_1", ""));
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
            
            export_buffer.AppendLine("Мелафен (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_melafen_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_melafen_price));
            
            StringTriplet st = new StringTriplet("Мелафен (вегетация)", FormatForExport_Litres(veg_melafen_v), FormatForExport_Price(veg_melafen_price));
            AddToExport(st);
            
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
            
            export_buffer.AppendLine("Универсал (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_universal_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_universal_price));
            
            StringTriplet st = new StringTriplet("Универсал (вегетация)", FormatForExport_Litres(veg_universal_v), FormatForExport_Price(veg_universal_price));
            AddToExport(st);
            
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
            
            export_buffer.AppendLine("Бор и Молибден (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_bor_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_bor_price));
            
            StringTriplet st = new StringTriplet("Бор и Молибден (вегетация)", FormatForExport_Litres(veg_bor_v), FormatForExport_Price(veg_bor_price));
            AddToExport(st);
            
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
            
            export_buffer.AppendLine("Медь (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_med_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_med_price));
            
            StringTriplet st = new StringTriplet("Медь (вегетация)", FormatForExport_Litres(veg_med_v), FormatForExport_Price(veg_med_price));
            AddToExport(st);
            
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
            
            export_buffer.AppendLine("Цинк (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_zink_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_zink_price));
            
            StringTriplet st = new StringTriplet("Цинк (вегетация)", FormatForExport_Litres(veg_zink_v), FormatForExport_Price(veg_zink_price));
            AddToExport(st);
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
            
            export_buffer.AppendLine("Итого (вегетация): " + FormatForExport_Price(sum2));
            AddToExport(new StringTriplet("Итого (вегетация):", "sum_price", FormatForExport_Price(sum2)));
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
            export_buffer.AppendLine(txt);
            MakeLabel(txt, ref current_y, ref delta_height);
            
            AddToExport(new StringTriplet("Обработка семян:", "title_1", ""));
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
            
            export_buffer.AppendLine("Мелафен:");
            export_buffer.AppendLine(FormatForExport_Litres(seed_melafen_v));
            export_buffer.AppendLine(FormatForExport_Price(seed_melafen_price));
            
            StringTriplet st = new StringTriplet("Мелафен", FormatForExport_Litres(seed_melafen_v), FormatForExport_Price(seed_melafen_price));
            AddToExport(st);
            
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
            
            
            export_buffer.AppendLine("Универсал:");
            export_buffer.AppendLine(FormatForExport_Litres(seed_universal_v));
            export_buffer.AppendLine(FormatForExport_Price(seed_universal_price));
            
            StringTriplet st = new StringTriplet("Универсал", FormatForExport_Litres(seed_universal_v), FormatForExport_Price(seed_universal_price));
            AddToExport(st);
            
            sum1 += seed_universal_price;
        }
        {
            string txt = "Итого: " + FormatPrice(sum1);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
            
            export_buffer.AppendLine("Итого: " + FormatForExport_Price(sum1));
            AddToExport(new StringTriplet("Итого: ", "sum_price", FormatForExport_Price(sum1)));
        }
        {
            string txt = "Фазы для обработок (рекомендации): между фазой формирования листовой розетки до развития стручков:";
            MakeLabel(txt, ref current_y, ref delta_height);
            export_buffer.AppendLine(txt);
            
            AddToExport(new StringTriplet(txt, "title_1", ""));
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
            
            export_buffer.AppendLine("Мелафен (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_melafen_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_melafen_price));
            
            StringTriplet st = new StringTriplet("Мелафен (вегетация)", FormatForExport_Litres(veg_melafen_v), FormatForExport_Price(veg_melafen_price));
            AddToExport(st);
            
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
            
            export_buffer.AppendLine("Универсал (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_universal_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_universal_price));
            
            StringTriplet st = new StringTriplet("Универсал (вегетация)", FormatForExport_Litres(veg_universal_v), FormatForExport_Price(veg_universal_price));
            AddToExport(st);
            
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
            
            export_buffer.AppendLine("Бор и Молибден (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_bor_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_bor_price));
            
            StringTriplet st = new StringTriplet("Бор и Молибден (вегетация)", FormatForExport_Litres(veg_bor_v), FormatForExport_Price(veg_bor_price));
            AddToExport(st);
            
            sum2 += veg_bor_price;
        }
        {
            string txt = "Итого: " + FormatPrice(sum2);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
            
            export_buffer.AppendLine("Итого: " + FormatForExport_Price(sum2));
            AddToExport(new StringTriplet("Итого (вегетация):", "sum_price", FormatForExport_Price(sum2)));
        }
        
        FitContentSize(delta_height);
    }
    
    
    public bool canExport = false;
    
    
    
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
            export_buffer.AppendLine(txt);
            
            MakeLabel(txt, ref current_y, ref delta_height);
            AddToExport(new StringTriplet("Обработка семян:", "title_1", ""));
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
            
            export_buffer.AppendLine("Мелафен:");
            export_buffer.AppendLine(FormatForExport_Litres(seed_melafen_v));
            export_buffer.AppendLine(FormatForExport_Price(seed_melafen_price));
            
            StringTriplet st = new StringTriplet("Мелафен", FormatForExport_Litres(seed_melafen_v), FormatForExport_Price(seed_melafen_price));
            AddToExport(st);
            
            sum1 += seed_melafen_price;
        }
        {
            string txt = "Итого: " + FormatPrice(sum1);
            export_buffer.AppendLine("Итого: " + FormatForExport_Price(sum1));
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
            
            AddToExport(new StringTriplet("Итого: ", "sum_price", FormatForExport_Price(sum1)));
        }
        {
            string txt = "Фазы для обработок (рекомендации): между полностью развитые листья 1 узла и начала бобо-образования:";
            export_buffer.AppendLine(txt);
            MakeLabel(txt, ref current_y, ref delta_height);
            
            AddToExport(new StringTriplet("Обработки по вегетации, фазы: кущения, выход в трубку, колошения:", "title_1", ""));
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
            
            export_buffer.AppendLine("Мелафен (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_melafen_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_melafen_v));
            
            StringTriplet st = new StringTriplet("Мелафен (вегетация)", FormatForExport_Litres(veg_melafen_v), FormatForExport_Price(veg_melafen_price));
            AddToExport(st);
            
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
            
            export_buffer.AppendLine("Универсал (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_universal_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_universal_price));
            
            StringTriplet st = new StringTriplet("Универсал (вегетация)", FormatForExport_Litres(veg_universal_v), FormatForExport_Price(veg_universal_price));
            AddToExport(st);
            
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
            
            export_buffer.AppendLine("Бор и Молибден (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_bor_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_bor_price));
            
            StringTriplet st = new StringTriplet("Бор и Молибден (вегетация)", FormatForExport_Litres(veg_bor_v), FormatForExport_Price(veg_bor_price));
            AddToExport(st);
            
            sum2 += veg_bor_price;
        }
        {
            string txt = "Итого (вегетация): " + FormatPrice(sum2);
            export_buffer.AppendLine("Итого (вегетация): " + FormatForExport_Price(sum2));
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
            
            AddToExport(new StringTriplet("Итого: ", "sum_price", FormatForExport_Price(sum2)));
        }
        
        FitContentSize(delta_height);
    }
    
    public string FormatForExport_Litres(double a)
    {
        return FormatForExport(a) + " л";
    }
    
    public string FormatForExport_Price(double a)
    {
        return FormatForExport(a) + " руб";
    }
    
    public string FormatForExport(double a)
    {
        return a.ToString(ExportSpecifier, ExportCulture);
    }
    
    public string FormatForExport_Litres(float a)
    {
        return FormatForExport(a) + " л";
    }
    
    public string FormatForExport_Price(float a)
    {
        return FormatForExport(a) + " руб";
    }
    
    public string FormatForExport(float a)
    {
        return a.ToString(ExportSpecifier, ExportCulture);
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
            export_buffer.AppendLine(txt);
            
            MakeLabel(txt, ref current_y, ref delta_height);
            AddToExport(new StringTriplet("Обработка семян:", "title_1", ""));
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
            
            
            export_buffer.AppendLine("Мелафен:");
            export_buffer.AppendLine(FormatForExport_Litres(seed_melafen_v));
            export_buffer.AppendLine(FormatForExport_Price(seed_melafen_price));
            
            StringTriplet st = new StringTriplet("Мелафен", FormatForExport_Litres(seed_melafen_v), FormatForExport_Price(seed_melafen_price));
            AddToExport(st);
            
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
            
            export_buffer.AppendLine("Универсал:");
            export_buffer.AppendLine(FormatForExport_Litres(seed_universal_v));
            export_buffer.AppendLine(FormatForExport_Price(seed_universal_price));
            
            StringTriplet st = new StringTriplet("Универсал", FormatForExport_Litres(seed_universal_v), FormatForExport_Price(seed_universal_price));
            AddToExport(st);
            
            sum1 += seed_universal_price;
        }
        export_buffer.AppendLine(System.Environment.NewLine);
        {
            string txt = "Итого: " + FormatPrice(sum1);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
            
            
            export_buffer.AppendLine("Итого: " + FormatForExport_Price(sum1));
            AddToExport(new StringTriplet("Итого: ", "sum_price", FormatForExport_Price(sum1)));
        }
        export_buffer.AppendLine(System.Environment.NewLine);
        {
            string txt = "Обработки по вегетации, фазы: кущения, выход в трубку, колошения: ";
            MakeLabel(txt, ref current_y, ref delta_height);
            
            export_buffer.AppendLine(txt);
            AddToExport(new StringTriplet("Обработки по вегетации, фазы: кущения, выход в трубку, колошения:", "title_1", ""));
        }
        export_buffer.AppendLine(System.Environment.NewLine);
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
            
            export_buffer.AppendLine("Мелафен (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_melafen_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_melafen_price));
            
            StringTriplet st = new StringTriplet("Мелафен (вегетация)", FormatForExport_Litres(veg_melafen_v), FormatForExport_Price(veg_melafen_price));
            AddToExport(st);
            
            sum2 += veg_melafen_price;
        }
        export_buffer.AppendLine(System.Environment.NewLine);
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
            
            export_buffer.AppendLine("Универсал (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_universal_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_universal_price));
            
            StringTriplet st = new StringTriplet("Универсал (вегетация)", FormatForExport_Litres(veg_universal_v), FormatForExport_Price(veg_universal_price));
            AddToExport(st);
            
            sum2 += veg_universal_price;
        }
        export_buffer.AppendLine(System.Environment.NewLine);
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
            
            
            export_buffer.AppendLine("Бор и Молибден (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_bor_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_bor_price));
            
            StringTriplet st = new StringTriplet("Бор и Молибден (вегетация)", FormatForExport_Litres(veg_bor_v), FormatForExport_Price(veg_bor_price));
            AddToExport(st);
            
            sum2 += veg_bor_price;
        }
        export_buffer.AppendLine(System.Environment.NewLine);
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
            
            export_buffer.AppendLine("Медь (вегетация):");
            export_buffer.AppendLine(FormatForExport_Litres(veg_med_v));
            export_buffer.AppendLine(FormatForExport_Price(veg_med_price));
            
            StringTriplet st = new StringTriplet("Медь (вегетация)", FormatForExport_Litres(veg_med_v), FormatForExport_Price(veg_med_price));
            AddToExport(st);
            
            sum2 += veg_med_price;
        }
        export_buffer.AppendLine(System.Environment.NewLine);
        {
            string txt = "Итого: " + FormatPrice(sum2);
            GameObject sum_label = MakeLabel(txt, ref current_y, ref delta_height);
            sum_label.GetComponent<Image>().color = new Color(188f/255f, 217f/255f, 67f/255f);
            
            export_buffer.AppendLine("Итого (вегетация):");
            export_buffer.AppendLine(FormatForExport_Price(sum2));
            
            AddToExport(new StringTriplet("Итого (вегетация):", "sum_price", FormatForExport_Price(sum2)));
        }
        
        Debug.Log(export_buffer.ToString());
        
        FitContentSize(delta_height);
    }
    
    StringBuilder export_buffer = new StringBuilder();
    
    GameObject MakeLabel(string txt, ref float yCoord, ref float yDeltaHeight)
    {
        float heightMult = 1;
        if(txt.Length > 32)
        {
            heightMult = 2;
        }
        
        GameObject label = Instantiate(labelItemPrefab, Vector3.zero, Quaternion.identity, content_rect);
        
        Label_calculated lc = label.GetComponent<Label_calculated>();
        lc.SetText(txt);
        
        RectTransform rectTransform;
        calculatedItems.Add(label);
        
        rectTransform = label.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, yCoord);
        rectTransform.sizeDelta = new Vector2(750, y_label_height * heightMult);
        
        yCoord -= y_step_size + y_label_height * heightMult;
        yDeltaHeight -= y_step_size + y_label_height * heightMult;
        
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
        string s = FormatMillion((val).ToString());
        StringBuilder sb = new StringBuilder(s);
        
        sb.Append(" руб");
        
        return sb.ToString();
    }
    
    string FormatPrice(double val)
    {
        string s = FormatMillion((val).ToString());
        StringBuilder sb = new StringBuilder(s);
        
        sb.Append(" руб");
        
        return sb.ToString();
    }
    
    string FormatPrice(decimal val)
    {
        string s = FormatMillion((val).ToString());
        StringBuilder sb = new StringBuilder(s);
        
        sb.Append(" руб");
        
        return sb.ToString();
    }
    
    string FormatLitres(float val)
    {
        //int val_int = Mathf.CeilToInt(val);
        
        string s = (val).ToString();
        StringBuilder sb = new StringBuilder(s);
        
        sb.Append(" л");
        
        return sb.ToString();
    }
    
    string FormatLitres(double val)
    {
        //int val_int = Mathf.CeilToInt(val);
        
        string s = (val).ToString();
        StringBuilder sb = new StringBuilder(s);
        
        sb.Append(" л");
        
        return sb.ToString();
    }
    
    string FormatLitres(decimal val)
    {
        //int val_int = Mathf.CeilToInt(val);
        
        string s = (val).ToString();
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
        
        //s.Contains(",");
        
        if(s.Length < 4)
        {
            return s;
        }
        
        int k = 0;
        for(int i = s.Length-1; i >= 0; i--)
        {
            k++;
            if(k % 3 == 0 && s[i] != '.' && s[i] != ',')
            {
                k = 0;
                if(i != 0)
                    Result.Insert(i, " ");
            }
        }
        
        return Result.ToString();
    }
    
    public void ReadStandardsFromDisk(Culture cult)
    {
        switch(cult)
        {
            case(Culture.None):
            {
                break;
            }
            case(Culture.Ozimaya_pshenica):
            {
                standards[0] = ReadSingleStandardFromDisk(standards[0].name);
                standards[1] = ReadSingleStandardFromDisk(standards[1].name);
                standards[2] = ReadSingleStandardFromDisk(standards[2].name);
                standards[3] = ReadSingleStandardFromDisk(standards[3].name);
                
                break;
            }
            case(Culture.Soy):
            {
                standards[4] = ReadSingleStandardFromDisk(standards[4].name);
                standards[5] = ReadSingleStandardFromDisk(standards[5].name);
                standards[6] = ReadSingleStandardFromDisk(standards[6].name);
                
                break;
            }
            case(Culture.Raps):
            {
                standards[7] = ReadSingleStandardFromDisk(standards[7].name);
                standards[8] = ReadSingleStandardFromDisk(standards[8].name);
                standards[9] = ReadSingleStandardFromDisk(standards[9].name);
                standards[10] = ReadSingleStandardFromDisk(standards[10].name);
                
                break;
            }
            case(Culture.Kartofel):
            {
                standards[11] = ReadSingleStandardFromDisk(standards[11].name);
                standards[12] = ReadSingleStandardFromDisk(standards[12].name);
                standards[13] = ReadSingleStandardFromDisk(standards[13].name);
                standards[14] = ReadSingleStandardFromDisk(standards[14].name);
                standards[15] = ReadSingleStandardFromDisk(standards[15].name);
                standards[16] = ReadSingleStandardFromDisk(standards[16].name);
                
                
                break;
            }
            case(Culture.Kukuruza):
            {
                standards[17] = ReadSingleStandardFromDisk(standards[17].name);
                standards[18] = ReadSingleStandardFromDisk(standards[18].name);
                standards[19] = ReadSingleStandardFromDisk(standards[19].name);
                standards[20] = ReadSingleStandardFromDisk(standards[20].name);
                standards[21] = ReadSingleStandardFromDisk(standards[21].name);
                standards[22] = ReadSingleStandardFromDisk(standards[22].name);
                
                break;
            }
            case(Culture.Podsolnechnik):
            {
                standards[23] = ReadSingleStandardFromDisk(standards[23].name);
                standards[24] = ReadSingleStandardFromDisk(standards[24].name);
                standards[25] = ReadSingleStandardFromDisk(standards[25].name);
                standards[26] = ReadSingleStandardFromDisk(standards[26].name);
                standards[27] = ReadSingleStandardFromDisk(standards[27].name);
                standards[28] = ReadSingleStandardFromDisk(standards[28].name);
                
                break;
            }
            case(Culture.Xlopchatnik):
            {
                standards[29] = ReadSingleStandardFromDisk(standards[29].name);
                standards[30] = ReadSingleStandardFromDisk(standards[30].name);
                standards[31] = ReadSingleStandardFromDisk(standards[31].name);
                standards[32] = ReadSingleStandardFromDisk(standards[32].name);
                standards[33] = ReadSingleStandardFromDisk(standards[33].name);
                standards[34] = ReadSingleStandardFromDisk(standards[34].name);
                
                break;
            }
            case(Culture.Svekla):
            {
                standards[35] = ReadSingleStandardFromDisk(standards[35].name);
                standards[36] = ReadSingleStandardFromDisk(standards[36].name);
                standards[37] = ReadSingleStandardFromDisk(standards[37].name);
                standards[38] = ReadSingleStandardFromDisk(standards[38].name);
                standards[39] = ReadSingleStandardFromDisk(standards[39].name);
                
                break;
            }
            
        }
        Debug.Log(string.Format("<color=yellow>Read</color> standard of {0}.", cult));
        
    }
    
    Standard ReadSingleStandardFromDisk(string _name)
    {
        string standardAsString;
        Standard st;
        
        standardAsString = PlayerPrefs.GetString(_name);
        st = JsonUtility.FromJson<Standard>(standardAsString);
        
        return st;
    }
    
    public void SaveStandardsIndexSection(int start, int end)
    {
        for(int i = start; i <= end; i++)
        {
            string standard = JsonUtility.ToJson(standards[i]);
            PlayerPrefs.SetString(standards[i].name, standard);
        }
        
        PlayerPrefs.Save();
        Debug.Log(string.Format("<color=green>Saved</color> from <color=yellow>{0}</color> to <color=yellow>{1}</color> standards!", start, end));
    }
    
    public void SaveAllStandards()
    {
        for(int i = 0; i < standards.Length; i++)
        {
            string standard = JsonUtility.ToJson(standards[i]);
            PlayerPrefs.SetString(standards[i].name, standard);
        }
        
        PlayerPrefs.Save();
        Debug.Log("<color=green>Saved</color> <color=yellow>ALL</color> standards!");
    }
    
    
    [Header("Backup:")]
    public Standard[] backupStandards;
    
    
    public void MakeStandardsBackupSection(int start, int end)
    {
        for(int i = start; i <= end; i++)
        {
            standards[i] = backupStandards[i];
        }
        Debug.Log(string.Format("<color=green>MakeStandardsBackupSection</color> from <color=yellow>{0}</color> to <color=yellow>{1}</color> standards!", start, end));
    }
    
    public void MakeStandardsBackup()
    {
        backupStandards = (Standard[])standards.Clone();
    }
        
}
