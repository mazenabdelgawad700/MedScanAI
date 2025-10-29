using MedScanAI.Domain.Entities;
using MedScanAI.Infrastructure.Abstracts;
using MedScanAI.Service.Abstracts;
using MedScanAI.Shared.Base;
using MedScanAI.Shared.SahredResponse;

namespace MedScanAI.Service.Implementation
{
    internal class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
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
        public async Task<ReturnBase<bool>> MakeAppointmentAsync(Appointment appointment)
        {
            try
            {
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
