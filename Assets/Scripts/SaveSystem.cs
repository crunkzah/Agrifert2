﻿using System.Text;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    // public static void Print(string name)
    // {
    //     string savePath = Application.persistentDataPath;
        
    //     string full_path = savePath + "/" + name;
        
        
    //     Debug.Log(string.Format("<color=yellow>Saving file path is {0}</color>", full_path));
    // }
    
    
    
    public static void SaveTxt(StringBuilder sb, string result_name)
    {
        string savePath = Application.persistentDataPath;
        string name = "Агриферт_" + result_name; 
        
        string dir_name = "Agrifert_exported_results";
        
        if(!Directory.Exists(savePath + Path.DirectorySeparatorChar + dir_name))
        {
            Directory.CreateDirectory(savePath + Path.DirectorySeparatorChar + dir_name);
        }
        
        string full_path = savePath + Path.DirectorySeparatorChar + dir_name + Path.DirectorySeparatorChar + name;
        
        string day = System.DateTime.Now.ToString("dd/MM/yy");
        
        full_path += "_" + day;
        
        full_path += ".txt";
        
        Debug.Log(string.Format("<color=yellow>Saving file path is {0}</color>", full_path));
        
        StreamWriter sw = File.CreateText(full_path);
        sw.Write(sb.ToString());
        sw.Close();
        
        //UI_Manager.ShowMessage("Файл сохранен!");
    }
}
