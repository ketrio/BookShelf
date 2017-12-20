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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookShelf.Graphics
{
    /// <summary>
    /// Interaction logic for ChartWindow.xaml
    /// </summary>
    public partial class ChartWindow : Window
    {
        private static Duration animationDuration = new Duration(new TimeSpan(0, 0, 0, 0, 450));

        private List<int> chartData;
        private List<Tuple<Shape, Storyboard>> storyElements = 
            new List<Tuple<Shape, Storyboard>>();

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

                initDot(i, e);

                if (prevPoint != new Point())
                {
                    initLine(i, e, prevPoint);
                }
                prevPoint = new Point(40 + i * 58.5 + 5, 295 - e * 30 + 5);

            }
        }

        private void initDot(int index, int element)
        {
            var ell = new Ellipse();
            ell.Height = 10;
            ell.Width = 10;
            ell.Fill = new SolidColorBrush(Colors.Red);
            ell.Margin = new Thickness(40 + index * 58.5, 295, 0, 0);
            Panel.SetZIndex(ell, 100);
            ChartCanvas.Children.Add(ell);

            var targetMargin = new Thickness(40 + index * 58.5, 295 - element * 30, 0, 0);

            Storyboard sb = new Storyboard();
            ThicknessAnimation da = new ThicknessAnimation(ell.Margin, targetMargin, animationDuration);
            Storyboard.SetTargetProperty(da, new PropertyPath("(Ellipse.Margin)"));
            sb.Children.Add(da);

            storyElements.Add(new Tuple<Shape, Storyboard>(ell, sb));
        }

        private void initLine(int index, int element, Point point)
        {
            var line = new Line();
            line.X1 = point.X;
            line.Y1 = 300;
            line.X2 = 40 + index * 58.5 + 5;
            line.Y2 = 300;
            line.Stroke = new SolidColorBrush(Colors.Red);
            line.StrokeThickness = 2;
            ChartCanvas.Children.Add(line);

            Storyboard sb = new Storyboard();
            DoubleAnimation da = new DoubleAnimation(line.Y1, point.Y, animationDuration);
            DoubleAnimation da1 = new DoubleAnimation(line.Y2, 295 - element * 30 + 5, animationDuration);
            Storyboard.SetTargetProperty(da, new PropertyPath("(Line.Y1)"));
            Storyboard.SetTargetProperty(da1, new PropertyPath("(Line.Y2)"));
            sb.Children.Add(da);
            sb.Children.Add(da1);

            storyElements.Add(new Tuple<Shape, Storyboard>(line, sb));
        }

        //private void initPoly(int index, int element, Point point)
        //{
        //    var poly = new Polygon();
        //    var color = Colors.Red;
        //    color.A = 150;
        //    poly.Fill = new SolidColorBrush(color);
        //    Panel.SetZIndex(poly, 0);

        //    var firstUpper = new Point(point.X, 300);
        //    var secondUpper = new Point(40 + index * 58.5 + 5, 300);

        //    poly.Points = new PointCollection(new List<Point>
        //                {
        //                    //new Point(point.X, point.Y),
        //                    //new Point(40 + index * 58.5 + 5, 295 - element * 30 + 5),
        //                    firstUpper,
        //                    secondUpper,
        //                    new Point(40 + index * 58.5 + 5, 300),
        //                    new Point(point.X, 300),
        //                });
        //    ChartCanvas.Children.Add(poly);

        //    poly.


        //    //Storyboard sb = new Storyboard();
        //    //DoubleAnimation da = new DoubleAnimation(firstUpper.Y, point.Y, animationDuration);
        //    //Storyboard.SetTargetProperty(da, new PropertyPath("(Point.Y)"));
        //    //sb.Children.Add(da);
        //    //storyElements.Add(new Tuple<Shape, Storyboard>(firstUpper, sb));

        //    //Storyboard sb1 = new Storyboard();
        //    //DoubleAnimation da1 = new DoubleAnimation(line.Y1, point.Y, animationDuration);
        //    //Storyboard.SetTargetProperty(da1, new PropertyPath("(Line.Y1)"));
        //    //sb.Children.Add(da1);
        //    //storyElements.Add(new Tuple<Shape, Storyboard>(line, sb1));
        //}

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            storyElements.ForEach(tuple => tuple.Item1.BeginStoryboard(tuple.Item2));
        }
    }
}
