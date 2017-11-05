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
    /// Логика взаимодействия для Authors.xaml
    /// </summary>
    public partial class Authors : Window
    {
        public enum Impact { New, Edit };
        Impact impact;
        Book result;

        public Authors(Impact impact)
        {
            InitializeComponent();
            this.impact = impact;
            AuthorImage.Source = Author.defaultImage;
        }

        ValidationResult ValidateForm()
        {
            if (NameBox.Text.Length < 3)
            {
                return new ValidationResult(false, "Name length is too small");
            }
            if (NameBox.Text.Split(' ').Any(name => char.IsLower(name[0])))
            {
                return new ValidationResult(false, "Every name should start with a capital letter");
            }
            if (BirthdayDate.SelectedDate > DateTime.Now)
            {
                return new ValidationResult(false, "Only past date");
            }

            return new ValidationResult(true, null);
        }
    }
}
