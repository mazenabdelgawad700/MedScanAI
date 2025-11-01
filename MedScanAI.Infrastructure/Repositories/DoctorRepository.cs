using MedScanAI.Domain.Entities;
using MedScanAI.Infrastructure.Abstracts;
using MedScanAI.Infrastructure.Context;
using MedScanAI.Infrastructure.RepositoryBase;
using MedScanAI.Shared.Base;
using MedScanAI.Shared.SharedResponse;
using Microsoft.EntityFrameworkCore;

namespace MedScanAI.Infrastructure.Repositories
{
    internal class DoctorRepository : BaseRepository<Doctor>, IDoctorRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<Doctor> _doctors;
        private readonly DbSet<Appointment> _appointments;
        public DoctorRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _doctors = _dbContext.Set<Doctor>();
            _appointments = _dbContext.Set<Appointment>();
        }

        //public async Task<ReturnBase<GetDoctorAppointmentsAndDoctorInfoResponse>> GetDoctorAppointmentsAndDoctorInfoAsync(string doctorId)
        //{
        //    try
        //    {
        //        var doctor = await _doctors.Where(x => x.Id == doctorId).FirstOrDefaultAsync();

        //        var appointments = await _appointments
        //            .Include(x => x.Patient)
        //            .Where(a => a.DoctorId == doctorId)
        //            .ToListAsync();


        //        var culture = new System.Globalization.CultureInfo("en-EG");

        //        var patientResponse = appointments.Select(a => new PatientResponse
        //        {
        //            PatientId = a.PatientId,
        //            AppointmentDate = a.Date.ToString("hh:mm tt", culture),
        //            Reason = a.Reason,
        //            PatientName = a.Patient.FullName,
        //            ChronicDiseases = a.Patient.ChronicDiseases.Select(x => x.Name).ToList(),
        //            Allergies = a.Patient.Allergies.Select(x => x.Name).ToList(),
        //            CurrentMedicine = a.Patient.CurrentMedications.Select(x => x.Name).ToList()
        //        }).ToList();



        //        var response = new GetDoctorAppointmentsAndDoctorInfoResponse
        //        {
        //            DoctorId = doctor?.Id,
        //            DoctorName = doctor?.FullName,
        //            Patients = patientResponse
        //        };

        //        return ReturnBaseHandler.Success(response);

        //    }
        //    catch (Exception ex)
        //    {
        //        return ReturnBaseHandler.Failed<GetDoctorAppointmentsAndDoctorInfoResponse>(ex.InnerException?.Message ?? ex.Message);
        //    }
        //}

        public async Task<ReturnBase<GetDoctorAppointmentsAndDoctorInfoResponse>> GetDoctorAppointmentsAndDoctorInfoAsync(string doctorId)
        {
            try
            {
                var doctor = await _doctors.FirstOrDefaultAsync(x => x.Id == doctorId);
                if (doctor == null)
                    return ReturnBaseHandler.Failed<GetDoctorAppointmentsAndDoctorInfoResponse>("Doctor not found");

                var today = DateTime.Today;
                var culture = new System.Globalization.CultureInfo("en-EG");

                var appointments = await _appointments
                    .Include(x => x.Patient)
                    .Include(x => x.Patient.Allergies)
                    .Include(x => x.Patient.ChronicDiseases)
                    .Include(x => x.Patient.CurrentMedications)
                    .Where(a => a.DoctorId == doctorId && a.Date.Date == today)
                    .ToListAsync();

                var patientResponse = appointments.Select(a => new PatientResponse
                {
                    PatientId = a.PatientId,
                    AppointmentDate = a.Date.ToString("hh:mm tt", culture),
                    Reason = a.Reason,
                    PatientName = a.PatientName,
                    ChronicDiseases = a.Patient?.ChronicDiseases?.Select(x => x.Name).ToList() ?? new List<string>(),
                    Allergies = a.Patient?.Allergies?.Select(x => x.Name).ToList() ?? new List<string>(),
                    CurrentMedicine = a.Patient?.CurrentMedications?.Select(x => x.Name).ToList() ?? new List<string>()
                }).ToList();

                var response = new GetDoctorAppointmentsAndDoctorInfoResponse
                {
                    DoctorId = doctor.Id,
                    DoctorName = doctor.FullName,
                    Patients = patientResponse
                };

                return ReturnBaseHandler.Success(response);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<GetDoctorAppointmentsAndDoctorInfoResponse>(
                    ex.InnerException?.Message ?? ex.Message
                );
            }
        }

    }
}