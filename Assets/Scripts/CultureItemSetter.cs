using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CultureItemSetter : MonoBehaviour
{
    public CultureItem[] allCultureItems;

    public CultureItemReader cultureItemReader;


    public Image cultureImage_on_singleCanvas;
    public TextMeshProUGUI cultureNomination_on_singleCanvas;

    public CultureItem GetCultureItem(string itemName)
    {
        CultureItem item = null;

        for(int i = 0; i < allCultureItems.Length; i++)
        {
            if(itemName.Equals(allCultureItems[i].Nomination))
            {
                item = allCultureItems[i];
                break;
            }
        }

        return item;
    }

    public void SetCultureItem(string itemName)
    {
        CultureItem itemToSet = null;

        for(int i = 0; i < allCultureItems.Length; i++)
        {
            if(itemName == allCultureItems[i].Nomination)
            {

                itemToSet = allCultureItems[i];
                break;
            }
        }

        cultureItemReader.item = itemToSet;
        
        cultureImage_on_singleCanvas.sprite = itemToSet.culture_sprite;
        cultureNomination_on_singleCanvas.SetText(itemToSet.Nomination);
    }

    

}
