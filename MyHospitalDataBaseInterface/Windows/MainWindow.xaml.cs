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
using System.Data.Entity.Core.Objects;
using MyHospitalDataBaseInterface.Windows;
using WPFCustomMessageBox;
using System.Data.SqlClient;
using System.Security;
using MyHospitalDataBaseInterface.Data;
using System.IO;
using Microsoft.Win32;
using MyHospitalDataBaseInterface.Models;

namespace MyHospitalDataBaseInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Doctor Doctor { get; }

        public MainWindow(Doctor doctor)
        {
            Doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));

            InitializeComponent();
            DataContext = Doctor;
        }

        private void OnAvatarBorderClick(object sender, RoutedEventArgs e)
        {
            SelectAvatarFromFile();
        }

        private void UpdateAvatarFromImageBytes(byte[] imgBytes)
        {
            BitmapImage img = new BitmapImage();
            using (MemoryStream memStream = new MemoryStream(imgBytes))
            {
                memStream.Position = 0;
                img.BeginInit();
                img.StreamSource = memStream;
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.EndInit();
            }
            var imgBrush = new ImageBrush(img);
            imgBrush.Stretch = Stretch.UniformToFill;
            AvatarBorder.Background = imgBrush;
        }

        private void SelectAvatarFromFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Виберіть малюнок";
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            openFileDialog.Multiselect = false;
            openFileDialog.ShowReadOnly = true;
            if (openFileDialog.ShowDialog() == true) 
            {
                try
                {
                    var imageBytes = File.ReadAllBytes(openFileDialog.FileName);
                    UpdateAvatarFromImageBytes(imageBytes);

                    using (var db = new DbModel())
                    {
                        var doctor = db.Doctors.First(d => d.Id == Doctor.Id);
                        doctor.ImageBytes = imageBytes;
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CreatePatientButton_Click(object sender, RoutedEventArgs e)
        {
            var patientWindow = new PatientWindow(Doctor);
            patientWindow.ShowDialog();
        }

        private void EditPatientButton_Click(object sender, RoutedEventArgs e)
        {
            if (PatientsList.SelectedItem == null)
            {
                MessageBox.Show("Будь-ласка виберіть пацієнта", "Редагувати пацієнта", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var patient = PatientsList.SelectedItem as Patient;
            var patientWindow = new PatientWindow(Doctor,patient);
            patientWindow.ShowDialog();
        }

        private void DeletePatientButton_Click(object sender, RoutedEventArgs e)
        {
            if (PatientsList.SelectedItem == null)
            {
                MessageBox.Show("Будь-ласка виберіть пацієнта","Видалити пацієнта",MessageBoxButton.OK,MessageBoxImage.Information);
                return;
            }

            MessageBoxResult result = CustomMessageBox.ShowYesNo(
              "Ви впевнені що хочете видалити пацієнта?",
              "Видалити пацієнта",
              "Прийняти",
              "Відхилити",
            MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var db = new DbModel())
                    {
                        var patient = PatientsList.SelectedItem as Patient;
                        var dbPatient = db.Patients.FirstOrDefault(p => p.Id == patient.Id);
                        if(dbPatient != null)
                        {
                            var dbVisits = dbPatient.Visits.ToArray();
                            foreach (var dbVisit in dbVisits)
                            {
                                db.Visits.Remove(dbVisit);
                            }
                            dbPatient.Visits.Clear();

                            db.Patients.Remove(dbPatient);
                            db.SaveChanges();
                            Doctor.Patients.Remove(patient);
                            Doctor.UpdateFilter();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void PatientInfoButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Patient patient = button.DataContext as Patient;
    
            var patientWindow = new PatientWindow(Doctor, patient);
            patientWindow.ShowDialog();
        }

        private void PatientFilterChanged(object sender, TextChangedEventArgs args)
        {
            Doctor.Filter = (sender as TextBox).Text;
        }

        private void MainWindowExit_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
