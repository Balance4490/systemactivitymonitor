using System.Collections.Generic;
using SystemActivityMonitor.Data.Entities;

namespace SystemActivityMonitor.Data.Patterns.Bridge
{
    public interface IReportRenderer
    {
        string RenderReport(string title, List<ResourceLog> logs);
    }
}