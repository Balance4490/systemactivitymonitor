using System.Text;

namespace SystemActivityMonitor.Data.Patterns.Visitor
{
    public class AnalysisVisitor : IVisitor
    {
        private float _totalCpu = 0;
        private int _count = 0;
        private int _chromeCount = 0;
        private int _idleCount = 0;
        private float _minRam = float.MaxValue;

        public void VisitCpu(CpuMetric cpu)
        {
            _totalCpu += cpu.LoadPercent;
            _count++;

            if (cpu.AppName == "Google Chrome")
                _chromeCount++;

            if (cpu.IsIdle)
                _idleCount++;
        }

        public void VisitRam(RamMetric ram)
        {
            if (ram.FreeMemoryMb < _minRam)
                _minRam = ram.FreeMemoryMb;
        }

        public string GetStats()
        {
            float avgCpu = _count > 0 ? _totalCpu / _count : 0;
            float browserPercent = _count > 0 ? ((float)_chromeCount / _count) * 100 : 0;
            float idlePercent = _count > 0 ? ((float)_idleCount / _count) * 100 : 0;

            return $"Аналіз завершено:\n" +
                   $" - Середнє CPU: {avgCpu:F2}%\n" +
                   $" - Час у браузері: {browserPercent:F2}%\n" +
                   $" - Час простою (Idle): {idlePercent:F2}%\n" +
                   $" - Мінімум RAM: {_minRam} MB";
        }
    }
}