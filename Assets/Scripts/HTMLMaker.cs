using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Text;

public class HTMLMaker //: MonoBehaviour
{
    static string PageHead_Calculator()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<!DOCTYPE html>");
        
        sb.AppendLine("<html lang=\"en\">");
        sb.AppendLine("<head>");
        
        sb.AppendLine("<meta name=\"description\" content=\"\" />");
        sb.AppendLine("<meta charset=\"utf-8\">");
        sb.AppendLine("<title>Результаты расчетов продукции Агриферт</title>");
        
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
    
    static string MakeTable_FromCalculator(string calculator_table_name, ref List<StringTriplet> raw)
    {
        StringBuilder sb = new StringBuilder();
        //string.Format("Расчет продукции для культуры \"{0}\"", calculator_table_name); 
        sb.AppendLine("<p align=\"center\"  style=\"color:green;font-size:36px;\"><b>Агриферт</b></p>");
        sb.AppendLine(string.Format("<p align=\"center\"  style=\"color:green;font-size:30px;\"><b>Расчет продукции для культуры \"{0}\"</b></p>", calculator_table_name));
        sb.AppendLine("<table bgcolor=\"#f4ffc2\" width=\"50%\" border=\"2\" bordercolor=\"#0d4a18\" align=\"center\" cellpadding=\"10\">");
        sb.AppendLine("<tr>");
        sb.AppendLine("<th>Продукт</th>");
        sb.AppendLine("<th>Объем</th>");
        sb.AppendLine("<th>Цена</th>");
        sb.AppendLine("</tr>");
        
        foreach(StringTriplet entry in raw)
        {
            if(entry.v2 == "title_1")
            {
                // string cell1 = entry.v1;
                // string cell2 = "КЕК";
                // sb.AppendLine(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", cell1, cell2));
                
                string cell1 = entry.v1;
                sb.AppendLine(string.Format("<th colspan=\"3\" style=\"font-size:110%\" bgcolor=\"#caf0c0\">{0}</th>", cell1));
            }
            else
            {
                if(entry.v2 == "sum_price")
                {
                    string cell1 = entry.v1;
                    string cell2 = "-";
                    string cell3 = entry.v3;
                    sb.AppendLine(string.Format("<tr style=\"font-size:110%\" bgcolor=\"#e3f786\"><td><b>{0}</b></td><td align=\"center\">{1}</td><td align=\"center\"><b>{2}</b></td></tr>", cell1, cell2, cell3));
                }
                else
                {
                    string cell1 = entry.v1;
                    string cell2 = entry.v2;
                    string cell3 = entry.v3;
                    sb.AppendLine(string.Format("<tr><td>{0}</td><td align=\"center\">{1}</td><td align=\"center\">{2}</td></tr>", cell1, cell2, cell3));
                }
            }
        }
        
        sb.AppendLine("</table>");
        sb.AppendLine("<p style=\"font-size:200%\" align=\"center\">");
        string date = System.DateTime.Now.ToString("dd/MM/yy");
        sb.AppendLine(string.Format("Дата отчета {0}", date));
        sb.AppendLine("</p>");
        
        return sb.ToString();
    }
    
    static string PageHead_Survey()
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
    
    public static string MakeHTMLPage_FromCalculator(string calculator_page_name, ref List<StringTriplet> calculator_results)
    {
        string Result = string.Empty;
        
        string head = PageHead_Calculator();
        string content = MakeTable_FromCalculator(calculator_page_name, ref calculator_results);
        string end = PageEnd();
        
        Result = head + content + end;
        
        return Result;
    }
    
    public static string MakeHTMLPage_FromSurvey(ref List<StringPair> survey_results)
    {
        string Result = string.Empty;
        
        string head = PageHead_Survey();
        string content = MakeTable_FromSurvey(ref survey_results);
        string end = PageEnd();
        
        Result = head + content + end;
        
        return Result;
    }
    
    static string MakeTable_FromSurvey(ref List<StringPair> raw)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<p align=\"center\"  style=\"color:green;font-size:30px;\"><b>Результаты опросника клиента Агриферт</b></p>");
        sb.AppendLine("<table bgcolor=\"#f4ffc2\" width=\"45%\" border=\"2\" bordercolor=\"#0d4a18\" align=\"center\" cellpadding=\"10\">");
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
