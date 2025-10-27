using MedScanAI.Domain.Entities;
using MedScanAI.Infrastructure.Abstracts;
using MedScanAI.Service.Abstracts;
using MedScanAI.Shared.Base;
using Microsoft.EntityFrameworkCore;

namespace MedScanAI.Service.Implementation
{
    internal class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<ReturnBase<bool>> DeleteDoctorAsync(string doctorId)
        {
            try
            {
                var doctor = await _doctorRepository.GetTableNoTracking().Data.Where(x => x.Id == doctorId).FirstOrDefaultAsync();

                if (doctor is null)
                    return ReturnBaseHandler.Failed<bool>("Doctor not found.");

                doctor.IsActive = false;
                await _doctorRepository.UpdateAsync(doctor);

                return ReturnBaseHandler.Success(true, "Doctor deleted successfully.");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> RestoreDoctorAsync(string doctorId)
        {
            try
            {
                var doctor = await _doctorRepository.GetTableNoTracking().Data!.Where(x => x.Id == doctorId).FirstOrDefaultAsync();

                if (doctor is null)
                    return ReturnBaseHandler.Failed<bool>("Doctor not found.");

                doctor.IsActive = true;
                await _doctorRepository.UpdateAsync(doctor);

                return ReturnBaseHandler.Success(true, "Doctor restored successfully.");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<IQueryable<Doctor>>> GetActiveDoctorsAsync()
        {
            try
            {
                var doctors = await _doctorRepository.GetTableNoTracking().Data!
                    .Where(x => x.IsActive)
                    .ToListAsync();

                return ReturnBaseHandler.Success(doctors.AsQueryable());
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<IQueryable<Doctor>>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<IQueryable<Doctor>>> GetAllDoctorsAsync()
        {
            try
            {
                var doctors = await _doctorRepository.GetTableNoTracking().Data!.ToListAsync();

                return ReturnBaseHandler.Success(doctors.AsQueryable());
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<IQueryable<Doctor>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

    }
}
