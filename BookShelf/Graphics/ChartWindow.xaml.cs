using Models;
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
        private List<int> chartData;

        public ChartWindow(List<Book> list)
        {
            InitializeComponent();
            chartData = Enumerable.Range(1, 12).ToList<int>()
                        .Select((e, i) => list.Where(book => book.PublishDate.Month == i + 1).Count())
                        .ToList<int>();
            Visualize();
        }

        private void Visualize()
        {
            Point prevPoint = new Point();
            for (int i = 0; i < 12; i++)
            {
                var e = chartData[i];
                
                var ell = new Ellipse();
                ell.Height = 10;
                ell.Width = 10;
                ell.Fill = new SolidColorBrush(Colors.Red);
                ell.Margin = new Thickness(40 + i * 58.5, 295 - e * 30, 0, 0);

                if (prevPoint != new Point())
                {
                    var line = new Line();
                    line.X1 = prevPoint.X;
                    line.Y1 = prevPoint.Y;
                    line.X2 = 40 + i * 58.5 + 5;
                    line.Y2 = 295 - e * 30 + 5;
                    line.Stroke = new SolidColorBrush(Colors.Red);
                    line.StrokeThickness = 2;
                    ChartCanvas.Children.Add(line);
                }
                prevPoint = new Point(40 + i * 58.5 + 5, 295 - e * 30 + 5);

                ChartCanvas.Children.Add(ell);
            }
        }
    }
}
