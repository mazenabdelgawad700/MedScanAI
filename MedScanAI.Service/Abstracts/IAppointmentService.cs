using MedScanAI.Domain.Entities;
using MedScanAI.Shared.Base;
using MedScanAI.Shared.SahredResponse;

namespace MedScanAI.Service.Abstracts
{
    public interface IAppointmentService
    {
        Task<ReturnBase<bool>> MakeAppointmentAsync(Appointment appointment);
        Task<ReturnBase<List<GetDoctorsForAppointmentsResponse>>> GetDoctorsForAppointmentsAsync();
    }
}
