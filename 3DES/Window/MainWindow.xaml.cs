using _3DES;
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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Window
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GeneratedKey_TextArea.Text = key;
        }

        private string key = "0E329232EA6D0D73";

        private string decrypted;
        private string encrypted;


        private void GenerateKey_Click(object sender, RoutedEventArgs e)
        {
            string s = GeneratedKey_TextArea.Text;
            if (s.Length == 16 && System.Text.RegularExpressions.Regex.IsMatch(s, "^[0-9A-Fa-f]+$"))
            {
                key = s;
            }
            else if (s.Length != 16)
            {
                MessageBox.Show("The key length must be 16!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("The key must be in hexadecimal format!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            decrypted = Decrypted_TextArea.Text;
            DesAlgorithm des = new DesAlgorithm(decrypted, key, false);
            encrypted = des.Encrypt();
            Encrypted_TextArea.Text = encrypted;
        }

        private void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            encrypted = Encrypted_TextArea.Text;
            DesAlgorithm des = new DesAlgorithm(encrypted, key, true);
            decrypted = des.Encrypt();
            Decrypted_TextArea.Text = decrypted;

        }


        private void Encrypted_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Decrypted_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void GeneratedKey_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}