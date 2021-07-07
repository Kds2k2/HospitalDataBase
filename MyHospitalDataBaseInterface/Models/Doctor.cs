using MyHospitalDataBaseInterface.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MyHospitalDataBaseInterface.Models
{
    public class Doctor : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string PinCode { get; set; }

        private string filter;
        private string firstName;
        private string lastName;
        private string middleName;
        private string specialty;
        private string organization;
        private byte[] imageBytes;
        private BitmapImage imageSource;

        private ObservableCollection<Patient> patients;
        private ObservableCollection<Patient> filteredPatients;

        public Doctor(DbDoctor dbDoctor)
        {
            if (dbDoctor == null)
                throw new ArgumentNullException(nameof(dbDoctor));

            Id = dbDoctor.Id;
            Specialty = dbDoctor.Specialty;
            Organization = dbDoctor.Organization;
            ImageBytes = dbDoctor.ImageBytes;
            FirstName = dbDoctor.FirstName;
            MiddleName = dbDoctor.MiddleName;
            LastName = dbDoctor.LastName;

            Patients = new ObservableCollection<Patient>(dbDoctor.Patients.Select(d => new Patient(d)));
            FilteredPatients = new ObservableCollection<Patient>(Patients);
        }

        public string Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                UpdateFilter();
                OnPropertyChanged(nameof(Filter));
            }
        }

        public string FullName
        {
            get { return $"{FirstName} {MiddleName} {LastName}"; }
        }

        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string MiddleName
        {
            get { return middleName; }
            set
            {
                middleName = value;
                OnPropertyChanged(nameof(MiddleName));
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public string Specialty
        {
            get { return specialty; }
            set
            {
                specialty = value;
                OnPropertyChanged(nameof(Specialty));
            }
        }

        public string Organization
        {
            get { return organization; }
            set
            {
                organization = value;
                OnPropertyChanged(nameof(Organization));
            }
        }

        public byte[] ImageBytes
        {
            get { return imageBytes; }
            set
            {
                imageBytes = value;
                if (imageBytes != null)
                {
                    BitmapImage img = new BitmapImage();
                    using (MemoryStream memStream = new MemoryStream(imageBytes))
                    {
                        memStream.Position = 0;
                        img.BeginInit();
                        img.StreamSource = memStream;
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.EndInit();
                    }
                    imageSource = img;
                }
                else
                {
                    imageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/DefaultUser.png"));
                }
                OnPropertyChanged(nameof(ImageBytes));
                OnPropertyChanged(nameof(ImageSource));
            }
        }

        public void UpdateFilter()
        {
            if (string.IsNullOrEmpty(filter))
            {
                FilteredPatients = new ObservableCollection<Patient>(Patients);
            }
            else
            {
                var patients = Patients.Where(p => p.FullName.ToLower().Contains(filter.ToLower()));
                FilteredPatients = new ObservableCollection<Patient>(patients);
            }
        }

        public BitmapImage ImageSource
        {
            get { return imageSource; }
        }

        public ObservableCollection<Patient> Patients
        {
            get { return patients; }
            set
            {
                patients = value;
                OnPropertyChanged(nameof(Patients));
            }
        }

        public ObservableCollection<Patient> FilteredPatients
        {
            get { return filteredPatients; }
            set
            {
                filteredPatients = value;
                OnPropertyChanged(nameof(FilteredPatients));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
