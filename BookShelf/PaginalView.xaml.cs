using BookShelf.Pages;
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

namespace BookShelf
{
    /// <summary>
    /// Interaction logic for PaginalView.xaml
    /// </summary>
    public partial class PaginalView : Window
    {
        public PaginalView(PageLoader loader, Page current)
        {
            InitializeComponent();

            Loader = loader;
            View.Content = current;
            DataContext = loader;
        }

        public PageLoader Loader { get; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            View.Content = Loader.First;
            Loader.Position = 0;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            View.Content = Loader.Prev;
            Loader.Position--;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            View.Content = Loader.Next;
            Loader.Position++;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            View.Content = Loader.Last;
            Loader.Position = Loader.LastIndex;
        }
    }
}
