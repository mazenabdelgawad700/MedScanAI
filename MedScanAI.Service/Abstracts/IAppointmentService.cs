using MedScanAI.Domain.Entities;
using MedScanAI.Shared.Base;
using MedScanAI.Shared.SahredResponse;
using MedScanAI.Shared.SharedResponse;

namespace MedScanAI.Service.Abstracts
{
    public interface IAppointmentService
    {
        Task<ReturnBase<bool>> MakeAppointmentAsync(Appointment appointment);
        Task<ReturnBase<bool>> ConfirmAppointmentAsync(int appointmentId);
        Task<ReturnBase<List<GetDoctorsForAppointmentsResponse>>> GetDoctorsForAppointmentsAsync();
        Task<ReturnBase<List<GetTodayAppointmentsResponse>>> GetTodaysAppointmentsAsync();
    }
}
