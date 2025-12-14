using System;
using System.Linq;
using System.Windows;
using SystemActivityMonitor.Data;
using SystemActivityMonitor.Data.Patterns.Iterator;
using SystemActivityMonitor.Data.Patterns.Command;
using SystemActivityMonitor.Data.Patterns.AbstractFactory;
using SystemActivityMonitor.Data.Patterns.Bridge;
using System.Windows.Controls;
using System.Collections.Generic;
using SystemActivityMonitor.Data.Entities;
using SystemActivityMonitor.Data.Patterns.Visitor;

namespace SystemActivityMonitor.UI
{
    public partial class MainWindow : Window
    {
        private SystemController _controller = new SystemController();
        private CommandInvoker _invoker = new CommandInvoker();

        public MainWindow(string username, string role)
        {
            InitializeComponent();
            txtWelcome.Text = $"Вітаємо, {username}!";
            txtRole.Text = $"Рівень доступу: {role}";
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            IMonitorFactory selectedFactory;

            if (cmbFactoryMode.SelectedIndex == 0)
                selectedFactory = new StandardFactory();
            else
                selectedFactory = new CriticalFactory();

            ICommand generateCmd = new GenerateDataCommand(_controller, selectedFactory);

            _invoker.SetCommand(generateCmd);
            _invoker.Run();

            MessageBox.Show($"Дані згенеровано! Режим: {((ComboBoxItem)cmbFactoryMode.SelectedItem).Content}");
            BtnLoadIterator_Click(null, null);
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ICommand clearCmd = new ClearDataCommand(_controller);
            _invoker.SetCommand(clearCmd);
            _invoker.Run();

            MessageBox.Show("Базу очищено (через Command)!");
            lstLogs.Items.Clear();
        }

        private void BtnLoadIterator_Click(object sender, RoutedEventArgs e)
        {
            lstLogs.Items.Clear();
            LogCollection collection = new LogCollection();

            using (var db = new MonitorDbContext())
            {
                var logsFromDb = db.ResourceLogs.OrderByDescending(l => l.CreatedAt).ToList();
                foreach (var log in logsFromDb)
                {
                    collection.Add(log);
                }
            }

            IIterator iterator = collection.CreateIterator();
            iterator.First();
            while (!iterator.IsDone())
            {
                var item = iterator.CurrentItem();
                if (item != null)
                {
                    string displayText = $"[{item.CreatedAt.ToLongTimeString()}] CPU: {item.CpuLoad}% | RAM: {item.RamUsage} MB";
                    lstLogs.Items.Add(displayText);
                }
                iterator.Next();
            }
        }

        private void BtnBridgeReport_Click(object sender, RoutedEventArgs e)
        {
            List<ResourceLog> logs;
            using (var db = new MonitorDbContext())
            {
                logs = db.ResourceLogs.OrderByDescending(l => l.CreatedAt).Take(10).ToList();
            }

            var elements = new List<IMetricElement>();
            foreach (var log in logs)
            {
                elements.Add(new CpuMetric(log.CpuLoad, log.CreatedAt.ToShortTimeString(), log.ActiveWindow, log.IsSystemIdle));
                elements.Add(new RamMetric(log.RamUsage));
            }

            var analyzer = new AnalysisVisitor();
            foreach (var el in elements) el.Accept(analyzer);

            string analyticsResult = analyzer.GetStats();

            IReportRenderer renderer = cmbReportFormat.SelectedIndex == 0
                ? new PlainTextRenderer()
                : new HtmlRenderer();

            ReportAbstraction report = new DailyReport(renderer);
            string finalOutput = report.Generate(logs, analyticsResult);

            MessageBox.Show(finalOutput, "Повний інтегрований звіт");
        }

        private void BtnVisitorXml_Click(object sender, RoutedEventArgs e)
        {
            var structure = PrepareElements();
            var visitor = new XmlExportVisitor();

            foreach (var element in structure)
            {
                element.Accept(visitor);
            }

            MessageBox.Show(visitor.GetXml(), "Результат Visitor (XML)");
        }

        private void BtnVisitorStats_Click(object sender, RoutedEventArgs e)
        {
            var structure = PrepareElements();
            var visitor = new AnalysisVisitor();

            foreach (var element in structure)
            {
                element.Accept(visitor);
            }

            MessageBox.Show(visitor.GetStats(), "Результат Visitor (Stats)");
        }

        private List<IMetricElement> PrepareElements()
        {
            var elements = new List<IMetricElement>();

            using (var db = new MonitorDbContext())
            {
                var logs = db.ResourceLogs.OrderByDescending(l => l.CreatedAt).Take(10).ToList();

                foreach (var log in logs)
                {
                    elements.Add(new CpuMetric(log.CpuLoad, log.CreatedAt.ToShortTimeString(), log.ActiveWindow, log.IsSystemIdle));
                    elements.Add(new RamMetric(log.RamUsage));
                }
            }
            return elements;
        }
    }
}