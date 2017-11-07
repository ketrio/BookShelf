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
using Microsoft.Win32;

namespace BookShelf
{
    /// <summary>
    /// Логика взаимодействия для Authors.xaml
    /// </summary>
    public partial class Authors : Window
    {
        public enum ImpactType { Save, Edit };

        Author impact { get; }
        ImpactType impactType;

        private Authors(Author author, ImpactType impactType)
        {
            InitializeComponent();

            impact = author;
            this.impactType = impactType;

            DataContext = impact;
        }

        public Authors() : this(new Author(), ImpactType.Save) { }
        public Authors(Author author) : this(author, ImpactType.Edit) { }

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
            if (BirthDate.SelectedDate > DateTime.Now)
            {
                return new ValidationResult(false, "Only past date");
            }

            return new ValidationResult(true, null);
        }

        void Save()
        {
            AuthorImage.GetBindingExpression(Image.SourceProperty).UpdateSource();
            NameBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            BirthDate.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource();

            var authorsCollection = (Application.Current as App).LibraryData.authors;
            if (impactType == ImpactType.Save)
            {
                if (!authorsCollection.Contains(impact))
                {
                    authorsCollection.Add(impact);
                }
                else
                {
                    MessageBox.Show("The collection already contains the author", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ValidationResult validationResult = ValidateForm();
            if (!validationResult.IsValid)
            {
                MessageBox.Show(validationResult.ErrorContent as String, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Save();
            Close();
        }

        private void AuthorImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    BitmapImage image = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute));
                    impact.Image = image;
                } 
                catch (NotSupportedException)
                {
                    MessageBox.Show("Provided file is not supported", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void root_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Button_Click(null, null);
        }
    }
}
