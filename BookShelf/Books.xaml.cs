using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Text.RegularExpressions;

using Models;

namespace BookShelf
{
    /// <summary>
    /// Interaction logic for Books.xaml
    /// </summary>
    public partial class Books : Window
    {
        enum ImpactType { Save, Edit };

        Book impact { get; }
        ImpactType impactType;
        
        Books(Book book, ImpactType impactType)
        {
            InitializeComponent();
            AuthorField.ItemsSource = (Application.Current as App).LibraryData.authors;
            PublisherField.ItemsSource = (Application.Current as App).LibraryData.publishers;

            impact = book;
            this.impactType = impactType;

            DataContext = impact;
        }

        public Books(Book book) : this(book, ImpactType.Edit) { }
        public Books() : this(new Book(), ImpactType.Save) { } 

        ValidationResult ValidateForm()
        {
            // Validating textboxes
            foreach(var textBox in root.Children.OfType<TextBox>())
            {
                if (textBox.Text == "")
                {
                    return new ValidationResult(false, "Empty textboxes are not allowed");
                }
            }

            //Validating datePickers
            foreach (var datePicker in root.Children.OfType<DatePicker>())
            {
                if (datePicker.SelectedDate == null)
                {
                    return new ValidationResult(false, "Publication date should be provided");
                }

                if (datePicker.SelectedDate > DateTime.Now)
                {
                    return new ValidationResult(false, "Only past date is allowed");
                } 
            }

            // Validating comboBoxes
            foreach (var comboBox in root.Children.OfType<ComboBox>())
            {
                if (comboBox.SelectedIndex == -1)
                {
                    return new ValidationResult(false, "Select item in comboboxes");
                }
            }

            return new ValidationResult(true, "");
        }

        void Save()
        {
            foreach (var textBox in root.Children.OfType<TextBox>())
            {
                BindingExpression be = textBox.GetBindingExpression(TextBox.TextProperty);
                be?.UpdateSource();
            }
            DateField.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource();
            AuthorField.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
            PublisherField.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();

            var booksCollection = (Application.Current as App).LibraryData.books;
            if (impactType == ImpactType.Save)
            {
                if (!booksCollection.Contains<Book>(impact))
                    (Application.Current as App).LibraryData.books.Add(impact);
                else
                    MessageBox.Show("A book with the ISBN has already been added", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Close();
        }

        private void TextBlock_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            String text = (sender as TextBox).Text + e.Text;
            Regex regex = new Regex("^([1-9][0-9]*)$");
            e.Handled = !regex.IsMatch(text);
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

        private void root_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Button_Click(null, null);
        }
    }

    public class ArrayToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return String.Join(", ", value as String[]);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as String)?.Replace(", ", ",").Split(',');
        }
    }
}
