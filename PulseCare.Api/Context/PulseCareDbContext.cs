using Microsoft.EntityFrameworkCore;
using PulseCare.API.Data.Entities.Communication;
using PulseCare.API.Data.Entities.Medical;
using PulseCare.API.Data.Entities.Users;

namespace PulseCare.API.Context;
{
    public class PulseCareDbContext : DbContext
    {
        public PulseCareDbContext(DbContextOptions<PulseCareDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<Note> Notes => Set<Note>();
        public DbSet<Medication> Medications => Set<Medication>();
        public DbSet<HealthStat> HealthStats => Set<HealthStat>();
        public DbSet<Allergy> Allergies => Set<Allergy>();
        public DbSet<Condition> Conditions => Set<Condition>();
        public DbSet<EmergencyContact> EmergencyContacts => Set<EmergencyContact>();
        public DbSet<HealthTip> HealthTips => Set<HealthTip>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Patient>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmergencyContact>()
                .HasOne(ec => ec.Patient)
                .WithMany(p => p.EmergencyContacts)
                .HasForeignKey(ec => ec.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId);

            // Message → Patient & Doctor
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Patient)
                .WithMany(p => p.Messages)
                .HasForeignKey(m => m.PatientId);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Doctor)
                .WithMany(d => d.Messages)
                .HasForeignKey(m => m.DoctorId);

            // Note → Appointment, Patient, Doctor
            modelBuilder.Entity<Note>()
                .HasOne(n => n.Appointment)
                .WithMany(a => a.AppointmentNotes)
                .HasForeignKey(n => n.AppointmentId);

            modelBuilder.Entity<Note>()
                .HasOne(n => n.Patient)
                .WithMany(p => p.Notes)
                .HasForeignKey(n => n.PatientId);

            modelBuilder.Entity<Note>()
                .HasOne(n => n.Doctor)
                .WithMany(d => d.Notes)
                .HasForeignKey(n => n.DoctorId);

            // Medication → Patient
            modelBuilder.Entity<Medication>()
                .HasOne(m => m.Patient)
                .WithMany(p => p.Medications)
                .HasForeignKey(m => m.PatientId);

            // HealthStat → Patient
            modelBuilder.Entity<HealthStat>()
                .HasOne(h => h.Patient)
                .WithMany(p => p.HealthStats)
                .HasForeignKey(h => h.PatientId);

            // Allergy → Patient
            modelBuilder.Entity<Allergy>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Allergies)
                .HasForeignKey(a => a.PatientId);

            // Condition → Patient
            modelBuilder.Entity<Condition>()
                .HasOne(c => c.Patient)
                .WithMany(p => p.Conditions)
                .HasForeignKey(c => c.PatientId);

            // HealthTip → Patient (if personalized)
            modelBuilder.Entity<HealthTip>()
                .HasOne(ht => ht.Patient)
                .WithMany(p => p.HealthTips)
                .HasForeignKey(ht => ht.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
