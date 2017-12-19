using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookShelf.Graphics
{
    /// <summary>
    /// Interaction logic for ChartWindow.xaml
    /// </summary>
    public partial class ChartWindow : Window
    {
        static readonly List<string> months = new List<string>
            { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public ChartWindow()
        {
            InitializeComponent();
            InitLayout();
        }

        private void InitLayout()
        {
            for(int i = 1; i < 11; i++)
            {
                var line = new Line();
                line.X1 = 0;
                line.X2 = 715;
                line.Y1 = line.Y2 = i * 30;
                line.Stroke = new SolidColorBrush(Colors.Gray);
                ChartCanvas.Children.Add(line);
            }
        }
    }
}
