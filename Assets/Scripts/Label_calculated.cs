using UnityEngine;
using TMPro;

public class Label_calculated : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public void SetText(string txt)
    {
        tmp.SetText(txt);
    }
}
