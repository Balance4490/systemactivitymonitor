using System.Collections.Generic;
using System.Text;
using SystemActivityMonitor.Data.Entities;

namespace SystemActivityMonitor.Data.Patterns.Bridge
{
    public class HtmlRenderer : IReportRenderer
    {
        public string RenderReport(string title, List<ResourceLog> logs)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<html><body>");
            sb.AppendLine($"<h1>{title}</h1>");
            sb.AppendLine("<table border='1' cellpadding='5' cellspacing='0'>");
            sb.AppendLine("<tr style='background-color: #f2f2f2;'><th>Time</th><th>CPU Load</th><th>RAM Usage</th></tr>");

            foreach (var log in logs)
            {
                string color = log.CpuLoad > 80 ? "color:red; font-weight:bold;" : "";
                
                sb.AppendLine("<tr>");
                sb.AppendLine($"<td>{log.CreatedAt.ToShortTimeString()}</td>");
                sb.AppendLine($"<td style='{color}'>{log.CpuLoad}%</td>");
                sb.AppendLine($"<td>{log.RamUsage} MB</td>");
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</table>");
            sb.AppendLine($"<p><i>Generated at {System.DateTime.Now}</i></p>");
            sb.AppendLine("</body></html>");
            return sb.ToString();
        }
    }
}