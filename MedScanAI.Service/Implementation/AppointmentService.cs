using MedScanAI.Domain.Entities;
using MedScanAI.Infrastructure.Abstracts;
using MedScanAI.Service.Abstracts;
using MedScanAI.Shared.Base;
using MedScanAI.Shared.SahredResponse;
using MedScanAI.Shared.SharedResponse;
using Microsoft.EntityFrameworkCore;

namespace MedScanAI.Service.Implementation
{
    internal class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository, IPatientRepository patientRepository)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
        }

        public async Task<ReturnBase<List<GetDoctorsForAppointmentsResponse>>> GetDoctorsForAppointmentsAsync()
        {
            try
            {
                var doctors = await _appointmentRepository.GetDoctorsForAppointmentsAsync();
                return ReturnBaseHandler.Success(doctors.Data!);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<List<GetDoctorsForAppointmentsResponse>>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<List<GetTodayAppointmentsResponse>>> GetTodaysAppointmentsAsync()
        {
            try
            {
                var doctors = await _appointmentRepository.GetTodayAppointmentsAsync();
                return ReturnBaseHandler.Success(doctors.Data!);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<List<GetTodayAppointmentsResponse>>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> MakeAppointmentAsync(Appointment appointment)
        {
            try
            {
                if (appointment.PatientId is not null)
                {
                    // Get the patient name and store it the appointment
                    var patientResult = await
                        _patientRepository.GetTableNoTracking()
                        .Data!.Where(x => x.Id == appointment.PatientId).FirstOrDefaultAsync();

                    appointment.PatientName = patientResult?.FullName;
                }

                var makeAppointmentResult = await _appointmentRepository.AddAsync(appointment);

                if (!makeAppointmentResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(makeAppointmentResult.Message);

                return ReturnBaseHandler.Success(true, "Appointment made successfully.");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
