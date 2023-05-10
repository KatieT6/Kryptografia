using _3DES;
using Microsoft.Win32;
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
using System;
using System.IO;



namespace Window
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {

        private string key;

        private string decrypted;
        private string encrypted;
        private byte[]? encryptedBuffer;
        private byte[]? decryptedBuffer;
        private bool onReadFile = false;

        public MainWindow()
        {
            InitializeComponent();
            key = new string("0E329232EA6D0D73");
            GeneratedKey_TextArea.Text = key;
        }

        

        


        private void GenerateKey_Click(object sender, RoutedEventArgs e)
        {
            string s = GeneratedKey_TextArea.Text;
            if (s.Length == 16 && System.Text.RegularExpressions.Regex.IsMatch(s, "^[0-9A-Fa-f]+$"))
            {
                key = s;
                MessageBox.Show("The key is OK!", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
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
            //decrypted = Encoding.UTF8.GetString(decryptedBuffer);
            DesAlgorithm des = new DesAlgorithm(decryptedBuffer, key, false);
            encryptedBuffer = des.Encrypt();
            onReadFile = true;
            Encrypted_TextArea.Text = Convert.ToHexString(encryptedBuffer);

        }

        private void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            //encrypted = Encoding.UTF8.GetString(encryptedBuffer);
            DesAlgorithm des = new DesAlgorithm(encryptedBuffer, key, true);
            decryptedBuffer = des.Encrypt();
            onReadFile = true;
            Decrypted_TextArea.Text = Encoding.UTF8.GetString(decryptedBuffer);

        }


        private void Encrypted_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!onReadFile)
                encryptedBuffer = Encrypted_TextArea.Text.Length == 0 ? null : Encoding.UTF8.GetBytes(Encrypted_TextArea.Text);
            else
                onReadFile = false;
        }

        private void Decrypted_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!onReadFile)
                decryptedBuffer = Decrypted_TextArea.Text.Length == 0 ? null : Encoding.UTF8.GetBytes(Decrypted_TextArea.Text);
            else
                onReadFile = false;
        }

        private void GeneratedKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            //key = GeneratedKey_TextArea.Text;
            key = new(GeneratedKey_TextArea.Text);
        }


        private void OpenDecryptedFile_Click(object sender, RoutedEventArgs e)
        {
            // specifies metadata for explorer window
            OpenFileDialog openFileDialog = new()
            {
                Filter = "All files (*.*)|*.*",
                Title = "Read File"
            };

            // open explorer window for user
            if (openFileDialog.ShowDialog() == true)
            {
                // trying to read data from file
                try
                {
                    onReadFile = true;
                    decryptedBuffer = File.ReadAllBytes(openFileDialog.FileName);
                    Decrypted_TextArea.Text = Encoding.UTF8.GetString(decryptedBuffer);
                }
                catch (Exception)
                {
                    MessageBox.Show("ehhhh!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void OpenEncryptedFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "DES Cypher files files (*.*)|*.*",
                Title = "Read DES Cypher"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    onReadFile = true;
                    encryptedBuffer = File.ReadAllBytes(openFileDialog.FileName);
                    Encrypted_TextArea.Text = Encoding.UTF8.GetString(encryptedBuffer);
                }
                catch (Exception)
                {
                    MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void SaveDecryptedFile_Click(object sender, RoutedEventArgs e)
        {
            
            if (decryptedBuffer == null)
            {
                MessageBox.Show("TextBox is empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new()
            {
                Filter = "All files (*.*)|*.*|Text files (*.txt)|*.txt|PDF documents (*.pdf)|*.pdf",
                Title = "Save File As"
            };

            // showing user explorer window
            if (saveFileDialog.ShowDialog() == true)
            {
                // trying to save text to file
                try
                {
                    // saving text in specified file
                    File.WriteAllBytes(saveFileDialog.FileName, decryptedBuffer);
                }
                catch (Exception)
                {
                    MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Error!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SaveEncryptedFile_Click(object sender, RoutedEventArgs e)
        {
            if (encryptedBuffer == null)
            {
                MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // specifies metadata for explorer window
            SaveFileDialog saveFileDialog = new()
            {
                Filter = "DES Cypher files All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Title = "Save DES Cypher As"
            };

            // showing user explorer window
            if (saveFileDialog.ShowDialog() == true)
            {
                // trying to save cypher to file
                try
                {
                    // saving cypher in specified file
                    File.WriteAllBytes(saveFileDialog.FileName, encryptedBuffer);
                }
                catch (Exception)
                {
                    MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
           

        }
    }
}