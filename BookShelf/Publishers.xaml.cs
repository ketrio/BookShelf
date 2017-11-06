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
        public enum ImpactType { Save, Edit }

        Publisher impact { get; }
        ImpactType impactType;

        private Publishers(Publisher publisher, ImpactType impactType)
        {
            InitializeComponent();

            impact = publisher;
            this.impactType = impactType;

            DataContext = impact;
        }

        public Publishers() : this(new Publisher(), ImpactType.Save) { }
        public Publishers(Publisher publisher) : this(publisher, ImpactType.Edit) { }

        ValidationResult ValidateForm()
        {
            if (NameField.Text == "" || CityField.Text == "")
            {
                return new ValidationResult(false, "Empty fields are not allowed");
            }
            if (NameField.Text.Split(' ').Any(name => char.IsLower(name[0])))
            {
                return new ValidationResult(false, "Name should start with a capital letter");
            }
            if (CityField.Text.Split(' ').Any(cityName => char.IsLower(cityName[0]))) {
                return new ValidationResult(false, "City name should start with a capital letter");
            }
            return new ValidationResult(true, null);
        }

        void Save()
        {
            NameField.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            CityField.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            if (impactType == ImpactType.Save) (Application.Current as App).LibraryData.publishers.Add(impact);
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
    }
}
