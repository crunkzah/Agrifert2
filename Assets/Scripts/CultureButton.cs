using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CultureButton : MonoBehaviour
{

    public string cultureName;

    public TextMeshProUGUI tmp;
    public Image cultureImage;

    public void OnClick()
    {
        if (culture_item_setter == null)
        {
            culture_item_setter = FindObjectOfType<CultureItemSetter>();
        }

        culture_item_setter.SetCultureItem(cultureName);
    }

    CultureItemSetter culture_item_setter;

    void SetItem()
    {
        if(culture_item_setter == null)
        {
            culture_item_setter = FindObjectOfType<CultureItemSetter>();
        }

        CultureItem culture = culture_item_setter.GetCultureItem(cultureName);

        if(tmp == null)
        {
            tmp = GetComponentInChildren<TextMeshProUGUI>(true);
        }
        tmp.SetText(culture.Nomination);
        cultureImage.sprite = culture.culture_sprite;        
    }

    void OnEnable()
    {
        SetItem();
    }


}
