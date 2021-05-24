using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Text;

public class HTMLMaker //: MonoBehaviour
{
    static string PageHead()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<!DOCTYPE html>");
        
        sb.AppendLine("<html lang=\"en\">");
        sb.AppendLine("<head>");
        
        sb.AppendLine("<meta name=\"description\" content=\"\" />");
        sb.AppendLine("<meta charset=\"utf-8\">");
        sb.AppendLine("<title>Отчет Агриферт</title>");
        
        sb.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
        sb.AppendLine("<link rel=\"stylesheet\" href=\"css/style.css\">");
        sb.AppendLine("<style>");
        sb.AppendLine(@"body{background-color: #FDFDFD;}");
        sb.AppendLine(@"table{ border-collapse: collapse;}");
        sb.AppendLine("</style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");
        
        return sb.ToString();
    }
    
    public static string MakeHTMLPage_FromSurvey(ref List<StringPair> survey_results)
    {
        string Result = string.Empty;
        
        string head = PageHead();
        string content = MakeTable_FromSurvey(ref survey_results);
        string end = PageEnd();
        
        Result = head + content + end;
        
        return Result;
    }
    
    static string MakeTable_FromSurvey(ref List<StringPair> raw)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<p align=\"center\"  style=\"color:green;font-size:30px;\"><b>Результаты опросника клиента Агриферт</b></p>");
        sb.AppendLine("<table width=\"45%\" border=\"2\" bordercolor=\"#0d4a18\" align=\"center\" cellpadding=\"10\">");
        sb.AppendLine("<tr>");
        sb.AppendLine("<th>Метрика (вопрос)</th>");
        sb.AppendLine("<th>Значение</th>");
        sb.AppendLine("</tr>");
        
        foreach(StringPair entry in raw)
        {
            if(entry.v2 == "title")
            {
                // string cell1 = entry.v1;
                // string cell2 = "КЕК";
                // sb.AppendLine(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", cell1, cell2));
                
                string cell1 = entry.v1;
                sb.AppendLine(string.Format("<th colspan=\"2\" style=\"font-size:110%\" bgcolor=\"#caf0c0\">{0}</th>", cell1));
            }
            else
            {
                string cell1 = entry.v1;
                string cell2 = entry.v2;
                sb.AppendLine(string.Format("<tr><td>{0}</td><td align=\"center\">{1}</td></tr>", cell1, cell2));
            }
        }
        
        sb.AppendLine("</table>");
        sb.AppendLine("<p style=\"font-size:200%\" align=\"center\">");
        string date = System.DateTime.Now.ToString("dd/MM/yy");
        sb.AppendLine(string.Format("Дата отчета {0}", date));
        sb.AppendLine("</p>");
        
        return sb.ToString();
    }
    
    static string PageEnd()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");
        
    
        
        return sb.ToString();
    }
    
}
