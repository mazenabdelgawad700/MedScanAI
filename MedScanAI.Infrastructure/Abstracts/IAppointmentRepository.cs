using MedScanAI.Domain.Entities;
using MedScanAI.Domain.IBaseRepository;
using MedScanAI.Shared.Base;
using MedScanAI.Shared.SahredResponse;

namespace MedScanAI.Infrastructure.Abstracts
{
    public interface IAppointmentRepository : IBaseRepository<Appointment>
    {
        Task<ReturnBase<List<GetDoctorsForAppointmentsResponse>>> GetDoctorsForAppointmentsAsync();
    }
}
