using Microsoft.EntityFrameworkCore;
using PulseCare.API.Data.Entities.Communication;
using PulseCare.API.Data.Entities.Medical;
using PulseCare.API.Data.Entities.Users;

namespace PulseCare.API.Context;

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
    public DbSet<Conversation> Conversations => Set<Conversation>();
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

        // USER RELATIONS

        modelBuilder.Entity<Patient>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Doctor>()
            .HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // EMERGENCY CONTACT

        modelBuilder.Entity<Patient>()
           .HasOne(p => p.EmergencyContact)
           .WithOne(ec => ec.Patient)
           .HasForeignKey<EmergencyContact>(ec => ec.PatientId)
           .OnDelete(DeleteBehavior.Cascade);

        // APPOINTMENTS

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        // NOTES

        modelBuilder.Entity<Note>()
            .HasOne(n => n.Doctor)
            .WithMany(d => d.Notes)
            .HasForeignKey(n => n.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Note>()
            .HasOne(n => n.Patient)
            .WithMany(p => p.Notes)
            .HasForeignKey(n => n.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Note>()
            .HasOne(n => n.Appointment)
            .WithMany(a => a.AppointmentNotes)
            .HasForeignKey(n => n.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // COMMUNICATION 

        // Conversation → Patient
        modelBuilder.Entity<Conversation>()
            .HasOne(c => c.Patient)
            .WithMany(p => p.Conversations)
            .HasForeignKey(c => c.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        // Conversation → Doctor
        modelBuilder.Entity<Conversation>()
            .HasOne(c => c.Doctor)
            .WithMany(d => d.Conversations)
            .HasForeignKey(c => c.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Message → Conversation
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Conversation)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);

        // MEDICAL DATA

        modelBuilder.Entity<Medication>()
            .HasOne(m => m.Patient)
            .WithMany(p => p.Medications)
            .HasForeignKey(m => m.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<HealthStat>()
            .HasOne(h => h.Patient)
            .WithMany(p => p.HealthStats)
            .HasForeignKey(h => h.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Allergy>()
            .HasOne(a => a.Patient)
            .WithMany(p => p.Allergies)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Condition>()
            .HasOne(c => c.Patient)
            .WithMany(p => p.Conditions)
            .HasForeignKey(c => c.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<HealthTip>()
            .HasOne(ht => ht.Patient)
            .WithMany(p => p.HealthTips)
            .HasForeignKey(ht => ht.PatientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
