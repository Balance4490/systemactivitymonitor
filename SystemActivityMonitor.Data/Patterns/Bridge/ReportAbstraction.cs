using System.Collections.Generic;
using SystemActivityMonitor.Data.Entities;

namespace SystemActivityMonitor.Data.Patterns.Bridge
{
    public abstract class ReportAbstraction
    {
        protected IReportRenderer _renderer; 

        public ReportAbstraction(IReportRenderer renderer)
        {
            _renderer = renderer;
        }

        public abstract string Generate(List<ResourceLog> logs);
    }
}