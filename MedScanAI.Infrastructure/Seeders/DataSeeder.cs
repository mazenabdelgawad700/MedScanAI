using MedScanAI.Domain.Entities;
using MedScanAI.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;

namespace MedScanAI.Infrastructure.Seeders
{
    public class DataSeeder
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataSeeder(AppDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await EnsureRolesExistAsync();
            await SeedAdminAccountAsync();
            await SeedDoctorAccountAsync();
            await SeedPatientAccountAsync();
        }

        private async Task EnsureRolesExistAsync()
        {
            string[] roles = ["Admin", "Doctor", "Patient"];

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private async Task SeedAdminAccountAsync()
        {
            const string adminEmail = "mazenabdelgawad700@gmail.com";

            var existingAdmin = await _userManager.FindByEmailAsync(adminEmail);
            if (existingAdmin != null)
                return;

            var adminUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = adminEmail,
                NormalizedEmail = adminEmail.ToUpper(),
                EmailConfirmed = true,
                PhoneNumber = "+201158907731",
                PhoneNumberConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(adminUser, "String@1234");

            if (createResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        private async Task SeedDoctorAccountAsync()
        {
            const string doctorEmail = "ali@gmail.com";

            var existingDoctor = await _userManager.FindByEmailAsync(doctorEmail);
            if (existingDoctor != null)
                return;

            var doctorUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Ali",
                NormalizedUserName = "ALI",
                Email = doctorEmail,
                NormalizedEmail = doctorEmail.ToUpper(),
                EmailConfirmed = true,
                PhoneNumber = "+201158907731",
                PhoneNumberConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(doctorUser, "String@1234");

            if (!createResult.Succeeded)
                return;

            await _userManager.AddToRoleAsync(doctorUser, "Doctor");

            // Create Doctor entity
            var doctor = new Doctor
            {
                Id = doctorUser.Id,
                FullName = "Ali",
                Email = doctorEmail,
                PhoneNumber = "+201158907731",
                SpecializationId = 2,
                YearsOfExperience = 16,
                IsActive = true,
                ApplicationUser = doctorUser,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _dbContext.Doctors.AddAsync(doctor);
            await _dbContext.SaveChangesAsync();

            // Create Doctor Schedules
            var schedules = new List<DoctorSchedule>
            {
                new DoctorSchedule
                {
                    DoctorId = doctor.Id,
                    DayOfWeek = "saturday",
                    StartTime = new TimeSpan(5, 0, 0),
                    EndTime = new TimeSpan(23, 0, 0),
                    IsAvailable = true
                },
                new DoctorSchedule
                {
                    DoctorId = doctor.Id,
                    DayOfWeek = "sunday",
                    StartTime = new TimeSpan(5, 0, 0),
                    EndTime = new TimeSpan(23, 0, 0),
                    IsAvailable = true
                },
                new DoctorSchedule
                {
                    DoctorId = doctor.Id,
                    DayOfWeek = "monday",
                    StartTime = new TimeSpan(5, 0, 0),
                    EndTime = new TimeSpan(23, 0, 0),
                    IsAvailable = true
                },
                new DoctorSchedule
                {
                    DoctorId = doctor.Id,
                    DayOfWeek = "tuesday",
                    StartTime = new TimeSpan(5, 0, 0),
                    EndTime = new TimeSpan(23, 0, 0),
                    IsAvailable = true
                },
                new DoctorSchedule
                {
                    DoctorId = doctor.Id,
                    DayOfWeek = "wednesday",
                    StartTime = new TimeSpan(5, 0, 0),
                    EndTime = new TimeSpan(23, 0, 0),
                    IsAvailable = true
                },
                new DoctorSchedule
                {
                    DoctorId = doctor.Id,
                    DayOfWeek = "thursday",
                    StartTime = new TimeSpan(5, 0, 0),
                    EndTime = new TimeSpan(23, 0, 0),
                    IsAvailable = true
                },
                new DoctorSchedule
                {
                    DoctorId = doctor.Id,
                    DayOfWeek = "friday",
                    StartTime = new TimeSpan(5, 0, 0),
                    EndTime = new TimeSpan(23, 0, 0),
                    IsAvailable = true
                }
            };

            await _dbContext.DoctorSchedules.AddRangeAsync(schedules);
            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedPatientAccountAsync()
        {
            const string patientEmail = "malak@gmail.com";

            var existingPatient = await _userManager.FindByEmailAsync(patientEmail);
            if (existingPatient != null)
                return;

            var patientUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Malak",
                NormalizedUserName = "MALAK",
                Email = patientEmail,
                NormalizedEmail = patientEmail.ToUpper(),
                EmailConfirmed = true,
                PhoneNumber = "+201158907731",
                PhoneNumberConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(patientUser, "String@1234");

            if (!createResult.Succeeded)
                return;

            await _userManager.AddToRoleAsync(patientUser, "Patient");

            // Create Patient entity
            var patient = new Patient
            {
                Id = patientUser.Id,
                FullName = "Malak",
                Email = patientEmail,
                PhoneNumber = "+201158907731",
                Gender = "Female",
                DateOfBirth = new DateOnly(2004, 2, 5),
                ApplicationUser = patientUser,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _dbContext.Patients.AddAsync(patient);
            await _dbContext.SaveChangesAsync();

            // Create Chronic Diseases
            var chronicDiseases = new List<PatientChronicDisease>
            {
                new PatientChronicDisease
                {
                    PatientId = patient.Id,
                    Name = "الربو"
                }
            };

            await _dbContext.PatientChronicDiseases.AddRangeAsync(chronicDiseases);
            await _dbContext.SaveChangesAsync();

            // Create Allergies
            var allergies = new List<PatientAllergy>
            {
                new PatientAllergy
                {
                    PatientId = patient.Id,
                    Name = "حساسية الغبار"
                },
                new PatientAllergy
                {
                    PatientId = patient.Id,
                    Name = "حساسية حبوب اللقاح"
                },
                new PatientAllergy
                {
                    PatientId = patient.Id,
                    Name = "حساسية وبر الحيوانات"
                }
            };

            await _dbContext.PatientAllergies.AddRangeAsync(allergies);
            await _dbContext.SaveChangesAsync();

            // Create Current Medications
            var currentMedications = new List<PatientCurrentMedication>
            {
                new PatientCurrentMedication
                {
                    PatientId = patient.Id,
                    Name = "فينتولين (بخاخ سالبوتامول)"
                },
                new PatientCurrentMedication
                {
                    PatientId = patient.Id,
                    Name = "بوديزونيد (بخاخ)"
                },
                new PatientCurrentMedication
                {
                    PatientId = patient.Id,
                    Name = "مونتيلوكاست"
                }
            };

            await _dbContext.PatientCurrentMedications.AddRangeAsync(currentMedications);
            await _dbContext.SaveChangesAsync();
        }
    }
}