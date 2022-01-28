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
using System.Text.RegularExpressions;
using System.Net.Http;
using Newtonsoft.Json;

namespace CurrencyConverter_Static
{
    public partial class MainWindow : Window
    {
        Root val = new Root();
        public class Root
        {
            public Rate rates { get; set; }
        }

        public class Rate
        {
            public double BGN { get; set; }
            public double EUR { get; set; }
            public double USD { get; set; }
            public double AUD { get; set; }
            public double GBP { get; set; }
            public double JPY { get; set; }
            public double MXN { get; set; }

            public double KRW { get; set; }
        }
        public MainWindow()
        {
            InitializeComponent();
            ClearControls();
            GetValue();
        }
        private async void GetValue()
        {
            val = await GetData<Root>("https://openexchangerates.org/api/latest.json?app_id=32da1ea2c7c54613b9dd34559555d079");
            BindCurrency();
        }
        public static async Task<Root> GetData<T>(string url)
        {
            var myRoot = new Root();
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(1); //the timespan to await before the request times out
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK) //check API status responce code
                    {
                        var ResponseString = await response.Content.ReadAsStringAsync(); //serialize HTTP content to string

                        var ResponseObject = JsonConvert.DeserializeObject<Root>(ResponseString);

                        return ResponseObject; //return API responce

                    }
                    return myRoot;
                }
            }
            catch
            {
                return myRoot;
            }
        }
        private void BindCurrency()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Text");
            dt.Columns.Add("Rate");

            //Add rows in the Datatable with text and value
            dt.Rows.Add("--SELECT--", 0);
            dt.Rows.Add("BGN", val.rates.BGN);
            dt.Rows.Add("EUR", val.rates.EUR);
            dt.Rows.Add("USD", val.rates.USD);
            dt.Rows.Add("AUD", val.rates.AUD);
            dt.Rows.Add("GBP", val.rates.GBP);
            dt.Rows.Add("JPY", val.rates.JPY);
            dt.Rows.Add("MXN", val.rates.MXN);
            dt.Rows.Add("KRW", val.rates.KRW);


            cmbFromCurrency.ItemsSource = dt.DefaultView;
            cmbFromCurrency.DisplayMemberPath = "Text";
            cmbFromCurrency.SelectedValuePath = "Rate";
            cmbFromCurrency.SelectedIndex = 0;

            cmbToCurrency.ItemsSource = dt.DefaultView;
            cmbToCurrency.DisplayMemberPath = "Text";
            cmbToCurrency.SelectedValuePath = "Rate";
            cmbToCurrency.SelectedIndex = 0;
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
            else if (cmbFromCurrency.SelectedIndex == 0 || cmbFromCurrency.SelectedValue == null)
            {
                //display message if combobox is empty/not selected
                MessageBox.Show("Please select currency From", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbFromCurrency.Focus();
                return;
            }
            //check if combobox ToCurrency is empty/not selected
            else if (cmbToCurrency.SelectedIndex == 0 || cmbToCurrency.SelectedValue == null)
            {
                //display message if combobox is empty/not selected
                MessageBox.Show("Please select currency To", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbToCurrency.Focus();
                return;
            }
            //check if From and To currency are the same
            if (cmbFromCurrency.Text == cmbToCurrency.Text)
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
                ConvertedValue = (double.Parse(cmbToCurrency.SelectedValue.ToString()) *
                    double.Parse(txtCurrency.Text)) /
                    double.Parse(cmbFromCurrency.SelectedValue.ToString());
                //setting label "Currency" to show the new currency type and value
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
        }

        private void ClearControls()
        {
            lblCurrency.Content = string.Empty;
            txtCurrency.Text = string.Empty;
            if (cmbFromCurrency.Items.Count > 0)
            {
                cmbFromCurrency.SelectedIndex = 0;
            }
            if (cmbToCurrency.Items.Count > 0)
            {
                cmbToCurrency.SelectedIndex = 0;
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
