using MyHospitalDataBaseInterface.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyHospitalDataBaseInterface.Models
{
    public class Visit : INotifyPropertyChanged, ICloneable
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
        public bool Changed { get; set; }

        private DateTime date;
        private string diagnose;
        private string symptome;
        private string treatment;

        public Visit() { }

        public Visit(DbVisit dbVisit)
        {
            if (dbVisit == null)
                throw new ArgumentNullException(nameof(dbVisit));

            Id = dbVisit.Id;
            Date = dbVisit.Date;
            Diagnose = dbVisit.Diagnose;
            Symptome = dbVisit.Symptome;
            Treatment = dbVisit.Treatment;
        }

        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public string Diagnose
        {
            get { return diagnose; }
            set
            {
                diagnose = value;
                OnPropertyChanged(nameof(Diagnose));
            }
        }

        public string Symptome
        {
            get { return symptome; }
            set
            {
                symptome = value;
                OnPropertyChanged(nameof(Symptome));
            }
        }

        public string Treatment
        {
            get { return treatment; }
            set
            {
                treatment = value;
                OnPropertyChanged(nameof(Treatment));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public Visit Clone()
        {
            return new Visit()
            {
                Id = this.Id,
                Date = this.Date,
                Symptome = this.Symptome,
                Diagnose = this.Diagnose,
                Treatment = this.Treatment
            };
        }

        object ICloneable.Clone() 
        {
            return this.Clone();
        }
    }
}
