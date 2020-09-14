using UnityEngine;
using TMPro;


[System.Serializable]
public struct Paragraph
{
    public string       title;

    [TextArea(15, 250)]
    public string       text;
    
    public Sprite    image; 
    //public TextMeshProUGUI tmp;
}

[CreateAssetMenu(fileName="Culture item")]
public class CultureItem : ScriptableObject
{
    public string    Nomination  = "Название культуры";
    public Sprite    culture_sprite;
    public Paragraph[]  paragraphs;


    public Paragraph[]  summary_paragraphs;
    public Paragraph[]  elements_paragraphs;
    public Paragraph[]  phases_paragraphs;
    public Paragraph[]  cost_to_use_paragraph;
    
}