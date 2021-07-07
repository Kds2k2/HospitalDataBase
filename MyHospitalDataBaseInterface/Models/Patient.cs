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
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MyHospitalDataBaseInterface.Models
{
    public class Patient : INotifyPropertyChanged, ICloneable
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }

        private string firstName;
        private string lastName;
        private string middleName;
        private string phone;
        private string adress;
        private DateTime dateofbirth;
        private byte[] imageBytes;

        private BitmapImage imageSource;

        private ObservableCollection<Visit> visits;
        private ObservableCollection<Visit> availableVisits;

        public Patient()
        {
            Visits = new ObservableCollection<Visit>();
            imageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/DefaultUser.png"));
            dateofbirth = DateTime.Now;
        }

        public Patient(DbPatient dbPatient)
        {
            if (dbPatient == null)
                throw new ArgumentNullException(nameof(dbPatient));

            Id = dbPatient.Id;
            Phone = dbPatient.Phone;
            Adress = dbPatient.Adress;
            ImageBytes = dbPatient.ImageBytes;
            DateOfBirth = dbPatient.DateOfBirth;
            FirstName = dbPatient.FirstName;
            MiddleName = dbPatient.MiddleName;
            LastName = dbPatient.LastName;

            Visits = new ObservableCollection<Visit>(dbPatient.Visits.Select(d => new Visit(d)));
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

        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged(nameof(LastName));
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

        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }

        public string Adress
        {
            get { return adress; }
            set
            {
                adress = value;
                OnPropertyChanged(nameof(Adress));
            }
        }

        public DateTime DateOfBirth
        {
            get { return dateofbirth; }
            set
            {
                dateofbirth = value;
                OnPropertyChanged(nameof(DateOfBirth));
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

        public BitmapImage ImageSource
        {
            get { return imageSource; } 
        }

        public ObservableCollection<Visit> Visits
        {
            get { return visits; }
            set
            {
                visits = value;
                UpdateAvailableVisits();
                OnPropertyChanged(nameof(Visits));
            }
        }

        public ObservableCollection<Visit> AvailableVisits
        {
            get { return availableVisits; }
            private set
            {
                availableVisits = value;
                OnPropertyChanged(nameof(AvailableVisits));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public void UpdateAvailableVisits()
        {
            var visits = new ObservableCollection<Visit>(Visits.Where(v => !v.Deleted).ToArray());
            AvailableVisits = visits;
        }

        public Patient Clone()
        {
            return new Patient()
            {
                Id = this.Id,
                Adress = this.Adress,
                Phone = this.Phone,
                FirstName = this.FirstName,
                MiddleName = this.MiddleName,
                LastName = this.LastName,
                ImageBytes = this.ImageBytes,
                DateOfBirth = this.DateOfBirth,
                Visits = new ObservableCollection<Visit>(this.Visits.Select(d => d.Clone()))
            };
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}
