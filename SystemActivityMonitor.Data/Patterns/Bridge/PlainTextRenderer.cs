using System.Collections.Generic;
using System.Text;
using SystemActivityMonitor.Data.Entities;

namespace SystemActivityMonitor.Data.Patterns.Bridge
{
    public class PlainTextRenderer : IReportRenderer
    {
        public string RenderReport(string title, List<ResourceLog> logs)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"=== {title} ===");
            sb.AppendLine($"Згенеровано: {System.DateTime.Now}");
            sb.AppendLine("--------------------------------------------------");
            
            foreach (var log in logs)
            {
                sb.AppendLine($"TIME: {log.CreatedAt.ToShortTimeString()} | CPU: {log.CpuLoad}% | RAM: {log.RamUsage} MB");
            }
            
            sb.AppendLine("--------------------------------------------------");
            sb.AppendLine("Кінець звіту.");
            return sb.ToString();
        }
    }
}