using PulseCare.API.Context;
using PulseCare.API.Data.Entities.Communication;
using PulseCare.API.Data.Entities.Medical;
using PulseCare.API.Data.Entities.Users;
using PulseCare.API.Data.Enums;

namespace PulseCare.Api.Context
{
    public static class SeedData
    {
        public static void Initialize(PulseCareDbContext db)
        {
            if (db.Users.Any())
                return;

            // -------------------------
            // USERS
            // -------------------------
            var patientUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "patient@example.com",
                Name = "John Doe",
                Role = UserRole.Patient,
                Avatar = "patient-avatar.png"
            };

            var doctorUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "doctor@example.com",
                Name = "Dr. Sarah Smith",
                Role = UserRole.Admin,
                Avatar = "doctor-avatar.png"
            };

            db.Users.AddRange(patientUser, doctorUser);
            db.SaveChanges();

            // -------------------------
            // PATIENT PROFILE
            // -------------------------
            var patient = new Patient
            {
                Id = Guid.NewGuid(),
                UserId = patientUser.Id,
                DateOfBirth = new DateTime(1990, 5, 12),
                BloodType = "O+",
                CreatedAt = DateTime.UtcNow
            };

            db.Patients.Add(patient);
            db.SaveChanges();

            // -------------------------
            // DOCTOR PROFILE
            // -------------------------
            var doctor = new Doctor
            {
                Id = Guid.NewGuid(),
                UserId = doctorUser.Id,
                Specialty = "Cardiology"
            };

            db.Doctors.Add(doctor);
            db.SaveChanges();

            // -------------------------
            // EMERGENCY CONTACT
            // -------------------------
            var emergency = new EmergencyContact
            {
                Name = "Jane Doe",
                Phone = "0701234567",
                Relationship = "Sister",
                PatientId = patient.Id
            };

            db.EmergencyContacts.Add(emergency);
            db.SaveChanges();

            // -------------------------
            // ALLERGIES
            // -------------------------
            db.Allergies.Add(new Allergy
            {
                Id = Guid.NewGuid(),
                PatientId = patient.Id,
                Name = "Peanuts"
            });

            db.Allergies.Add(new Allergy
            {
                Id = Guid.NewGuid(),
                PatientId = patient.Id,
                Name = "Penicillin"
            });

            db.SaveChanges();

            // -------------------------
            // CONDITIONS
            // -------------------------
            db.Conditions.Add(new Condition
            {
                Id = Guid.NewGuid(),
                PatientId = patient.Id,
                Name = "Hypertension"
            });

            db.SaveChanges();

            // -------------------------
            // MEDICATIONS
            // -------------------------
            db.Medications.Add(new Medication
            {
                Id = Guid.NewGuid(),
                PatientId = patient.Id,
                Name = "Atenolol",
                Dosage = "50mg",
                Frequency = "Daily",
                TimesPerDay = 1,
                StartDate = DateTime.UtcNow.AddMonths(-2),
                Instructions = "Take in the morning"
            });

            db.SaveChanges();

            // -------------------------
            // HEALTH STATS
            // -------------------------
            db.HealthStats.Add(new HealthStat
            {
                Id = Guid.NewGuid(), 
                PatientId = patient.Id, 
                Type = HealthStatType.HeartRate, 
                Value = "72", Unit = "bpm", 
                Status = HealthStatusType.Normal, 
                Date = DateTime.UtcNow.AddDays(-1)
            });

            db.SaveChanges();

            // -------------------------
            // APPOINTMENT
            // -------------------------
            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                PatientId = patient.Id,
                DoctorId = doctor.Id,
                Date = DateTime.UtcNow.AddDays(-3),
                Status = AppointmentStatusType.Completed
            };

            db.Appointments.Add(appointment);
            db.SaveChanges();

            // -------------------------
            // NOTE
            // -------------------------
            db.Notes.Add(new Note
            {
                Id = Guid.NewGuid(),
                AppointmentId = appointment.Id,
                PatientId = patient.Id,
                DoctorId = doctor.Id,
                Title = "Blood Pressure Review",
                Diagnosis = "Stable condition",
                Content = "Patient shows good progress. Continue medication.",
                Date = DateTime.UtcNow.AddDays(-3),
            });

            db.SaveChanges();

            // -------------------------
            // MESSAGE
            // -------------------------
            db.Messages.Add(new Message
            {
                Id = Guid.NewGuid(),
                PatientId = patient.Id,
                DoctorId = doctor.Id,
                Subject = "Medication Question",
                Content = "Should I take my medication before or after breakfast?",
                Date = DateTime.UtcNow.AddDays(-1),
                Read = false,
                FromPatient = true
            });

            db.Messages.Add(new Message
            {
                Id = Guid.NewGuid(),
                PatientId = patient.Id,
                DoctorId = doctor.Id,
                Subject = "Re: Medication Question",
                Content = "Take it after breakfast.",
                Date = DateTime.UtcNow,
                Read = false,
                FromPatient = false
            });

            db.SaveChanges();

            // -------------------------
            // HEALTH TIPS (PERSONALIZED)
            // -------------------------
            db.HealthTips.Add(new HealthTip
            {
                Id = Guid.NewGuid(),
                PatientId = patient.Id,
                Category = HealthTipCategory.Medication,
                Title = "Medication Timing",
                Content = "Take your blood pressure medication at the same time every day.",
                Icon = "Pill"
            });

            db.HealthTips.Add(new HealthTip
            {
                Id = Guid.NewGuid(),
                PatientId = patient.Id,
                Category = HealthTipCategory.Nutrition,
                Title = "Hydration Matters",
                Content = "Drink 8 glasses of water daily to support kidney function.",
                Icon = "Droplets"
            });

            db.SaveChanges();
        }
    }
}