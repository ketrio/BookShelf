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
        public Book impact { get; }
        
        private Books()
        {
            InitializeComponent();
            AuthorField.ItemsSource = (Application.Current as App).LibraryData.authors;
            PublisherField.ItemsSource = (Application.Current as App).LibraryData.publishers;
        }

        public Books(out Book book) : this()
        {
            book = new Book();
            impact = book;
            DataContext = impact;
        }

        public Books(Book book) : this()
        {
            impact = book;
            DataContext = impact;
        }

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

            return new ValidationResult(true, "");
        }

        private void TextBlock_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text) && (e.Text.Length < 5);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ValidationResult validationResult = ValidateForm();

            if (!validationResult.IsValid)
            {
                MessageBox.Show(validationResult.ErrorContent as string);
                return;
            }

            foreach (var textBox in root.Children.OfType<TextBox>())
            {
                BindingExpression be = textBox.GetBindingExpression(TextBox.TextProperty);
                be?.UpdateSource();
            }
            DateField.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource();
            AuthorField.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
            PublisherField.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();

            Close();
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
