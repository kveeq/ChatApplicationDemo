using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ChatApplication.View
{
    /// <summary>
    /// Логика взаимодействия для AutorizationWindow.xaml
    /// </summary>
    public partial class AutorizationWindow : Window
    {
        public AutorizationWindow()
        {
            InitializeComponent();
            FillFromConfigFile();
            //Process.Start("notepad.exe", App.configPath);
        }

        private void RememberCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (RememberCheckBox.IsChecked == false)
            {
                UsernameTxt.Clear();
                PassTxt.Clear();
                WriteToconfigFile("");
            }
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (UsernameTxt.Text == "" || PassTxt.Password == "")
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            App.user = App.db.Employee.Where(x => x.Username == UsernameTxt.Text && x.Password == PassTxt.Password).FirstOrDefault();
            if (App.user == null)
            {
                MessageBox.Show("Неправильный логин или пароль!");
                return;
            }

            if ((bool)RememberCheckBox.IsChecked)
            {
                WriteToconfigFile($"Login: {UsernameTxt.Text}");
                WriteToconfigFile($"Password: {PassTxt.Password}");
            }

            new ChatRoomWindow().Show();
            this.Close();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            var mes = MessageBox.Show("Приложение будет закрыто?", "Закрытие приложения!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mes == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        private void FillFromConfigFile()
        {
            try
            {
                using (StreamReader sr = new StreamReader(App.configPath))
                {
                    var str = sr.ReadLine();
                    if (String.IsNullOrEmpty(str))
                        return;

                    var str2 = sr.ReadLine();
                    if (String.IsNullOrEmpty(str2))
                        return;

                    UsernameTxt.Text = str.Contains("Login") ? str.Replace("Login: ", "") : "";
                    PassTxt.Password = str2.Contains("Password") ? str2.Replace("Password: ", "") : "";
                    sr.Close();
                }

                RememberCheckBox.IsChecked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void WriteToconfigFile(string str)
        {
            using (StreamWriter sw = new StreamWriter(App.configPath))
            {
                sw.WriteLine(str);
            }
        }

    }
}
