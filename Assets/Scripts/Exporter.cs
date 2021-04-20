using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;


public class Exporter : MonoBehaviour
{
    [TextArea(25, 250)]
    public string startXML_text = "";
    [TextArea(15, 250)]
    public string endXML_text = "";
    
    
    void Start()
    {
        // XmlDocument doc = new XmlDocument();
        // doc.CreateAttribute("KEK1");
        // doc.CreateAttribute("KEK2");
        // doc.CreateAttribute("KEK3");
        // XmlNode node = new XmlNode();
        
        // Debug.Log(doc.ToString());
    }
}
