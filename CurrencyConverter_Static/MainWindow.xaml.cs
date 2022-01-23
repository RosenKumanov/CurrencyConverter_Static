using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CurrencyConverter_Static
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BindCurrency();
            
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            //Double variable to store converted currency 
            double ConvertedValue;
            //Check if Currency box is null or empty 
            if (txtCurrency.Text == null || txtCurrency.Text.Trim() == "")
            {
                //displaying message if Currency is empty/null
                MessageBox.Show("Please enter value", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                //After clicking OK button, focus becomes the currency textbox
                txtCurrency.Focus();
                return;
            }
            //Check if combobox FromCurrency is empty/not selected
            else if(cmbFromCurrency.SelectedIndex == 0 || cmbFromCurrency.SelectedValue == null)
            {
                //display message if combobox is empty/not selected
                MessageBox.Show("Please enter Currency From", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbFromCurrency.Focus();
                return;
            }
            //check if combobox ToCurrency is empty/not selected
            else if (cmbToCurrency.SelectedIndex == 0 || cmbToCurrency.SelectedValue == null)
            {
                //display message if combobox is empty/not selected
                MessageBox.Show("Please enter Currency To", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbToCurrency.Focus();
                return;
            }
            //check if From and To currency are the same
            if(cmbFromCurrency.Text == cmbToCurrency.Text)
            {
                //set ConvertedValue amount to the input value
                //parsing to Double from string
                ConvertedValue = double.Parse(txtCurrency.Text);

                //setting label "Currency" to show the new currency type and value
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
            else
            {
                //calculate converted value by multiplying "FromCurrency" value with the Input value, then gets divided by the "ToCurrency" Value
                ConvertedValue = (double.Parse(cmbFromCurrency.SelectedValue.ToString()) * 
                    double.Parse(txtCurrency.Text)) / 
                    double.Parse(cmbToCurrency.SelectedValue.ToString());
                //setting label "Currency" to show the new currency type and value
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
        }

        private void BindCurrency()
        {
            DataTable dtCurrency = new DataTable();
            dtCurrency.Columns.Add("Text");
            dtCurrency.Columns.Add("Value");

            //Add rows in the Datatable with text and value
            dtCurrency.Rows.Add("--SELECT--", 0);
            dtCurrency.Rows.Add("BGN", 1);
            dtCurrency.Rows.Add("EUR", 1.90);
            dtCurrency.Rows.Add("USD", 1.70);

            cmbFromCurrency.ItemsSource = dtCurrency.DefaultView;
            cmbFromCurrency.DisplayMemberPath = "Text";
            cmbFromCurrency.SelectedValuePath = "Value";
            cmbFromCurrency.SelectedIndex = 0;

            cmbToCurrency.ItemsSource = dtCurrency.DefaultView;
            cmbToCurrency.DisplayMemberPath = "Text";
            cmbToCurrency.SelectedValuePath = "Value";
            cmbToCurrency.SelectedIndex = 0; 
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            lblCurrency.Content = "";
            cmbFromCurrency.SelectedIndex = 0;
            cmbToCurrency.SelectedIndex = 0;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {

        }
    }
}
