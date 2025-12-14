using System;

namespace SystemActivityMonitor.Data.Entities
{
    public class ResourceLog : BaseEntity
    {
        public Guid SessionId { get; set; }
        public float CpuLoad { get; set; } 
        public float RamUsage { get; set; } 
        public string ActiveWindow { get; set; }
        public bool IsSystemIdle { get; set; }
        public Session Session { get; set; }
    }
}