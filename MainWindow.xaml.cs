using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Threading;

namespace FileEncryptionWPF_dz
{
    public partial class MainWindow : Window
    {
        private List<string> selectedFiles = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        // выбор файлов
        private void btnSelectFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                selectedFiles = openFileDialog.FileNames.ToList();
                lstSelectedFiles.ItemsSource = selectedFiles;
            }
        }

        // шифр Цезаря
        private void btnEncryptCaesar_Click(object sender, RoutedEventArgs e)
        {
            int shift;
            if (int.TryParse(txtCaesarKey.Text, out shift))
            {
                Task.Run(() => EncryptFilesUsingCaesar(shift)); // запуск в отдельном потоке
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректный ключ для шифра Цезаря.");
            }
        }

        // шифрование с помощью Цезаря
        private void EncryptFilesUsingCaesar(int shift)
        {
            string encryptedFolder = "Encrypted";
            Directory.CreateDirectory(encryptedFolder);

            foreach (var file in selectedFiles)
            {
                string content = File.ReadAllText(file, Encoding.UTF8);
                string encryptedContent = EncryptCaesar(content, shift);
                string encryptedFile = Path.Combine(encryptedFolder, Path.GetFileName(file));

                File.WriteAllText(encryptedFile, encryptedContent);
            }

            MessageBox.Show("Файлы зашифрованы и сохранены в папке Encrypted.");
        }

        // алгоритм Цезаря
        private string EncryptCaesar(string input, int shift)
        {
            StringBuilder result = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsLetter(c))
                {
                    char offset = char.IsUpper(c) ? 'A' : 'a';
                    char encryptedChar = (char)((c + shift - offset) % 26 + offset);
                    result.Append(encryptedChar);
                }
                else
                {
                    result.Append(c); // не изменяем символы, которые не являются буквами
                }
            }
            return result.ToString();
        }

        // шифр Вижинера
        private void btnEncryptVigenere_Click(object sender, RoutedEventArgs e)
        {
            string key = txtVigenereKey.Text;
            if (!string.IsNullOrEmpty(key))
            {
                Task.Run(() => EncryptFilesUsingVigenere(key)); // запуск в отдельном потоке
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите ключ для шифра Вижинера.");
            }
        }

        // шифрование с помощью шифра Вижинера
        private void EncryptFilesUsingVigenere(string key)
        {
            string encryptedFolder = "Encrypted";
            Directory.CreateDirectory(encryptedFolder);

            foreach (var file in selectedFiles)
            {
                string content = File.ReadAllText(file, Encoding.UTF8);
                string encryptedContent = EncryptVigenere(content, key);
                string encryptedFile = Path.Combine(encryptedFolder, Path.GetFileName(file));

                File.WriteAllText(encryptedFile, encryptedContent);
            }

            MessageBox.Show("Файлы зашифрованы с использованием шифра Вижинера.");
        }

        // алгоритм Вижинера
        private string EncryptVigenere(string input, string key)
        {
            StringBuilder result = new StringBuilder();
            int keyIndex = 0;

            foreach (char c in input)
            {
                if (char.IsLetter(c))
                {
                    char offset = char.IsUpper(c) ? 'A' : 'a';
                    char k = char.IsUpper(c) ? char.ToUpper(key[keyIndex % key.Length]) : char.ToLower(key[keyIndex % key.Length]);

                    char encryptedChar = (char)((c + (k - offset)) % 26 + offset);
                    result.Append(encryptedChar);
                    keyIndex++;
                }
                else
                {
                    result.Append(c);
                }
            }
            return result.ToString();
        }

        // расшифровка файлов
        private void btnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            int shift;
            if (int.TryParse(txtCaesarKey.Text, out shift))
            {
                Task.Run(() => DecryptFilesUsingCaesar(shift));
            }
            else
            {
                MessageBox.Show("Введите корректный ключ для расшифровки.");
            }
        }

        // расшифровка с помощью Цезаря
        private void DecryptFilesUsingCaesar(int shift)
        {
            string decryptedFolder = "Decrypted";
            Directory.CreateDirectory(decryptedFolder);

            foreach (var file in selectedFiles)
            {
                string content = File.ReadAllText(file, Encoding.UTF8);
                string decryptedContent = EncryptCaesar(content, -shift); // Обратное шифрование
                string decryptedFile = Path.Combine(decryptedFolder, Path.GetFileName(file));

                File.WriteAllText(decryptedFile, decryptedContent);
            }

            MessageBox.Show("Файлы расшифрованы и сохранены в папке Decrypted.");
        }

        // отправка письма (просто заглушка для примера)
        private void btnSendEmail_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Отправка письма с зашифрованными файлами.");
        }
    }
}
