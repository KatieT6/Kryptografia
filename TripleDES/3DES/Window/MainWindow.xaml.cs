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
using System.Security.Cryptography;
using _3DES.Logic;

namespace Window
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {

        private string key1;
        private string key2;
        private string key3;

        private byte[]? encryptedBuffer;
        private byte[]? decryptedBuffer;
        private bool onReadFile = false;

        public MainWindow()
        {
            InitializeComponent();
            key1 = new string("9F4882D692973450");
            GeneratedKey1_TextArea.Text = key1;
            key2 = new string("B32847E0D3A685F1");
            GeneratedKey2_TextArea.Text = key2;
            key3 = new string("83BF2648CFA0B735");
            GeneratedKey3_TextArea.Text = key3;
        }

        

        


        private void GenerateKeys_Click(object sender, RoutedEventArgs e)
        {
            KeyGenerator keyGenerator = new KeyGenerator();
            byte[] keyBuffer = new byte[16];
            keyBuffer = keyGenerator.Generate3DesKeys();
            key1 = Converter.ByteArrayToHex(keyBuffer);
            GeneratedKey1_TextArea.Text = key1;
            keyBuffer = keyGenerator.Generate3DesKeys();
            key2 = Converter.ByteArrayToHex(keyBuffer);
            GeneratedKey2_TextArea.Text = key2;
            keyBuffer = keyGenerator.Generate3DesKeys();
            key3 = Converter.ByteArrayToHex(keyBuffer);
            GeneratedKey3_TextArea.Text = key3;
        }

        private void CheckKeys_Click(object sender, RoutedEventArgs e)
        {
            string s1 = GeneratedKey1_TextArea.Text;
            string s2 = GeneratedKey2_TextArea.Text;
            string s3 = GeneratedKey3_TextArea.Text;
            if (s1.Length == 16 && System.Text.RegularExpressions.Regex.IsMatch(s1, "^[0-9A-Fa-f]+$") &&
                s2.Length == 16 && System.Text.RegularExpressions.Regex.IsMatch(s2, "^[0-9A-Fa-f]+$") &&
                s3.Length == 16 && System.Text.RegularExpressions.Regex.IsMatch(s3, "^[0-9A-Fa-f]+$"))
            {
                key1 = s1;
                key2 = s2;
                key3 = s3;
                MessageBox.Show("Keys are OK!", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (s1.Length != 16 || s2.Length != 16 || s3.Length != 16)
            {
                MessageBox.Show("The every key length must be 16!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("The every key must be in hexadecimal format!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void GeneratedKey1_TextChanged(object sender, TextChangedEventArgs e)
        {
            key1 = GeneratedKey1_TextArea.Text.Length == 0 ? null : GeneratedKey1_TextArea.Text;
        }

        private void GeneratedKey2_TextChanged(object sender, TextChangedEventArgs e)
        {
            key2 = GeneratedKey2_TextArea.Text.Length == 0 ? null : GeneratedKey2_TextArea.Text;

        }

        private void GeneratedKey3_TextChanged(object sender, TextChangedEventArgs e)
        {
            key3 = GeneratedKey3_TextArea.Text.Length == 0 ? null : GeneratedKey3_TextArea.Text;

        }

        private void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            DesTripleAlgorithm tripleDES = new DesTripleAlgorithm(key1, key2, key3, decryptedBuffer);
            encryptedBuffer = tripleDES.encrypt();
            onReadFile = true;
            Encrypted_TextArea.Text = Convert.ToHexString(encryptedBuffer);

        }

        private void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            DesTripleAlgorithm tripleDES = new DesTripleAlgorithm(key1, key2, key3, encryptedBuffer);
            decryptedBuffer = tripleDES.decrypt();
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