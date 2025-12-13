using System.Collections.Generic;
using SystemActivityMonitor.Data.Entities;

namespace SystemActivityMonitor.Data.Patterns.Bridge
{
    public class DailyReport : ReportAbstraction
    {
        public DailyReport(IReportRenderer renderer) : base(renderer)
        {
        }

        public override string Generate(List<ResourceLog> logs)
        {

            return _renderer.RenderReport("Щоденний Звіт Системи", logs);
        }
    }
}