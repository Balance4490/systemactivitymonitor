using System.Linq;
using System.Windows;
using SystemActivityMonitor.Data;
using SystemActivityMonitor.Data.Patterns.Iterator; // Використовуємо наш патерн!

namespace SystemActivityMonitor.UI
{
    public partial class UserWindow : Window
    {
        public UserWindow(string username)
        {
            InitializeComponent();
            txtUserWelcome.Text = $"Вітаємо, {username}!";
            
            // Автоматично завантажуємо дані при вході
            LoadData();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            lstUserLogs.Items.Clear();
            LogCollection collection = new LogCollection();

            using (var db = new MonitorDbContext())
            {
                // Юзер бачить останні 20 записів системи
                var logsFromDb = db.ResourceLogs.OrderByDescending(l => l.CreatedAt).Take(20).ToList();
                foreach (var log in logsFromDb)
                {
                    collection.Add(log);
                }
            }

            // Використовуємо наш ITERATOR (Патерн працює і тут!)
            IIterator iterator = collection.CreateIterator();
            iterator.First();
            while (!iterator.IsDone())
            {
                var item = iterator.CurrentItem();
                if (item != null)
                {
                    // Формат виводу
                    string displayText = $"[{item.CreatedAt.ToShortTimeString()}] {item.ActiveWindow} | CPU: {item.CpuLoad}% | RAM: {item.RamUsage} MB";
                    lstUserLogs.Items.Add(displayText);
                }
                iterator.Next();
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}