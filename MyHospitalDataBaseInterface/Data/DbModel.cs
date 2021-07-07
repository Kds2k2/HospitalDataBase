using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace MyHospitalDataBaseInterface.Data
{
    public partial class DbModel : DbContext
    {
        public DbModel()
            : base("name=DbModel")
        {
        }

        public virtual DbSet<DbDoctor> Doctors { get; set; }
        public virtual DbSet<DbPatient> Patients { get; set; }
        public virtual DbSet<DbVisit> Visits { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbDoctor>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<DbDoctor>()
                .Property(e => e.MiddleName)
                .IsUnicode(false);

            modelBuilder.Entity<DbDoctor>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<DbDoctor>()
                .Property(e => e.Specialty)
                .IsUnicode(false);

            modelBuilder.Entity<DbDoctor>()
                .Property(e => e.PinCode)
                .IsUnicode(false);

            modelBuilder.Entity<DbDoctor>()
                .Property(e => e.Organization)
                .IsUnicode(false);

            modelBuilder.Entity<DbDoctor>()
                .HasMany(e => e.Patients)
                .WithRequired(e => e.Doctor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DbPatient>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<DbPatient>()
                .Property(e => e.MiddleName)
                .IsUnicode(false);

            modelBuilder.Entity<DbPatient>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<DbPatient>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<DbPatient>()
                .Property(e => e.Adress)
                .IsUnicode(false);

            modelBuilder.Entity<DbPatient>()
                .Property(e => e.DateOfBirth);

            modelBuilder.Entity<DbPatient>()
                .HasMany(e => e.Visits)
                .WithRequired(e => e.Patient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DbVisit>()
                .Property(e => e.Date);

            modelBuilder.Entity<DbVisit>()
                .Property(e => e.Diagnose)
                .IsUnicode(false);

            modelBuilder.Entity<DbVisit>()
                .Property(e => e.Symptome)
                .IsUnicode(false);

            modelBuilder.Entity<DbVisit>()
                .Property(e => e.Treatment)
                .IsUnicode(false);
        }
    }
}
