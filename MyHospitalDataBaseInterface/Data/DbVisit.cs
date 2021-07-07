namespace MyHospitalDataBaseInterface.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Visit")]
    public partial class DbVisit
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        [StringLength(1024)]
        public string Diagnose { get; set; }

        [StringLength(1024)]
        public string Symptome { get; set; }

        [StringLength(1024)]
        public string Treatment { get; set; }

        public int PatientId { get; set; }

        public virtual DbPatient Patient { get; set; }
    }
}
