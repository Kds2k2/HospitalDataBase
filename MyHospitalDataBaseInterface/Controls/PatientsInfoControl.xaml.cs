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

namespace MyHospitalDataBaseInterface.Controls
{
    /// <summary>
    /// Interaction logic for PatientInfoControl.xaml
    /// </summary>
    public partial class PatientInfoControl : UserControl
    {
        public PatientInfoControl()
        {
            InitializeComponent();
           //PatientsList.ItemsSource = listVisit;
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
