using BookShelf.PluginSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace StatisticsPlugin
{
    [PluginInfo("Statistics", "A plugin that allows you to track time usage of your application.", "Vlad Gutkovsky", "0.1")]
    class StatisticsPlugin : BookPlugin
    {
        DispatcherTimer timer;

        public StatisticsPlugin(Application app) : base(app)
        {

        }

        public override void Impact()
        {
            Window window = new Window();
            window.Height = 200;
            window.Width = 300;

            var stackPanel = new StackPanel();
            var upperLabel = new Label();
            var label = new Label();

            upperLabel.FontSize = label.FontSize = 24;
            upperLabel.Content = "Application up time is:";
            
            label.Content = (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"hh\:mm\:ss");

            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1),
                IsEnabled = true
            };
            timer.Tick += delegate (object sender, EventArgs e)
            {
                label.Content = (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"hh\:mm\:ss");
            };
            timer.Start();

            stackPanel.Children.Add(upperLabel);
            stackPanel.Children.Add(label);
            window.Content = stackPanel;

            window.Show();
        }

        public override void Dispose()
        {
            timer.Stop();
        }
    }
}
