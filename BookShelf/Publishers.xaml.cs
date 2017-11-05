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

using Models;

namespace BookShelf
{
    /// <summary>
    /// Логика взаимодействия для Publishers.xaml
    /// </summary>
    public partial class Publishers : Window
    {
        public Publisher impact { get; }

        private Publishers()
        {
            InitializeComponent();
        }

        public Publishers(out Publisher publisher) : this()
        {
            publisher = new Publisher();
            impact = publisher;
            DataContext = impact;
        }

        public Publishers(Publisher publisher) : this()
        {
            impact = publisher;
            DataContext = impact;
        }
    }
}
