namespace MyHospitalDataBaseInterface.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Doctor")]
    public partial class DbDoctor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DbDoctor()
        {
            Patients = new HashSet<DbPatient>();
        }

        public int Id { get; set; }

        //[StringLength(50)]
        //public string FullName { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Specialty { get; set; }

        [StringLength(50)]
        public string PinCode { get; set; }

        [StringLength(50)]
        public string Organization { get; set; }

        public byte[] ImageBytes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DbPatient> Patients { get; set; }
    }
}
