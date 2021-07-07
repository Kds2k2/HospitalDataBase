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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFCustomMessageBox;

namespace MyHospitalDataBaseInterface.Controls
{
    /// <summary>
    /// Interaction logic for PatientVisitsControl.xaml
    /// </summary>
    public partial class PatientVisitsControl : UserControl
    {
        public PatientVisitsControl()
        {
            InitializeComponent();

        }

        private void DeleteVisitButton_Click(object sender, RoutedEventArgs e)
        {
            if (VisitsList.SelectedItem == null)
            {
                MessageBox.Show("Будь-ласка виберіть візит", "Редагувати візит", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBoxResult result = CustomMessageBox.ShowYesNo(
               "Ви впевнені що хочете видалити візит?",
               "Видалити візит",
               "Прийняти",
               "Відхилити",
            MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var visit = (VisitsList.SelectedItem as Visit);
                var patient = (DataContext as Patient);
                visit.Deleted = true;
                patient.UpdateAvailableVisits();
            }
        }

        private void EditVisitButton_Click(object sender, RoutedEventArgs e)
        {
            if (VisitsList.SelectedItem == null)
            {
                MessageBox.Show("Будь-ласка виберіть візит", "Редагувати візит", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var visit = VisitsList.SelectedItem as Visit;
            VisitInfo.DataContext = visit.Clone();

            VisitInfo.Visibility = Visibility.Visible;
            VisitsGrid.Visibility = Visibility.Hidden;
        }

        private void CreateVisitButton_Click(object sender, RoutedEventArgs e)
        {
            var visit = new Visit();
            var patient = (DataContext as Patient);
            visit.Id = -(patient.Visits.Count+1);
            VisitInfo.DataContext = visit;

            VisitInfo.Visibility = Visibility.Visible;
            VisitsGrid.Visibility = Visibility.Hidden;
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            var visit = (VisitInfo.DataContext as Visit);
            var patient = (DataContext as Patient);
            var result = patient.Visits.FirstOrDefault(v => v.Id == visit.Id);

            if(result == null)
            {
                visit.Date = DateTime.Now;
                visit.Changed = true;
                patient.Visits.Insert(0,visit);
            }
            else
            {
                result.Symptome = visit.Symptome;
                result.Diagnose = visit.Diagnose;
                result.Treatment = visit.Treatment;
                result.Changed = true;
            }

            patient.UpdateAvailableVisits();

            VisitInfo.Visibility = Visibility.Hidden;
            VisitsGrid.Visibility = Visibility.Visible;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            VisitInfo.DataContext = null;
            VisitInfo.Visibility = Visibility.Hidden;
            VisitsGrid.Visibility = Visibility.Visible;
        }
    }
}
