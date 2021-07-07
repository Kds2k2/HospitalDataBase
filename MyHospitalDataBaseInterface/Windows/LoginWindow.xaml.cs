using MyHospitalDataBaseInterface.Data;
using MyHospitalDataBaseInterface.Models;
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

namespace MyHospitalDataBaseInterface.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var pinCode = PinCodeBox.Password;
            if (string.IsNullOrEmpty(pinCode)) 
            {
                WarningLabel.Text = "PIN-code required!";    
                WarningLabel.Visibility = Visibility.Visible;
                return;
            }
            try
            {
                using (var db = new DbModel())
                {
                    var doctor = db.Doctors.FirstOrDefault(d => String.Compare(pinCode, d.PinCode) == 0);
                    if (doctor == null)
                    {
                        WarningLabel.Visibility = Visibility.Visible;
                        WarningLabel.Text = "Невірний PIN - код.\nБудь - ласка спробуйте ще раз.";
                        PinCodeBox.Password = string.Empty;
                        return;
                    }

                    var mainWindow = new MainWindow(new Doctor(doctor));
                    mainWindow.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
