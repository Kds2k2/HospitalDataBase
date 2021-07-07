using Microsoft.Win32;
using MyHospitalDataBaseInterface.Data;
using MyHospitalDataBaseInterface.Models;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for PatientsWindow.xaml
    /// </summary>
    public partial class PatientWindow : Window
    {
        public Doctor Doctor { get; }

        public Patient Patient { get; }

        public Visit Visit { get; }

        public PatientWindow(Doctor doctor, Patient patient = null)
        {
            Doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));
            Patient = (patient ?? new Patient()).Clone();

            InitializeComponent();
            InfoButton_Click(InfoButton, null);
            DataContext = Patient;
            PatientVisits.VisitInfo.DataContext = Visit;
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            ContentTitleLabel.Text = (string)((Button)sender).Content + ":";
            PatientInfo.Visibility = Visibility.Visible;
            PatientVisits.Visibility = Visibility.Hidden;
        }

        private void VisitsButton_Click(object sender, RoutedEventArgs e)
        {
            ContentTitleLabel.Text = (string)((Button)sender).Content + ":";
            PatientInfo.Visibility = Visibility.Hidden;
            PatientVisits.Visibility = Visibility.Visible;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var patient = DataContext as Patient;
                using (DbModel db = new DbModel())
                {
                    var dbPatient = db.Patients.FirstOrDefault(p => p.Id == Patient.Id);
                    if (dbPatient == null)
                    {
                        dbPatient = new DbPatient();
                        db.Patients.Add(dbPatient);
                    }
                    dbPatient.FirstName = patient.FirstName;
                    dbPatient.MiddleName = patient.MiddleName;
                    dbPatient.LastName = patient.LastName;
                    dbPatient.Phone = patient.Phone;
                    dbPatient.Adress = patient.Adress;
                    dbPatient.DateOfBirth = patient.DateOfBirth;
                    dbPatient.ImageBytes = patient.ImageBytes;
                    dbPatient.DoctorId = Doctor.Id;

                    foreach (var visit in patient.Visits)
                    {
                        var dbVisit = dbPatient.Visits.FirstOrDefault(v => v.Id == visit.Id);

                        if (visit.Deleted && dbVisit != null)
                        {
                            db.Visits.Remove(dbVisit);
                            dbPatient.Visits.Remove(dbVisit);
                        }

                        if (visit.Changed)
                        {
                           if(dbVisit == null)
                           {
                                dbVisit = new DbVisit();
                                dbVisit.Id = visit.Id;
                                dbPatient.Visits.Add(dbVisit);
                           }

                            dbVisit.Date = visit.Date;
                            dbVisit.Symptome = visit.Symptome;
                            dbVisit.Diagnose = visit.Diagnose;
                            dbVisit.Treatment = visit.Treatment;
                        }
                    }

                    db.SaveChanges();

                    patient.Visits = new ObservableCollection<Visit>(dbPatient.Visits.Select(d => new Visit(d)));

                    var existingPatient = Doctor.Patients.FirstOrDefault(p => p.Id == dbPatient.Id);
                    if(existingPatient == null)
                    {
                        patient.Id = dbPatient.Id;
                        Doctor.Patients.Insert(0, patient);
                    }
                    else
                    {
                        existingPatient.FirstName = patient.FirstName;
                        existingPatient.MiddleName = patient.MiddleName;
                        existingPatient.LastName = patient.LastName;
                        existingPatient.Phone = patient.Phone;
                        existingPatient.Adress = patient.Adress;
                        existingPatient.DateOfBirth = patient.DateOfBirth;
                        existingPatient.ImageBytes = patient.ImageBytes;
                        existingPatient.Visits = patient.Visits;
                    }

                    Doctor.UpdateFilter();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                 var imageBytes = File.ReadAllBytes(openFileDialog.FileName);
                 UpdateAvatarFromImageBytes(imageBytes);
                 Patient.ImageBytes = imageBytes;
            }
        }
    }
}
