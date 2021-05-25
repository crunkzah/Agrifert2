using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CultureItemReader : MonoBehaviour
{

    public CultureItem item;
    public VerticalLayoutCustom vlc;
    public ScrollRect_fix sr;

    [Header("Prefabs:")]
    public GameObject title_prefab;
    public GameObject text_prefab;
    public GameObject image_prefab;

    [SerializeField]  int paragraphs_index = -1;

    public static string GetCultureName(Culture _item)
    {
        string Result = "Название_Культуры";
        
        switch(_item)
        {
            case(Culture.Ozimaya_pshenica):
            {
                Result = "ОзимаяПшеница";
                break;
            }
            case(Culture.Kartofel):
            {
                Result = "Картофель";
                break;
            }
            case(Culture.Kukuruza):
            {
                Result = "Кукуруза";
                break;
            }
            case(Culture.Podsolnechnik):
            {
                Result = "Подсолнечник";
                break;
            }
            case(Culture.Raps):
            {
                Result = "Рапс";
                break;
            }
            case(Culture.Soy):
            {
                Result = "Соя";
                break;
            }
            case(Culture.Svekla):
            {
                Result = "СахарнаяСвёкла";
                break;
            }
            case(Culture.Xlopchatnik):
            {
                Result = "Хлопчатник";
                break;
            }
            default:
            {
                break;
            }
        }
        
        return Result;
    }
    
    public static string GetCultureNamePretty(Culture _item)
    {
        string Result = "Название_Культуры";
        
        switch(_item)
        {
            case(Culture.Ozimaya_pshenica):
            {
                Result = "Озимая пшеница";
                break;
            }
            case(Culture.Kartofel):
            {
                Result = "Картофель";
                break;
            }
            case(Culture.Kukuruza):
            {
                Result = "Кукуруза";
                break;
            }
            case(Culture.Podsolnechnik):
            {
                Result = "Подсолнечник";
                break;
            }
            case(Culture.Raps):
            {
                Result = "Рапс";
                break;
            }
            case(Culture.Soy):
            {
                Result = "Соя";
                break;
            }
            case(Culture.Svekla):
            {
                Result = "Сахарная свекла";
                break;
            }
            case(Culture.Xlopchatnik):
            {
                Result = "Хлопчатник";
                break;
            }
            default:
            {
                break;
            }
        }
        
        return Result;
    }


    void Awake()
    {
        if(labelNomination == null)
        {
            Debug.LogError("Name label is null !!!");
        }

        if(vlc == null)
        {
            Debug.LogError("VerticalLayoutCustom is null !!!");
        }
    }

    public void SetParagraphIndex(int index)
    {
        this.paragraphs_index = index;
    }

    void OnEnable()
    {
        if(item != null)
        {
            labelNomination.SetText(item.Nomination);    
            sr.verticalNormalizedPosition = 1f;
            cultureImage.sprite = item.culture_sprite;

            Paragraph[] paragraphs = GetNeededParagraphList(paragraphs_index, item);
            SetContent(ref paragraphs, vlc);   
        }
    }
    
    GameObject header_image;
    


    Paragraph[] GetNeededParagraphList(int paragraph, CultureItem item)
    {
        Paragraph[] p = null;

        switch (paragraph)
        {
            case 0:
                p = item.summary_paragraphs;
                
                break;
            case 1:
                p = item.elements_paragraphs;
                
                break;
            case 2:
                p = item.phases_paragraphs;
                

                break;
            case 3:
                p = item.cost_to_use_paragraph;
                
                break;
            default:
                Debug.LogError("Paragraph with number " + paragraph.ToString() + " is not handled !!!");
                break;
        }

        return p;
    }

    void SetContent(ref Paragraph[] paragraphs, VerticalLayoutCustom vlc)
    {
        foreach(Transform child in vlc.transform)
        {
            GameObject child_to_delete = child.gameObject;
            if(!child_to_delete.name.Equals("Image_header"))
            {
//                print("Destroying " + child_to_delete.name);
                Destroy(child_to_delete);
            }
            else
            {
                header_image = child_to_delete;
            }
        }



        List<Transform> added_objects = new List<Transform>();
        
        
        foreach(Paragraph p in paragraphs)
        {
            if(!p.title.Equals("SKIP"))
            {
                GameObject p_title = Instantiate(title_prefab, Vector3.zero, Quaternion.identity);
                p_title.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(p.title);
                p_title.transform.SetParent(vlc.transform);

                added_objects.Add(p_title.transform);
            }

            if(p.image == null)
            {
                GameObject p_text = Instantiate(text_prefab, Vector3.zero, Quaternion.identity, vlc.GetComponent<RectTransform>());

                p_text.GetComponent<TextMeshProUGUI>().SetText(p.text);
                added_objects.Add(p_text.transform);
            }
            else
            {
                GameObject p_image = Instantiate(image_prefab, Vector3.zero, Quaternion.identity, vlc.GetComponent<RectTransform>());
                p_image.GetComponent<Image>().sprite = p.image;
                p_image.GetComponent<Image>().SetNativeSize();

                added_objects.Add(p_image.transform);
            }
        }
            
        
        foreach(Transform t in added_objects)
        {
            //t.SetAsFirstSibling();
            t.localScale = Vector3.one;
            
        }

        header_image.transform.SetAsFirstSibling();

        vlc.ResizeTextContainers();
        vlc.AlignElements();
        vlc.ResizeContainer();

        vlc.updateNeeded = true;
    }

    [Header("GUI mapping:")]
    public TextMeshProUGUI labelNomination;
    public Image           cultureImage;
}
