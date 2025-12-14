namespace SystemActivityMonitor.Data.Patterns.Visitor
{
    public class CpuMetric : IMetricElement
    {
        public float LoadPercent { get; set; }
        public string Time { get; set; }
        public string AppName { get; set; }
        public bool IsIdle { get; set; }

        public CpuMetric(float load, string time, string appName, bool isIdle)
        {
            LoadPercent = load;
            Time = time;
            AppName = appName;
            IsIdle = isIdle;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.VisitCpu(this);
        }
    }
}