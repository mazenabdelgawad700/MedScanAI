using MedScanAI.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedScanAI.Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<PatientAllergy> PatientAllergies { get; set; }
        public DbSet<PatientChronicDisease> PatientChronicDiseases { get; set; }
        public DbSet<PatientCurrentMedication> PatientCurrentMedications { get; set; }
        public DbSet<AIChatSession> AIChatSessions { get; set; }
        public DbSet<AIChatMessage> AIChatMessages { get; set; }
        public DbSet<AIReport> AIReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ============================================
            // PATIENT RELATIONSHIPS
            // ============================================

            // Patient 1-to-1 with ApplicationUser
            modelBuilder.Entity<Patient>()
                .HasOne(p => p.ApplicationUser)
                .WithOne(u => u.Patient)
                .HasForeignKey<Patient>(p => p.Id)
                .OnDelete(DeleteBehavior.Cascade);

            // Patient 1-to-Many with Allergies
            modelBuilder.Entity<PatientAllergy>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Allergies)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Patient 1-to-Many with Chronic Diseases
            modelBuilder.Entity<PatientChronicDisease>()
                .HasOne(d => d.Patient)
                .WithMany(p => p.ChronicDiseases)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Patient 1-to-Many with Current Medications
            modelBuilder.Entity<PatientCurrentMedication>()
                .HasOne(m => m.Patient)
                .WithMany(p => p.CurrentMedications)
                .HasForeignKey(m => m.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Patient 1-to-Many with Chat Sessions
            modelBuilder.Entity<AIChatSession>()
                .HasOne(s => s.Patient)
                .WithMany(p => p.ChatSessions)
                .HasForeignKey(s => s.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Patient 1-to-Many with AI Reports

            // Patient 1-to-Many with Appointments
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // ============================================
            // DOCTOR RELATIONSHIPS
            // ============================================

            // Doctor 1-to-1 with ApplicationUser
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.ApplicationUser)
                .WithOne(u => u.Doctor)
                .HasForeignKey<Doctor>(d => d.Id)
                .OnDelete(DeleteBehavior.Cascade);

            // Doctor Many-to-1 with Specialization
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Specialization)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.SpecializationId)
                .OnDelete(DeleteBehavior.Restrict); // Don't delete doctor if specialization is deleted

            // Doctor 1-to-Many with Schedules
            modelBuilder.Entity<DoctorSchedule>()
                .HasOne(s => s.Doctor)
                .WithMany(d => d.Schedules)
                .HasForeignKey(s => s.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Doctor 1-to-Many with Appointments
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // ============================================
            // AI CHAT SESSION RELATIONSHIPS
            // ============================================

            // AIChatSession 1-to-Many with Messages
            modelBuilder.Entity<AIChatMessage>()
                .HasOne(m => m.AIChatSession)
                .WithMany(s => s.Messages)
                .HasForeignKey(m => m.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // ============================================
            // APPOINTMENT RELATIONSHIPS
            // ============================================

            // Appointment Optional 1-to-1 with AIReport
            modelBuilder.Entity<AIReport>()
                .HasOne(r => r.Appointment)
                .WithOne(a => a.AIReport)
                .HasForeignKey<AIReport>(r => r.AppointmentId)
                .OnDelete(DeleteBehavior.SetNull); // If appointment deleted, set AppointmentId to null

            // ============================================
            // INDEXES FOR PERFORMANCE
            // ============================================

            // Patient indexes
            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.Email)
                .IsUnique();

            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.PhoneNumber);

            // Doctor indexes
            modelBuilder.Entity<Doctor>()
                .HasIndex(d => d.SpecializationId);

            modelBuilder.Entity<Doctor>()
                .HasIndex(d => d.IsActive);

            // Appointment indexes
            modelBuilder.Entity<Appointment>()
                .HasIndex(a => a.PatientId);

            modelBuilder.Entity<Appointment>()
                .HasIndex(a => a.DoctorId);

            modelBuilder.Entity<Appointment>()
                .HasIndex(a => a.Date);

            modelBuilder.Entity<Appointment>()
                .HasIndex(a => a.Status);

            // DoctorSchedule indexes
            modelBuilder.Entity<DoctorSchedule>()
                .HasIndex(s => new { s.DoctorId, s.DayOfWeek });

            // AIChatSession indexes
            modelBuilder.Entity<AIChatSession>()
                .HasIndex(s => s.PatientId);

            modelBuilder.Entity<AIChatSession>()
                .HasIndex(s => s.StartedAt);

            // AIReport indexes
            modelBuilder.Entity<AIReport>()
                .HasIndex(r => r.PatientId);

            modelBuilder.Entity<AIReport>()
                .HasIndex(r => r.AppointmentId);

            // ============================================
            // SEED DATA
            // ============================================

            modelBuilder.Entity<Specialization>().HasData(
                new Specialization { Id = 1, Name = "الأمراض الجلدية", Description = "تشخيص وعلاج أمراض الجلد والشعر والأظافر" },
                new Specialization { Id = 2, Name = "طب الأعصاب", Description = "تشخيص وعلاج أمراض واضطرابات الدماغ والجهاز العصبي" },
                new Specialization { Id = 3, Name = "أمراض الصدر", Description = "تشخيص وعلاج أمراض الجهاز التنفسي والرئتين" }
            );

        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }
        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Entity.GetType().GetProperty("UpdatedAt") != null)
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Added &&
                    entry.Entity.GetType().GetProperty("CreatedAt") != null)
                {
                    entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                }
            }
        }
    }
}
