using System;
using System.Linq;
using SystemActivityMonitor.Data.Entities;
using SystemActivityMonitor.Data.Patterns.AbstractFactory;

namespace SystemActivityMonitor.Data.Patterns.Command
{
    public class SystemController
    {
        public void ClearAllData()
        {
            using (var db = new MonitorDbContext())
            {
                db.ResourceLogs.RemoveRange(db.ResourceLogs);
                db.Sessions.RemoveRange(db.Sessions);
                db.SaveChanges();
            }
        }

        public void GenerateDataWithFactory(IMonitorFactory factory)
        {
            ICpuSensor cpuSensor = factory.CreateCpuSensor();
            IRamSensor ramSensor = factory.CreateRamSensor();

            using (var db = new MonitorDbContext())
            {
                var admin = db.Users.FirstOrDefault(u => u.Username == "admin");
                if (admin == null) return;

                var session = new Session
                {
                    UserId = admin.Id,
                    MachineName = "FULL-PC",
                    OSVersion = "Windows 11 Pro"
                };
                db.Sessions.Add(session);
                db.SaveChanges();

                var rnd = new Random();
                string[] windows = { "Google Chrome", "Visual Studio", "Google Chrome", "Telegram", "Word" };

                for (int i = 0; i < 10; i++)
                {
                    db.ResourceLogs.Add(new ResourceLog
                    {
                        SessionId = session.Id,
                        CpuLoad = cpuSensor.GetCpuLoad(),
                        RamUsage = ramSensor.GetFreeRam(),
                        ActiveWindow = windows[rnd.Next(windows.Length)],
                        CreatedAt = DateTime.UtcNow.AddSeconds(i * 2)
                    });
                }

                string[] keyActions = { "Ctrl+C", "Ctrl+V", "Enter", "Alt+Tab", "Space" };
                string[] mouseActions = { "Left Click", "Right Click", "Scroll Down", "Double Click" };

                for (int i = 0; i < 5; i++)
                {
                    bool isKeyboard = rnd.Next(0, 2) == 0;

                    db.InputEvents.Add(new InputEvent
                    {
                        SessionId = session.Id,
                        EventType = isKeyboard ? "Keyboard" : "Mouse",
                        Details = isKeyboard
                            ? keyActions[rnd.Next(keyActions.Length)]
                            : mouseActions[rnd.Next(mouseActions.Length)],
                        CreatedAt = DateTime.UtcNow.AddSeconds(i * 3)
                    });
                }

                db.SaveChanges();
            }
        }
    }
}