using MediatR;
using MedScanAI.Shared.Base;
using MedScanAI.Shared.SahredResponse;

namespace MedScanAI.Core.Features.AppointmentFeature.Query.Model
{
    public class GetDoctorsForAppointmentsQuery : IRequest<ReturnBase<List<GetDoctorsForAppointmentsResponse>>>
    {
    }
}
