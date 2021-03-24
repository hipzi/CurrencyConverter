using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BindCurrency();
        }

        private double getRate(string fromCurrency, string toCurrency)
        {
            var json = "";
            double rate;
            try
            {
                string url = string.Format("https://free.currconv.com/api/v7/convert?q={0}_{1}&compact=ultra&apiKey=cd0304f3d4c6b7e923d3", fromCurrency.ToUpper(), toCurrency.ToUpper());
                string key = string.Format("{0}_{1}", fromCurrency.ToUpper(), toCurrency.ToUpper());

                json = new WebClient().DownloadString(url);
                dynamic stuff = JsonConvert.DeserializeObject(json);
                rate = stuff[key];
            }
            catch
            {
                rate = 0;
            }
            return rate;
        }

        private void BindCurrency()
        {
            DataTable dtCurrency = new DataTable();
            dtCurrency.Columns.Add("Text");

            dtCurrency.Rows.Add("--SELECT--");
            dtCurrency.Rows.Add("IDR");
            dtCurrency.Rows.Add("INR");
            dtCurrency.Rows.Add("USD");
            dtCurrency.Rows.Add("EUR");
            dtCurrency.Rows.Add("SAR");
            dtCurrency.Rows.Add("BAM");
            dtCurrency.Rows.Add("PYG");
            dtCurrency.Rows.Add("TRY");
            dtCurrency.Rows.Add("JPY");

            cmbFromCurrency.ItemsSource = dtCurrency.DefaultView;
            cmbFromCurrency.DisplayMemberPath = "Text";
            cmbFromCurrency.SelectedIndex = 0;

            cmbToCurrency.ItemsSource = dtCurrency.DefaultView;
            cmbToCurrency.DisplayMemberPath = "Text";
            cmbToCurrency.SelectedIndex = 0;
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            double ConvertedValue;

            if (txtCurrency.Text == null || txtCurrency.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Currency", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                txtCurrency.Focus();
                return;
            }

            if (cmbFromCurrency.Text == cmbToCurrency.Text)
            {
                ConvertedValue = double.Parse(txtCurrency.Text);
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
            else
            {
                double rate = getRate(cmbFromCurrency.Text, cmbToCurrency.Text);
                ConvertedValue = (rate * double.Parse(txtCurrency.Text));
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ClearControls()
        {
            txtCurrency.Text = string.Empty;
            if (cmbFromCurrency.Items.Count > 0)
                cmbFromCurrency.SelectedIndex = 0;
            if (cmbToCurrency.Items.Count > 0)
                cmbToCurrency.SelectedIndex = 0;
            lblCurrency.Content = "";
            txtCurrency.Focus();
        }

    }
}
