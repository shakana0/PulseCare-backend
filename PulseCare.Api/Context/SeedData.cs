using PulseCare.API.Context;
using PulseCare.API.Data.Entities.Communication;
using PulseCare.API.Data.Entities.Medical;
using PulseCare.API.Data.Entities.Users;
using PulseCare.API.Data.Enums;

namespace PulseCare.API.Context;

public static class SeedData
{
    public static void Initialize(PulseCareDbContext db)
    {
        if (db.Users.Any())
            return;

        // ------------------------- USERS -------------------------
        var patientUser1 = new User
        {
            Id = Guid.NewGuid(),
            Email = "patient1@example.com",
            Name = "John Doe",
            Role = UserRoleType.Patient,
            Avatar = "patient-avatar1.png"
        };

        var patientUser2 = new User
        {
            Id = Guid.NewGuid(),
            Email = "patient2@example.com",
            Name = "Alice Johnson",
            Role = UserRoleType.Patient,
            Avatar = "patient-avatar2.png"
        };

        var doctorUser1 = new User
        {
            Id = Guid.NewGuid(),
            Email = "doctor1@example.com",
            Name = "Dr. Sarah Smith",
            Role = UserRoleType.Doctor,
            Avatar = "doctor-avatar1.png"
        };

        var doctorUser2 = new User
        {
            Id = Guid.NewGuid(),
            Email = "doctor2@example.com",
            Name = "Dr. Michael Brown",
            Role = UserRoleType.Doctor,
            Avatar = "doctor-avatar2.png"
        };

        db.Users.AddRange(patientUser1, patientUser2, doctorUser1, doctorUser2);
        db.SaveChanges();

        // ------------------------- PATIENT PROFILES -------------------------
        var patient1 = new Patient
        {
            Id = Guid.NewGuid(),
            UserId = patientUser1.Id,
            DateOfBirth = new DateTime(1990, 5, 12),
            BloodType = "O+",
            CreatedAt = DateTime.UtcNow
        };

        var patient2 = new Patient
        {
            Id = Guid.NewGuid(),
            UserId = patientUser2.Id,
            DateOfBirth = new DateTime(1985, 3, 20),
            BloodType = "A-",
            CreatedAt = DateTime.UtcNow
        };

        db.Patients.AddRange(patient1, patient2);
        db.SaveChanges();

        // ------------------------- DOCTOR PROFILES -------------------------
        var doctor1 = new Doctor
        {
            Id = Guid.NewGuid(),
            UserId = doctorUser1.Id,
            Specialty = "Cardiology"
        };

        var doctor2 = new Doctor
        {
            Id = Guid.NewGuid(),
            UserId = doctorUser2.Id,
            Specialty = "General Medicine"
        };

        db.Doctors.AddRange(doctor1, doctor2);
        db.SaveChanges();

        // ------------------------- EMERGENCY CONTACTS -------------------------
        var emergency1 = new EmergencyContact
        {
            Name = "Jane Doe",
            Phone = "0701234567",
            Relationship = "Sister",
            PatientId = patient1.Id
        };

        var emergency2 = new EmergencyContact
        {
            Name = "Bob Johnson",
            Phone = "0709876543",
            Relationship = "Brother",
            PatientId = patient2.Id
        };

        db.EmergencyContacts.AddRange(emergency1, emergency2);
        db.SaveChanges();

        // ------------------------- ALLERGIES -------------------------
        db.Allergies.AddRange(
            new Allergy { Id = Guid.NewGuid(), PatientId = patient1.Id, Name = "Peanuts" },
            new Allergy { Id = Guid.NewGuid(), PatientId = patient1.Id, Name = "Penicillin" },
            new Allergy { Id = Guid.NewGuid(), PatientId = patient2.Id, Name = "Shellfish" }
        );
        db.SaveChanges();

        // ------------------------- CONDITIONS -------------------------
        db.Conditions.AddRange(
            new Condition { Id = Guid.NewGuid(), PatientId = patient1.Id, Name = "Hypertension" },
            new Condition { Id = Guid.NewGuid(), PatientId = patient2.Id, Name = "Diabetes" },
            new Condition { Id = Guid.NewGuid(), PatientId = patient2.Id, Name = "Asthma" }
        );
        db.SaveChanges();

        // ------------------------- MEDICATIONS -------------------------
        db.Medications.AddRange(
            new Medication
            {
                Id = Guid.NewGuid(),
                PatientId = patient1.Id,
                Name = "Atenolol",
                Dosage = "50mg",
                Frequency = "Daily",
                TimesPerDay = 1,
                StartDate = DateTime.UtcNow.AddMonths(-2),
                EndDate =  DateTime.UtcNow.AddMonths(1),
                Instructions = "Take in the morning"
            },
            new Medication
            {
                Id = Guid.NewGuid(),
                PatientId = patient2.Id,
                Name = "Metformin",
                Dosage = "500mg",
                Frequency = "Twice daily",
                TimesPerDay = 2,
                StartDate = DateTime.UtcNow.AddMonths(-1),
                EndDate = DateTime.UtcNow.AddMonths(1),
                Instructions = "Take with meals"
            }
        );
        db.SaveChanges();

        // ------------------------- HEALTH STATS -------------------------
        db.HealthStats.AddRange(
            new HealthStat
            {
                Id = Guid.NewGuid(),
                PatientId = patient1.Id,
                Type = HealthStatType.HeartRate,
                Value = "72",
                Unit = "bpm",
                Status = HealthStatusType.Normal,
                Date = DateTime.UtcNow.AddDays(-1)
            },
            new HealthStat
            {
                Id = Guid.NewGuid(),
                PatientId = patient1.Id,
                Type = HealthStatType.BloodPressure,
                Value = "120/80",
                Unit = "mmHg",
                Status = HealthStatusType.Normal,
                Date = DateTime.UtcNow.AddDays(-2)
            },
            new HealthStat
            {
                Id = Guid.NewGuid(),
                PatientId = patient2.Id,
                Type = HealthStatType.Glucose,
                Value = "140",
                Unit = "mg/dL",
                Status = HealthStatusType.Warning,
                Date = DateTime.UtcNow.AddDays(-1)
            },
            new HealthStat
            {
                Id = Guid.NewGuid(),
                PatientId = patient2.Id,
                Type = HealthStatType.Weight,
                Value = "70",
                Unit = "kg",
                Status = HealthStatusType.Normal,
                Date = DateTime.UtcNow.AddDays(-3)
            }
        );
        db.SaveChanges();

        // ------------------------- APPOINTMENTS -------------------------
        var appointment1 = new Appointment
        {
            Id = Guid.NewGuid(),
            PatientId = patient1.Id,
            DoctorId = doctor1.Id,
            Date = DateTime.UtcNow.AddDays(-3),
            Time = new TimeSpan(10, 0, 0),
            Type = AppointmentType.Checkup,
            Status = AppointmentStatusType.Completed,
            Comment = "Regular check-up"
        };

        var appointment2 = new Appointment
        {
            Id = Guid.NewGuid(),
            PatientId = patient2.Id,
            DoctorId = doctor2.Id,
            Date = DateTime.UtcNow.AddDays(1),
            Time = new TimeSpan(14, 30, 0),
            Type = AppointmentType.FollowUp,
            Status = AppointmentStatusType.Scheduled,
            Comment = "Follow-up on diabetes management"
        };

        db.Appointments.AddRange(appointment1, appointment2);
        db.SaveChanges();

        // ------------------------- NOTES -------------------------
        db.Notes.AddRange(
            new Note
            {
                Id = Guid.NewGuid(),
                AppointmentId = appointment1.Id,
                PatientId = patient1.Id,
                DoctorId = doctor1.Id,
                Title = "Blood Pressure Review",
                Diagnosis = "Stable condition",
                Content = "Patient shows good progress. Continue medication.",
                Date = DateTime.UtcNow.AddDays(-3)
            },
            new Note
            {
                Id = Guid.NewGuid(),
                AppointmentId = appointment2.Id,
                PatientId = patient2.Id,
                DoctorId = doctor2.Id,
                Title = "Diabetes Follow-Up",
                Diagnosis = "Needs adjustment",
                Content = "Increase Metformin dosage and monitor glucose levels.",
                Date = DateTime.UtcNow.AddDays(1)
            }
        );
        db.SaveChanges();

        // ------------------------- CONVERSATIONS -------------------------
        var conversation1 = new Conversation
        {
            Id = Guid.NewGuid(),
            PatientId = patient1.Id,
            DoctorId = doctor1.Id
        };

        var conversation2 = new Conversation
        {
            Id = Guid.NewGuid(),
            PatientId = patient2.Id,
            DoctorId = doctor2.Id
        };

        db.Conversations.AddRange(conversation1, conversation2);
        db.SaveChanges();

        // ------------------------- MESSAGES -------------------------
        db.Messages.AddRange(
            new Message
            {
                Id = Guid.NewGuid(),
                ConversationId = conversation1.Id,
                Subject = "Medication Question",
                Content = "Should I take my medication before or after breakfast?",
                Date = DateTime.UtcNow.AddDays(-1),
                Read = false,
                FromPatient = true
            },
            new Message
            {
                Id = Guid.NewGuid(),
                ConversationId = conversation1.Id,
                Subject = "Re: Medication Question",
                Content = "Take it after breakfast.",
                Date = DateTime.UtcNow,
                Read = false,
                FromPatient = false
            },
            new Message
            {
                Id = Guid.NewGuid(),
                ConversationId = conversation2.Id,
                Subject = "Appointment Reminder",
                Content = "Don't forget your follow-up appointment tomorrow.",
                Date = DateTime.UtcNow.AddHours(-2),
                Read = true,
                FromPatient = false
            }
        );
        db.SaveChanges();

        // ------------------------- HEALTH TIPS -------------------------
        db.HealthTips.AddRange(
            new HealthTip
            {
                Id = Guid.NewGuid(),
                PatientId = patient1.Id,
                Category = HealthTipCategoryType.Medication,
                Title = "Medication Timing",
                Content = "Take your blood pressure medication at the same time every day.",
                Icon = "Pill"
            },
            new HealthTip
            {
                Id = Guid.NewGuid(),
                PatientId = patient1.Id,
                Category = HealthTipCategoryType.Nutrition,
                Title = "Hydration Matters",
                Content = "Drink 8 glasses of water daily to support kidney function.",
                Icon = "Droplets"
            },
            new HealthTip
            {
                Id = Guid.NewGuid(),
                PatientId = patient2.Id,
                Category = HealthTipCategoryType.Exercise,
                Title = "Daily Walk",
                Content = "Aim for 30 minutes of walking daily to manage blood sugar.",
                Icon = "Walking"
            },
            new HealthTip
            {
                Id = Guid.NewGuid(),
                PatientId = patient2.Id,
                Category = HealthTipCategoryType.Sleep,
                Title = "Quality Sleep",
                Content = "Get 7-8 hours of sleep to improve overall health.",
                Icon = "Moon"
            }
        );
        db.SaveChanges();
    }
}
